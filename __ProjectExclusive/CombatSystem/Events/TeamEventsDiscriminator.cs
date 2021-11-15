using __ProjectExclusive.Player;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatSystem.Enemy;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.Events
{
    public class TeamEventsDiscriminator : ICombatPreparationListener, ICombatSystemEvents
    {
        private CombatingTeam _playerTeam;

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _playerTeam = playerTeam;
        }

        public void OnAfterLoadsCombat()
        {
        }

        private ICombatSystemEvents GetEventsHolder(CombatingEntity entity)
        {
            if (_playerTeam.Contains(entity))
                return PlayerCombatSingleton.PlayerEvents;
            return EnemyCombatSingleton.EventsHolder;
        }

        private ICombatSystemEvents GetEventsHolder(CombatingTeam team)
        {
            if(_playerTeam == team)
                return PlayerCombatSingleton.PlayerEvents;
            return EnemyCombatSingleton.EventsHolder;
        }

        public void OnFirstAction(CombatingEntity entity)
        {
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnFirstAction(entity);
        }

        public void OnFinishAction(CombatingEntity entity)
        {
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnFinishAction(entity);
        }

        public void OnFinishAllActions(CombatingEntity entity)
        {
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnFinishAllActions(entity);
        }


        public void OnShieldLost(ISkillParameters parameters, CombatingSkill value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnShieldLost(parameters, value);
        }
        public void OnHealthLost(ISkillParameters parameters, CombatingSkill value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnHealthLost(parameters,value);
        }
        public void OnStanceChange(CombatingTeam team, EnumTeam.TeamStance switchStance)
        {
            var eventsHolder = GetEventsHolder(team);
            eventsHolder.OnStanceChange(team,switchStance);
        }

        public void OnMemberDeath(CombatingTeam team, CombatingEntity member)
        {
            var eventsHolder = GetEventsHolder(team);
            eventsHolder.OnMemberDeath(team,member);
        }

        public void OnSkillUse(SkillValuesHolders values)
        {
            var entity = values.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnSkillUse(values);
        }

        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
            var entity = values.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnSkillCostIncreases(values);
        }

        public void OnBeforeAnimation(SkillValuesHolders element)
        {
            var entity = element.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnBeforeAnimation(element);
        }

        public void OnAnimationClimax(SkillValuesHolders element)
        {
            var entity = element.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnAnimationClimax(element);

        }

        public void OnAnimationHaltFinish(SkillValuesHolders element)
        {
            var entity = element.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnAnimationHaltFinish(element);

        }

        public void OnReceiveOffensiveAction(ISkillParameters parameters, CombatingSkill value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveOffensiveAction(parameters, value);
        }

        public void OnReceiveOffensiveEffect(ISkillParameters parameters, ref SkillComponentResolution value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveOffensiveEffect(parameters, ref value);
        }
        public void OnReceiveSupportAction(ISkillParameters parameters, CombatingSkill value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveSupportAction(parameters, value);
        }
        public void OnReceiveSupportEffect(ISkillParameters parameters, ref SkillComponentResolution value)
        {
            var entity = parameters.Performer;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveSupportEffect(parameters, ref value);
        }


        public void OnCantAct(CombatingEntity entity)
        {
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnCantAct(entity);
        }

        public void OnRoundFinish(CombatingEntity entity)
        {
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnRoundFinish(entity);
        }

    }
}
