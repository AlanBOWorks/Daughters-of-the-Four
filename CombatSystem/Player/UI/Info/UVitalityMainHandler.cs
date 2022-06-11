using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI.Info
{
    public class UVitalityMainHandler : UTeamDualAlimentHandler<UVitalityInfo>,
        ICombatStatesListener,
        IVitalityChangeListener, IDamageDoneListener, IRecoveryDoneListener

    {
        [SerializeField, SuffixLabel("px")] 
        private float playerHorizontalElementsSeparation = 12f;
        [SerializeField, SuffixLabel("px")] 
        private float enemyHorizontalElementsSeparation = 12f;

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
            playerPrefabs.HidePrefabs();
            enemyPrefabs.HidePrefabs();
        }

        public void HandleElementInjection(CombatEntity entity, UVitalityInfo element)
        {
            _dictionary.Add(entity, element);
            element.EntityInjection(in entity);
        }

        private void OnSpawnElement(CombatEntity entity, UVitalityInfo element, float horizontalSeparation , bool invertPosition)
        {
            var entityRole = entity.RoleType;
            var modifier = EnumTeam.GetRoleIndex(entityRole);
            var elementTransform = (RectTransform)element.transform;
            float size = elementTransform.rect.width;

            float horizontalPosition = (size + horizontalSeparation) * modifier;
            if (invertPosition) horizontalPosition = -horizontalPosition;

            var position = elementTransform.localPosition;
            position.x = horizontalPosition;
            elementTransform.localPosition = position;

            element.ShowElement();
            element.EntityInjection(in entity);
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            Action<CombatEntity,UVitalityInfo> onCreationAction = HandleElementInjection;

            onCreationAction += OnSpawnPlayerElement;
            HandlePlayerElements(playerTeam, onCreationAction);
            onCreationAction -= OnSpawnPlayerElement;

            onCreationAction += OnSpawnEnemyElement;
            HandleEnemyElements(enemyTeam, onCreationAction);

            void OnSpawnPlayerElement(CombatEntity entity, UVitalityInfo element)
            {
                OnSpawnElement(entity, element, playerHorizontalElementsSeparation, true);
            }

            void OnSpawnEnemyElement(CombatEntity entity, UVitalityInfo element)
            {
                OnSpawnElement(entity, element, enemyHorizontalElementsSeparation, false);
            }
        }
        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            _dictionary.Clear();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }


        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {

        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            var dictionary = GetDictionary();
            dictionary[entity].HideKnockOut();
        }

        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        { }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            var dictionary = GetDictionary();
            dictionary[target].ShowKnockOut();
        }


        public void OnShieldGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnHealthGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnMortalityGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnRecoveryReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTargetVitality(target);
        }

        public void OnKnockHeal(in CombatEntity performer, in CombatEntity target, in int currentTick, in int amount)
        {
            TickKnockOut(target, currentTick);
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
        private sealed class PlayerVitalityPrefabSpawners : ITeamAlimentStructureRead<VitalityPrefabSpawner>
        {
            [SerializeField]
            private VitalityPrefabSpawner mainRole = new VitalityPrefabSpawner();

            public VitalityPrefabSpawner MainRole => mainRole;
            public VitalityPrefabSpawner SecondaryRole => null;
            public VitalityPrefabSpawner ThirdRole => null;

            public void HidePrefabs()
            {
                mainRole.GetPrefab().gameObject.SetActive(false);
            }
        }
    }
}
