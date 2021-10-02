using System;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem.Events
{
    // This is invoked by the CombatEvents (implicitly by being subscribed to it)
    public class CombatEntityFixedEvents : ICombatPreparationListener, IEventListenerHandler<SkillValuesHolders,CombatingEntity,EffectResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
        private CombatingTeam _playerTeam;
        private CombatingTeam _enemyTeam;

        [ShowInInspector]
        private PlayerEvents _playerEvents;

        public void SubscribePlayerEvents(PlayerEvents events) => _playerEvents = events;

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            if(_playerEvents == null)
                throw new NullReferenceException("Player events weren't injected before the start of the combat");

            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }

        public void OnStartAndBeforeFirstTick()
        {
        }


        public void OnInitiativeTrigger(CombatingEntity element)
        {
            var stats = element.CombatStats;
            UtilsCombatStats.RefillActions(stats);
            UtilsCombatStats.InitiativeResetOnTrigger(stats);

            element.EventsHolder.OnInitiativeTrigger(element);
            element.SkillUsageTracker.ResetOnStartSequence();

            _playerEvents.OnInitiativeTrigger(element);
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            var stats = element.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);
            element.EventsHolder.OnDoMoreActions(element);

            _playerEvents.OnDoMoreActions(element);
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            element.EventsHolder.OnFinishAllActions(element);

            _playerEvents.OnFinishAllActions(element);
        }

        public void OnSkipActions(CombatingEntity element)
        {
            element.EventsHolder.OnSkipActions(element);

            _playerEvents.OnSkipActions(element);
        }

        public void OnRoundFinish(CombatingEntity lastElement)
        {
            lastElement.EventsHolder.OnRoundFinish(lastElement);
            DoResetBurst(_enemyTeam);
            DoResetBurst(_playerTeam);

            _playerEvents.OnRoundFinish(lastElement);

            // Privates 
            void DoResetBurst(CombatingTeam team)
            {
                foreach (CombatingEntity entity in team)
                {
                    UtilsCombatStats.ResetBurstStats(entity.CombatStats);
                }
            }
        }

        private static void ExtractEntities(SkillValuesHolders skillValues, out CombatingEntity user,
            out CombatingEntity target)
        {
            user = skillValues.User;
            target = skillValues.Target;
        }
        
        public void OnReceiveOffensiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnReceiveOffensiveAction(user,value);
            user.EventsHolder.OnPerformOffensiveAction(target,value);

            _playerEvents.OnReceiveOffensiveAction(element,value);
        }

        public void OnReceiveSupportAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnReceiveSupportAction(user,value);
            user.EventsHolder.OnPerformSupportAction(target,value);

            _playerEvents.OnReceiveSupportAction(element,value);
        }

        public void OnRecoveryReceiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            OnReceiveSupportAction(element,value);
            target.EventsHolder.OnRecoveryReceiveAction(user,value);
            
            _playerEvents.OnRecoveryReceiveAction(element, value);
        }

        public void OnDamageReceiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            OnReceiveOffensiveAction(element,value);
            target.EventsHolder.OnDamageReceiveAction(user,value);

            _playerEvents.OnDamageReceiveAction(element, value);
        }

        public void OnShieldLost(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            OnDamageReceiveAction(element,value);
            target.EventsHolder.OnShieldLost(user,value);

            _playerEvents.OnShieldLost(element, value);
        }

        public void OnHealthLost(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            OnDamageReceiveAction(element,value);
            target.EventsHolder.OnHealthLost(user,value);

            _playerEvents.OnHealthLost(element, value);
        }

        public void OnMortalityDeath(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            OnDamageReceiveAction(element,value);
            target.EventsHolder.OnMortalityDeath(user,value);
            target.Team.Events.OnMemberDeath(target);

            _playerEvents.OnMortalityDeath(element, value);
        }
    }
}
