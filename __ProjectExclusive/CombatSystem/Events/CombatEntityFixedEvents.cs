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
        IOffensiveActionReceiverListener<CombatEntityPairAction,SkillComponentResolution>,
        ISupportActionReceiverListener<CombatEntityPairAction,SkillComponentResolution>
    {
#if UNITY_EDITOR
        [ShowInInspector,TextArea]
        private const string _behaviourExplanation = "A Sigleton event caller for fixed events that occurs always in the same " +
                                                     "pattern (It calls the entities's Events in the required order as a consequence)";
#endif
        public CombatEntityFixedEvents()
        {
            _entityEventInvoker = new CombatEntityEventInvoker();
        }

        private readonly CombatEntityEventInvoker _entityEventInvoker;


        public void OnFirstAction(CombatingEntity entity)
        {
#if UNITY_EDITOR
            Debug.Log($"---- >> INITIATIVE: {entity.GetEntityName()}");
#endif

            entity.GuardHandler.RemoveGuarding();
            entity.CombatStats.GetBurstStat().SelfBurst.ResetAsBurst();
            entity.SkillUsageTracker.ResetOnStartSequence();
            entity.SkillsHolder.TickCoolDowns();

            _entityEventInvoker.InjectUser(entity);
        }

        public void OnFinishAction(CombatingEntity entity)
        {
            var stats = entity.CombatStats;
            UtilsCombatStats.DecreaseActions(stats);

            _entityEventInvoker.InvokeEvents();

        }

        public void OnFinishAllActions(CombatingEntity entity)
        {
            entity.CombatStats.GetBurstStat().ReceivedBurst.ResetAsBurst();

            _entityEventInvoker.OnFinishAllActions();
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

        public void OnReceiveOffensiveAction(CombatEntityPairAction element, ref SkillComponentResolution value)
        {
            var receiver = element.Target;
            _entityEventInvoker.OnReceiveOffensiveAction(receiver);
        }

        public void OnReceiveSupportAction(CombatEntityPairAction element, ref SkillComponentResolution value)
        {
            var receiver = element.Target;
            _entityEventInvoker.OnReceiveOffensiveAction(receiver);
        }
    }

    internal sealed class CombatEntityEventInvoker : 
        IOffensiveActionReceiverListener<CombatingEntity>,
        ISupportActionReceiverListener<CombatingEntity>
    {
        public CombatEntityEventInvoker()
        {
            _offensiveCalls = new HashSet<CombatingEntity>();
            _supportCalls = new HashSet<CombatingEntity>();
        }

        private CombatingEntity _currentEntity;

        private readonly HashSet<CombatingEntity> _offensiveCalls;
        private readonly HashSet<CombatingEntity> _supportCalls;


        public void OnReceiveOffensiveAction(CombatingEntity receiver)
        {
            if(_offensiveCalls.Contains(receiver)) return;
            _offensiveCalls.Add(receiver);

        }
        public void OnReceiveSupportAction(CombatingEntity receiver)
        {
            if(_supportCalls.Contains(receiver)) return;
            _supportCalls.Add(receiver);

        }

        private void CallOffensiveListeners()
        {
            foreach (var receiver in _offensiveCalls)
            {
                receiver.EventsHolder.OnReceiveOffensiveAction(_currentEntity);
                receiver.Team.ProvokeHandler.OnReceiveOffensiveAction(receiver);
            }


            _offensiveCalls.Clear();
        }

        private void CallSupportListeners()
        {
            foreach (var receiver in _supportCalls)
            {
                receiver.EventsHolder.OnReceiveSupportAction(_currentEntity);
            }
            _supportCalls.Clear();
        }

        public void InjectUser(CombatingEntity user)
        {
            _currentEntity = user;
        }

        public void InvokeEvents()
        {
            CallOffensiveListeners();
            CallSupportListeners();
        }

        public void OnFinishAllActions()
        {
            _currentEntity = null;
        }
    }
}
