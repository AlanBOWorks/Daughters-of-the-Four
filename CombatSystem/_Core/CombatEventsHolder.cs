using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class SystemCombatEventsHolder : CombatEventsHolder
    {
        public SystemCombatEventsHolder() : base()
        {
            _eventHandlers = new HashSet<CombatEventsHolder>();
            _tempoTicker = new TempoTicker();

            CombatSystemSingleton.TempoTicker = _tempoTicker;
            Subscribe(_tempoTicker);
            Subscribe(_tempoTicker.EntitiesTempoTicker);

            var skillsUsageEventHandler = new CombatSkillEventHandler();
            Subscribe(skillsUsageEventHandler);

#if UNITY_EDITOR
            var debugEvents = new DebugEvents();

            Subscribe(debugEvents);
#endif
        }

        [TitleGroup("Combat System Exclusive"), ShowInInspector]
        private readonly TempoTicker _tempoTicker;

        // Problem: can't subscribe in static constructor and subscribing each time the combat
        // starts makes repeated subscriptions; 
        // Solution: just keep track of the combatEventsHandlers;
        [Title("Events Holder"), ShowInInspector, PropertyOrder(-10)]
        private readonly HashSet<CombatEventsHolder> _eventHandlers;

        public void SubscribeEventsHandler(CombatEventsHolder eventsHandler)
        {
            if (_eventHandlers.Contains(eventsHandler)) 
                return;

            _eventHandlers.Add(eventsHandler);
            base.Subscribe(eventsHandler);
        }


        protected override void SubscribeTempo(ITempoTickListener tickListener)
        {
            _tempoTicker.Subscribe(tickListener);
        }


#if UNITY_EDITOR
        #region -- DEBUG --
        private sealed class DebugEvents : ITempoEntityStatesListener, ITempoTickListener, ISkillUsageListener
        {
            public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
            {
                Debug.Log($"> --- Entity Sequence: {entity.GetEntityName()}");
            }

            public void OnEntityRequestControl(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [Request ACTION]: {entity.GetEntityName()}");
            }

            public void OnEntityFinishAction(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [End ACTION]: {entity.GetEntityName()}");
            }

            public void OnEntityFinishSequence(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [END] Sequence: {entity.GetEntityName()}");
            }

            public void OnStartTicking()
            {
                Debug.Log("xx - START Ticking");
            }

            public void OnTick()
            {
                Debug.Log("Tick");
            }

            public void OnRoundPassed()
            {
                Debug.Log("xx - Round Passed");
            }

            public void OnStopTicking()
            {
                Debug.Log("xx - STOP Ticking");
            }

            public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
            {

                Debug.Log($"----------------- SKILL --------------- \n" +
                          $"Random Controller: {performer.GetEntityName()} / " +
                          $"Used : {usedSkill.Preset} /" +
                          $"Target: {target.GetEntityName()}");
                Debug.Log($"ACTIONS LEFT: {performer.Stats.CurrentActions}");
            }

            public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
            {
                
            }

            public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
            {
                Debug.Log($"Effect performed  {performer.GetEntityName()} / On target: {target.GetEntityName()} ");
            }
        } 
        #endregion
#endif
    }

    public abstract class CombatEventsHolder : 
        ICombatPreparationListener,ICombatStatesListener, 
        ITempoEntityStatesListener,
        ISkillUsageListener
    {
        protected CombatEventsHolder()
        {
            CombatPreparationListeners = new HashSet<ICombatPreparationListener>();
            CombatStatesListeners = new HashSet<ICombatStatesListener>();
            TempoEntityListeners = new HashSet<ITempoEntityStatesListener>();
            SkillUsageListeners = new HashSet<ISkillUsageListener>();
        }
        

        [Title("Events")] 
        [ShowInInspector]
        protected readonly ICollection<ICombatPreparationListener> CombatPreparationListeners;
        [ShowInInspector]
        protected readonly ICollection<ICombatStatesListener> CombatStatesListeners;
        [ShowInInspector]
        protected readonly ICollection<ITempoEntityStatesListener> TempoEntityListeners;
        [ShowInInspector] 
        protected readonly ICollection<ISkillUsageListener> SkillUsageListeners;


        public void Subscribe(ICombatEventListener listener)
        {
            if(listener == null)
                throw new ArgumentNullException(nameof(listener), "Event listener can't be Null");

            if (listener is ICombatPreparationListener preparationListener)
                CombatPreparationListeners.Add(preparationListener);

            if(listener is ICombatStatesListener combatStatesListener)
                CombatStatesListeners.Add(combatStatesListener);

            if (listener is ITempoTickListener tickListener)
                SubscribeTempo(tickListener);

            if (listener is ITempoEntityStatesListener tempoEntityListener)
                TempoEntityListeners.Add(tempoEntityListener);

            if(listener is ISkillUsageListener skillUsageListener)
                SkillUsageListeners.Add(skillUsageListener);
        }

        protected abstract void SubscribeTempo(ITempoTickListener tickListener);


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var listener in CombatPreparationListeners)
            {
                listener.OnCombatPrepares(allMembers, playerTeam, enemyTeam);
            }
        }

        public void OnCombatStart()
        {
            foreach (var listener in CombatStatesListeners)
            {
                listener.OnCombatStart();
            }
        }

        public void OnCombatFinish()
        {
            foreach (var listener in CombatStatesListeners)
            {
                listener.OnCombatFinish();
            }
        }

        public void OnCombatQuit()
        {
            foreach (var listener in CombatStatesListeners)
            {
                listener.OnCombatQuit();
            }
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in TempoEntityListeners)
            {
                listener.OnEntityRequestSequence(entity, canAct);
            }
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            foreach (var listener in TempoEntityListeners)
            {
                listener.OnEntityRequestControl(entity);
            }
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            foreach (var listener in TempoEntityListeners)
            {
                listener.OnEntityFinishAction(entity);
            }
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            foreach (var listener in TempoEntityListeners)
            {
                listener.OnEntityFinishSequence(entity);
            }
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in SkillUsageListeners)
            {
                listener.OnSkillSubmit(in performer, in usedSkill, in target);
            }
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in SkillUsageListeners)
            {
                listener.OnSkillPerform(in performer, in usedSkill, in target);
            }
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            foreach (var listener in SkillUsageListeners)
            {
                listener.OnEffectPerform(in performer, in usedSkill, in target, in effect);
            }
        }
    }

    /// <summary>
    /// An eventListener interface to extend for (others eventListeners types should extend from this to
    /// be assorted correctly)
    /// </summary>
    public interface ICombatEventListener
    {

    }
}
