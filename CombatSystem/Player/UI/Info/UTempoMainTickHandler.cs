using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UTempoMainTickHandler : UTeamDualAlimentHandler<UTempoTrackerHolder>, 
        ICombatStartListener, ICombatTerminationListener,
        ITempoEntityPercentListener,
        ITempoEntityStatesExtraListener,
        IStatsChangeListener
    {
        [SerializeField]
        private PlayerPrefabHolder playerPrefabs = new PlayerPrefabHolder();
        [SerializeField] 
        private EnemyPrefabsHolder enemyPrefabs = new EnemyPrefabsHolder();


        [Title("Params")] 
        [SerializeField] private float verticalSpacing = 32;

        [Title("Trackers")]
        [ShowInInspector,DisableInEditorMode]
        private Dictionary<CombatEntity, UTempoTrackerHolder> _dictionary;

        public IReadOnlyDictionary<CombatEntity, UTempoTrackerHolder> GetDictionary() => _dictionary;


        private Color _backgroundInitialColor;
        private Color _stepsTextInitialColor;
        private Color _iconInitialColor;

        private void Start()
        {
            var prefab = playerPrefabs.MainRole.GetPrefab();
            _backgroundInitialColor = prefab.GetBackgroundHolder().color;
            _stepsTextInitialColor = prefab.GetStepTextHolder().color;
            _iconInitialColor = prefab.GetBackgroundIconHolder().color;


            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            _dictionary = new Dictionary<CombatEntity, UTempoTrackerHolder>();
            
            UtilsTeamPrefabs.TryHide(playerPrefabs);
            UtilsTeamPrefabs.TryHide(enemyPrefabs);
        }

        public override ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<UTempoTrackerHolder>> PlayerTeamType
            => playerPrefabs;

        public override ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<UTempoTrackerHolder>> EnemyTeamType
            => enemyPrefabs;


        private static void HideActiveTracker(UTempoTrackerHolder holder)
        {
            holder.HideElement();
        }
        public void OnCombatEnd()
        {
            Action<UTempoTrackerHolder> onFinishCallBack = HideActiveTracker;

            _dictionary.Clear();
            ReturnElements(onFinishCallBack);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {

            Action<CombatEntity, UTempoTrackerHolder> onCreateCallback = HandleEntity;
            HandleElements(playerTeam, enemyTeam, onCreateCallback);
        }

        public void OnCombatStart()
        {
        }


        private void HandleEntity(CombatEntity entity, UTempoTrackerHolder element)
        {
            _dictionary.Add(entity,element);

            var entityRole = entity.RoleType;
            var targetHeight = TargetHeight(entityRole);
            HandleHeight(element, targetHeight);
            element.EntityInjection(entity);
            element.OnInstantiation();


            var rolesThemes = CombatThemeSingleton.RolesThemeHolder;
            var entityTheme = UtilsTeam.GetElement(entityRole, rolesThemes);
            element.Injection(entityTheme.GetThemeIcon());
            element.Injection(entityTheme.GetThemeColor());

            DoFinishTempo(element);
        }

        private static void HandleHeight(Component element, float targetHeight)
        {
            var elementTransform = element.transform;
            var position = elementTransform.localPosition;
            position.y = targetHeight;
            elementTransform.localPosition = position;
        }

        private float TargetHeight(EnumTeam.Role role)
        {
            return -EnumTeam.GetRoleIndex(role) * verticalSpacing;
        }


        public void OnEntityTick(in TempoTickValues values)
        {
            var entity = values.Entity;
            if(!_dictionary.ContainsKey(entity)) return;

            _dictionary[entity].TickTempo(in values);
        }

        public void OnBuffDone(EntityPairInteraction entities, IBuffEffect buff, float effectValue)
        {
            var target = entities.Target;
            if (!_dictionary.ContainsKey(target)) return;
            _dictionary[target].UpdateToCurrent();
        }

        public void OnDeBuffDone(EntityPairInteraction entities, IDeBuffEffect deBuff, float effectValue)
        {
            var target = entities.Target;
            if (!_dictionary.ContainsKey(target)) return;
            _dictionary[target].UpdateToCurrent();

        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            if (!_dictionary.ContainsKey(entity)) return;

            var tempoInfo = _dictionary[entity];
            tempoInfo.OnControlStart();

            int steps = UtilsCombatStats.CalculateRemainingSteps(entity.Stats);

            // ---- Problem:
            // After sequenceRequest the entity requires an another tick to perform its
            // sequence phase (this is by design), but the calculations are made as if the entity had already finished
            // its sequence and it calculates the next tick
            // ticking in the TempoHandler
            // ---- Solution:
            // Just +1 the step count so it shows the real count
            tempoInfo.UpdateStepsText(steps + 1);
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            if (!_dictionary.ContainsKey(entity)) return;
            DoFinishTempo(_dictionary[entity]);
        }

        private void DoFinishTempo(UTempoTrackerHolder element)
        {
            element.OnSequenceFinish(in _backgroundInitialColor, in _stepsTextInitialColor, in _iconInitialColor);
        }


        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
        }


        [Serializable]
        private class PrefabsPoolHandler : PrefabInstantiationHandlerPool<UTempoTrackerHolder>
        {
            
        }

        [Serializable]
        private sealed class PlayerPrefabHolder : TeamAlimentStructureMainOnlyClass<PrefabInstantiationHandlerPool<UTempoTrackerHolder>>
        {
           
        }

        [Serializable]
        private sealed class EnemyPrefabsHolder : TeamAlimentStructureClass<PrefabInstantiationHandlerPool<UTempoTrackerHolder>>
        {
            
        }

    }
}
