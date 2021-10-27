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
    public class CombatEntityFixedEvents :ITempoListener<CombatingEntity>, IRoundListener<CombatingEntity>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
       

        public void OnFirstAction(CombatingEntity element)
        {
            element.CombatStats.GetBurstStat().SelfBurst.ResetAsBurst();
            element.SkillUsageTracker.ResetOnStartSequence();
            element.SkillsHolder.TickCoolDowns();
        }

        public void OnFinishAction(CombatingEntity element)
        {
            var stats = element.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);

        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            element.CombatStats.GetBurstStat().ReceivedBurst.ResetAsBurst();
            element.GuardHandler.RemoveGuarding();
        }


        public void OnRoundFinish(CombatingEntity lastElement)
        {
            ResetTeamBurst();

            void ResetTeamBurst()
            {
                CombatSystemSingleton.VolatilePlayerTeam.DoResetBurst();
                CombatSystemSingleton.VolatileEnemyTeam.DoResetBurst();
            }
        }

       
        
    }
}
