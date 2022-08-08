using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVitalityLateralHandler : MonoBehaviour,
        ITeamFlexStructure<UHealthInfo>,
        ITeamMainStructureInstantiationListener<UCombatEntitySwitchButton>,
        IDamageDoneListener, IRecoveryDoneListener,
        ICombatStartListener
    {
        [ShowInInspector, DisableInEditorMode]
        public UHealthInfo VanguardType { get; set; }
        [ShowInInspector, DisableInEditorMode]
        public UHealthInfo AttackerType { get; set; }
        [ShowInInspector, DisableInEditorMode]
        public UHealthInfo SupportType { get; set; }
        [ShowInInspector, DisableInEditorMode]
        public UHealthInfo FlexType { get; set; }


        public void OnInstantiateElement(UCombatEntitySwitchButton element, EnumTeam.Role role)
        {
            var healthInfo = element.GetComponent<UHealthInfo>();
            UtilsTeam.Injection(this, healthInfo, role);
        }


        [SerializeField] private bool isPlayerHandler = true;

        private ICombatEventsHolder GetEventHolder() => isPlayerHandler
            ? PlayerCombatSingleton.PlayerCombatEvents
            : (ICombatEventsHolder) EnemyCombatSingleton.EnemyEventsHolder;


        private void Awake()
        {
            GetEventHolder().Subscribe(this);
        }

        private void OnDestroy()
        {
            GetEventHolder().UnSubscribe(this);
        }

        private void UpdateTarget(CombatEntity target)
        {
            if(target.Team.IsPlayerTeam != isPlayerHandler) return;

            var targetElement = UtilsTeam.GetElement(target.RoleType, this);
            targetElement.UpdateHealth(target);
        }


        public void OnShieldLost(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnHealthLost(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnDamageReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTarget(target);
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            UpdateTarget(target);
        }

        public void OnShieldGain(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnHealthGain(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnMortalityGain(CombatEntity performer, CombatEntity target, float amount)
        {
            UpdateTarget(target);
        }

        public void OnRecoveryReceive(CombatEntity performer, CombatEntity target)
        {
            UpdateTarget(target);
        }

        public void OnKnockHeal(EntityPairInteraction entities, int currentTick, int amount)
        {
            UpdateTarget(entities.Target);
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            var team = isPlayerHandler ? playerTeam : enemyTeam;
            foreach (var member in team.GetAllMembers())
            {
                FirstUpdate(member);
            }

            void FirstUpdate(CombatEntity target)
            {
                if (target.Team.IsPlayerTeam != isPlayerHandler) return;

                var targetElement = UtilsTeam.GetElement(target.RoleType, this);
                targetElement.FirstInjectionHealth(target);
            }
        }

        public void OnCombatStart()
        {
        }
    }
}
