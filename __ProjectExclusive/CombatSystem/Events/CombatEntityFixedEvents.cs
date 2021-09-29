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
    public class CombatEntityFixedEvents : ICombatPreparationListener, IFullEventListener<SkillValuesHolders,CombatingEntity,EffectResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
        private CombatingTeam _playerTeam;
        private CombatingTeam _enemyTeam;

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
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
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            var stats = element.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);
            element.EventsHolder.OnDoMoreActions(element);
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            var stats = element.CombatStats;
            element.EventsHolder.OnFinishAllActions(element);
        }

        public void OnSkipActions(CombatingEntity element)
        {
            element.EventsHolder.OnSkipActions(element);
        }

        public void OnRoundFinish(CombatingEntity lastElement)
        {
            lastElement.EventsHolder.OnRoundFinish(lastElement);
            DoResetBurst(_enemyTeam);
            DoResetBurst(_playerTeam);


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
        public void OnPerformOffensiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            user.EventsHolder.OnPerformOffensiveAction(target,value);
        }
        public void OnReceiveOffensiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnReceiveOffensiveAction(user,value);
        }

        public void OnPerformSupportAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            user.EventsHolder.OnPerformSupportAction(target,value);
        }

        public void OnReceiveSupportAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnReceiveSupportAction(user,value);
        }

        public void OnRecoveryReceiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnRecoveryReceiveAction(user,value);
        }

        public void OnDamageReceiveAction(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnDamageReceiveAction(user,value);
        }

        public void OnShieldLost(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnShieldLost(user,value);
        }

        public void OnHealthLost(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnHealthLost(user,value);
        }

        public void OnMortalityDeath(SkillValuesHolders element, EffectResolution value)
        {
            ExtractEntities(element, out var user, out var target);
            target.EventsHolder.OnMortalityDeath(user,value);
        }
    }
}
