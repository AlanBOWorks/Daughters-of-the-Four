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


        public void OnShieldLost(CombatEntityPairAction pair, CombatingSkill value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnShieldLost(pair, value);
        }
        public void OnHealthLost(CombatEntityPairAction pair, CombatingSkill value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnHealthLost(pair,value);
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

        public void OnReceiveOffensiveAction(CombatEntityPairAction pair, CombatingSkill value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveOffensiveAction(pair,value);
        }

        public void OnReceiveOffensiveEffect(CombatEntityPairAction pair, ref SkillComponentResolution value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveOffensiveEffect(pair,ref value);
        }
        public void OnReceiveSupportAction(CombatEntityPairAction pair, CombatingSkill value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveSupportAction(pair, value);
        }
        public void OnReceiveSupportEffect(CombatEntityPairAction pair, ref SkillComponentResolution value)
        {
            var entity = pair.User;
            var eventsHolder = GetEventsHolder(entity);
            eventsHolder.OnReceiveSupportEffect(pair, ref value);
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
