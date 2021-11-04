using System;
using System.Collections.Generic;
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
    public class CombatEntityFixedEvents :
        ITempoListener<CombatingEntity>, 
        IRoundListener<CombatingEntity>,
        IOffensiveActionReceiverListener<CombatEntityPairAction,CombatingSkill,SkillComponentResolution>,
        ISupportActionReceiverListener<CombatEntityPairAction, CombatingSkill, SkillComponentResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
       


        public void OnFirstAction(CombatingEntity entity)
        {
#if UNITY_EDITOR
            Debug.Log($"---- >> INITIATIVE: {entity.GetEntityName()}");
#endif

            entity.GuardHandler.RemoveGuarding();
            entity.CombatStats.GetBurstStat().SelfBurst.ResetAsBurst();
            entity.SkillUsageTracker.ResetOnStartSequence();
        }

        public void OnFinishAction(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);

        }

        public void OnFinishAllActions(CombatingEntity entity)
        {
            entity.CombatStats.GetBurstStat().ReceivedBurst.ResetAsBurst();
            entity.SkillsHolder.ResetCosts();

            UtilsCombatStats.DoCurrentPersistentDamage(entity.CombatStats);

        }


        public void OnRoundFinish(CombatingEntity lastEntity)
        {
            ResetTeamBurst();

            void ResetTeamBurst()
            {
                CombatSystemSingleton.VolatilePlayerTeam.DoResetBurst();
                CombatSystemSingleton.VolatileEnemyTeam.DoResetBurst();
            }
        }

        public void OnReceiveOffensiveAction(CombatEntityPairAction element, CombatingSkill skill)
        {
            // todo call for reactionEvents
        }

        public void OnReceiveOffensiveEffect(CombatEntityPairAction element, ref SkillComponentResolution effectResolution)
        {
           
        }

        public void OnReceiveSupportAction(CombatEntityPairAction element, CombatingSkill skill)
        {
            // todo call for reactionEvents
        }

        public void OnReceiveSupportEffect(CombatEntityPairAction element, ref SkillComponentResolution effectResolution)
        {
            
        }
    }

}
