using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI.Info
{
    public class UVitalityMainHandler : UTeamDualAlimentHandler<UVitalityInfo>,
        ICombatStartListener, ICombatTerminationListener,
        IVitalityChangeListener, IDamageDoneListener, IRecoveryDoneListener

    {
        [SerializeField, SuffixLabel("px")] 
        private float playerHorizontalMargin = -126f - 12;
        [SerializeField, SuffixLabel("px")] 
        private float enemyHorizontalMargin = 126f + 12;

        [SerializeField]
        private PlayerVitalityPrefabSpawners playerPrefabs = new PlayerVitalityPrefabSpawners();
        [SerializeField]
        private EnemyVitalityPrefabSpawners enemyPrefabs = new EnemyVitalityPrefabSpawners();

        [ShowInInspector, DisableInEditorMode] 
        private Dictionary<CombatEntity, UVitalityInfo> _dictionary;
        private IReadOnlyDictionary<CombatEntity, UVitalityInfo> GetDictionary() => _dictionary;


        public override ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<UVitalityInfo>> PlayerTeamType =>
            playerPrefabs;

        public override ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<UVitalityInfo>> EnemyTeamType =>
            enemyPrefabs;


        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            _dictionary = new Dictionary<CombatEntity, UVitalityInfo>();


            UtilsTeamPrefabs.TryHide(playerPrefabs);
            UtilsTeamPrefabs.TryHide(enemyPrefabs);
        }


        private void OnSpawnElement(CombatEntity entity, UVitalityInfo element, float horizontalSeparation)
        {
            var entityRole = entity.RoleType;
            var modifier = EnumTeam.GetRoleIndex(entityRole);
            var elementTransform = (RectTransform)element.transform;

            float horizontalPosition = (horizontalSeparation) * modifier;

            var position = elementTransform.localPosition;
            position.x = horizontalPosition;
            elementTransform.localPosition = position;

            element.ShowElement();
            element.EntityInjection(entity);

            _dictionary.Add(entity, element);
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            HandlePlayerElements(playerTeam, OnSpawnPlayerElement);
            HandleEnemyElements(enemyTeam, OnSpawnEnemyElement);


            void OnSpawnPlayerElement(CombatEntity entity, UVitalityInfo element)
            {
                OnSpawnElement(entity, element, playerHorizontalMargin);
            }
            void OnSpawnEnemyElement(CombatEntity entity, UVitalityInfo element)
            {
                OnSpawnElement(entity, element, enemyHorizontalMargin);
            }
        }
        public void OnCombatStart()
        {
        }


        private static void HideVitality(UVitalityInfo element)
        {
            element.HideElement();
        }

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            _dictionary.Clear();
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            Action<UVitalityInfo> onFinishAction = HideVitality;
            ReturnElements(onFinishAction);
        }


        public void OnDamageBeforeDone(CombatEntity performer, CombatEntity target, float amount)
        {

        }

        public void OnRevive(CombatEntity entity, bool isHealRevive)
        {
            var dictionary = GetDictionary();
            dictionary[entity].HideKnockOut();
        }

        public void OnShieldLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnHealthLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount)
        { }

        public void OnDamageReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].ShowKnockOut();
        }


        public void OnShieldGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnHealthGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnMortalityGain(CombatEntity performer, CombatEntity target, float amount)
        {
        }

        public void OnRecoveryReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockHeal(EntityPairInteraction entities, int currentTick, int amount)
        {
            TickKnockOut(entities.Target, currentTick);
        }


        private void UpdateTargetVitality(CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].UpdateToCurrentStats();
        }

        private void TickKnockOut(CombatEntity target, int tick)
        {
            var dictionary = GetDictionary();
            dictionary[target].UpdateKnockOut(tick);
        }

        [Serializable]
        private sealed class VitalityPrefabSpawner : PrefabInstantiationHandlerPool<UVitalityInfo>
        {
            

        }

        [Serializable]
        private sealed class EnemyVitalityPrefabSpawners : TeamAlimentStructureClass<VitalityPrefabSpawner>
        {
            public void HidePrefabs()
            {
                var enumerable = UtilsTeam.GetEnumerable(this);
                foreach (var spawner in enumerable)
                {
                    spawner.GetPrefab().gameObject.SetActive(false);
                }
            }
        }

        [Serializable]
        private sealed class PlayerVitalityPrefabSpawners : TeamAlimentStructureMainOnlyClass<VitalityPrefabSpawner>
        {
            public void HidePrefabs()
            {
                mainRole.GetPrefab().gameObject.SetActive(false);
            }
        }
    }
}
