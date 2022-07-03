using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UTempoMainTickHandler : UTeamDualAlimentHandler<UTempoTrackerHolder>, 
        ICombatStatesListener, ITempoEntityPercentListener
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


        private void Start()
        {
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
            holder.DisableElement();
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


            var rolesTheme = CombatThemeSingleton.RolesThemeHolder;
            var roleIcon = UtilsTeam.GetElement(entityRole, rolesTheme);
            element.Injection(roleIcon.GetThemeIcon());
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
