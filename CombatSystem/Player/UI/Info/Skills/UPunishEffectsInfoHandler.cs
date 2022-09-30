using System;
using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UPunishEffectsInfoHandler : MonoBehaviour, 
        IOppositionTeamStructureRead<UPunishEffectsTooltipWindowHandler>,
        IVanguardEffectUsageListener,
        ITempoControlStatesListener,
        ICombatTerminationListener
    {
        [SerializeField] 
        private UPunishEffectsTooltipWindowHandler playerEffectTooltipsHandler;
        [SerializeField] 
        private UPunishEffectsTooltipWindowHandler enemyEffectTooltipsHandler;

        private void Awake()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }


        private void OnDestroy()
        {
            CombatSystemSingleton.EventsHolder.UnSubscribe(this);
        }

        public UPunishEffectsTooltipWindowHandler PlayerTeamType 
            => playerEffectTooltipsHandler;
        public UPunishEffectsTooltipWindowHandler EnemyTeamType 
            => enemyEffectTooltipsHandler;

        private UPunishEffectsTooltipWindowHandler GetWindowHandler(bool isPlayer)
        {
            return isPlayer ? playerEffectTooltipsHandler : enemyEffectTooltipsHandler;
        }

        private UPunishEffectsTooltipWindowHandler GetWindowHandler(CombatEntity entity)
        {
            return GetWindowHandler(entity.Team.IsPlayerTeam);
        }

        public void OnVanguardSkillSubscribe(IVanguardSkill skill, CombatEntity performer)
        {
            var window = GetWindowHandler(performer);
            window.AddVanguardEffects(skill);
        }

        public void OnVanguardEffectsPerform(CombatEntity attacker, CombatEntity onTarget)
        { }

        public void OnVanguardEffectPerform(EnumsEffect.TargetType targetType, VanguardEffectUsageValues values)
        { }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            var team = controller.ControllingTeam;
            bool isMainActive = team.PunishEffectsHolder.GetMainEntity().IsActive();
            if (!isMainActive) return;

            var window = GetWindowHandler(team.IsPlayerTeam);
            window.ReleaseElements();
        }
        public void OnAllActorsNoActions(CombatEntity lastActor)
        { }
        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        { }


        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            playerEffectTooltipsHandler.ReleaseElements();
            enemyEffectTooltipsHandler.ReleaseElements();
        }

    }
}
