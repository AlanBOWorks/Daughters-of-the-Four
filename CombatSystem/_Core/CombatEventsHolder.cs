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
    public sealed class SystemCombatEventsHolder 
        //: IEventsHolder
    {
        public SystemCombatEventsHolder() 
        {
            _eventsHolder = new SystemEventsHolder();
            var prefabInstantiations = new PrefabInstantiationHandler();
            _eventsHolder.Subscribe(prefabInstantiations);

#if UNITY_EDITOR
            var debugEvents = new DebugEvents();

            _eventsHolder.Subscribe(debugEvents);
#endif
        }

        // Problem: PlayerEvents could be invoked before core events while it should always be invoked last
        // so the data represented is the more consistent for the player;
        // Solution: Manual invoke through code
        private readonly SystemEventsHolder _eventsHolder;
        private PlayerCombatEventsHolder _playerCombatEvents;

        public void SubscribeEventsHandler(PlayerCombatEventsHolder eventsHandler)
        {
            _playerCombatEvents = eventsHandler;
        }

        public void SubscribeListener(ICombatEventListener listener) => _eventsHolder.Subscribe(listener);



        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _eventsHolder.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
            _playerCombatEvents.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
        }

        public void OnCombatStart()
        {
            _eventsHolder.OnCombatStart();
            _playerCombatEvents.OnCombatStart();
        }

        public void OnCombatFinish()
        {
            _eventsHolder.OnCombatFinish();
            _playerCombatEvents.OnCombatFinish();
        }

        public void OnCombatQuit()
        {
            _eventsHolder.OnCombatQuit();
            _playerCombatEvents.OnCombatQuit();
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _eventsHolder.OnEntityRequestSequence(entity,canAct);
            _playerCombatEvents.OnEntityRequestSequence(entity, canAct);
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            _eventsHolder.OnEntityRequestControl(entity);
            _playerCombatEvents.OnEntityRequestControl(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishAction(entity);
            _playerCombatEvents.OnEntityFinishAction(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishSequence(entity);
            _playerCombatEvents.OnEntityFinishSequence(entity);
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            _eventsHolder.OnSkillSubmit(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillSubmit(in performer, in usedSkill, in target);

            // Manual sequence Flow
            OnSkillPerform(in performer,in usedSkill, in target);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            _eventsHolder.OnSkillPerform(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillPerform(in performer, in usedSkill, in target);
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            _eventsHolder.OnEffectPerform(in performer,in usedSkill,in target, in effect);
            _playerCombatEvents.OnEffectPerform(in performer,in usedSkill,in target, in effect);
        }


        private sealed class SystemEventsHolder : CombatEventsHolder
        {
            public SystemEventsHolder()
            {
                _tempoTicker = new TempoTicker();

                CombatSystemSingleton.TempoTicker = _tempoTicker;
                Subscribe(_tempoTicker);
                Subscribe(_tempoTicker.EntitiesTempoTicker);

                var skillsUsageEventHandler = new CombatSkillEventHandler();
                Subscribe(skillsUsageEventHandler);
            }


            [TitleGroup("Combat System Exclusive"), ShowInInspector]
            private readonly TempoTicker _tempoTicker;
            protected override void SubscribeTempo(ITempoTickListener tickListener)
            {
                _tempoTicker.Subscribe(tickListener);
            }
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

    public interface IEventsHolder : ICombatPreparationListener, ICombatStatesListener,
        ITempoEntityStatesListener,
        ISkillUsageListener
    { }

    public abstract class CombatEventsHolder : IEventsHolder
        
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
