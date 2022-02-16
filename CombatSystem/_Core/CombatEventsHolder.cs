using System;
using System.Collections.Generic;
using CombatSystem.AI;
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
        private ControllerCombatEventsHolder _playerCombatEvents;
        private ControllerCombatEventsHolder _enemyCombatEvents;
        private CombatEntityEventsHolder _currentDiscriminatedEntityEventsHolder;

        public void SubscribeEventsHandler(PlayerCombatEventsHolder eventsHandler)
        {
            _playerCombatEvents = eventsHandler;
        }

        public void SubscribeEventsHandler(EnemyCombatEventsHolder eventsHandler)
        {
            _enemyCombatEvents = eventsHandler;
        }


        public void SubscribeListener(ICombatEventListener listener) => _eventsHolder.Subscribe(listener);

        private void HandleCurrentEntityEventsHolder(in CombatEntity entity)
        {
            bool isPlayerEntity = CombatSystemSingleton.PlayerTeam.Contains(entity);
            _currentDiscriminatedEntityEventsHolder = isPlayerEntity 
                ? _playerCombatEvents.DiscriminationEventsHolder 
                : _enemyCombatEvents.DiscriminationEventsHolder;
        }




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

            HandleCurrentEntityEventsHolder(in entity);
            _currentDiscriminatedEntityEventsHolder.OnEntityRequestSequence(entity,canAct);
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            _eventsHolder.OnEntityRequestControl(entity);
            _playerCombatEvents.OnEntityRequestControl(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityRequestControl(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishAction(entity);
            _playerCombatEvents.OnEntityFinishAction(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishAction(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishSequence(entity);
            _playerCombatEvents.OnEntityFinishSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishSequence(entity);
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, CombatEntity target)
        {
            _eventsHolder.OnSkillSubmit(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillSubmit(in performer, in usedSkill, in target);

            _currentDiscriminatedEntityEventsHolder.OnSkillSubmit(in performer, in usedSkill, target);

            // Manual sequence Flow
            OnSkillPerform(in performer,in usedSkill, in target);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            _eventsHolder.OnSkillPerform(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillPerform(in performer, in usedSkill, in target);

            _currentDiscriminatedEntityEventsHolder.OnSkillPerform(in performer, in usedSkill, in target);

            //todo wait until skill ends (animation)
            OnEntityFinishAction(performer);
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            _eventsHolder.OnEffectPerform(in performer,in usedSkill,in target, in effect);
            _playerCombatEvents.OnEffectPerform(in performer,in usedSkill,in target, in effect);

            _currentDiscriminatedEntityEventsHolder.OnEffectPerform(in performer, in usedSkill, target, effect);
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
                Debug.Log($"ACTIONS Used: {performer.Stats.UsedActions}");
            }

            public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
            {

            }

            public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
            {
                Debug.Log($"Effect performed  {performer.GetEntityName()} / On target: {target.GetEntityName()} ");
            }

            public void OnSkillFinish()
            {
                Debug.Log("-------------- SKILL END --------------- ");
            }
        }
        #endregion
#endif
    }


    public abstract class ControllerCombatEventsHolder : CombatEventsHolder
    {
        private protected ControllerCombatEventsHolder() : base()
        {
            _tempoTickListeners = new HashSet<ITempoTickListener>();

            DiscriminationEventsHolder = new CombatEntityEventsHolder();
        }

        [ShowInInspector]
        private readonly HashSet<ITempoTickListener> _tempoTickListeners;

        public readonly CombatEntityEventsHolder DiscriminationEventsHolder;

        protected override void SubscribeTempo(ITempoTickListener tickListener)
        {
            _tempoTickListeners.Add(tickListener);
        }

        public void OnStartTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStartTicking();
            }
        }

        public void OnTick()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnTick();
            }
        }

        public void OnRoundPassed()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnRoundPassed();
            }
        }

        public void OnStopTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStopTicking();
            }
        }

        public CombatEntityEventsHolder GetDiscriminationEventsHolder() => DiscriminationEventsHolder;
    }

    public abstract class CombatEventsHolder : CombatEntityEventsHolder,IEventsHolder, ICombatEntityExistenceListener

    {
        protected CombatEventsHolder() : base()
        {
            _combatPreparationListeners = new HashSet<ICombatPreparationListener>();
            _combatStatesListeners = new HashSet<ICombatStatesListener>();
            _entitiesExistenceListeners = new HashSet<ICombatEntityExistenceListener>();
        }
        

        [Title("Events")] 
        [ShowInInspector]
        private readonly ICollection<ICombatPreparationListener> _combatPreparationListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatStatesListener> _combatStatesListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatEntityExistenceListener> _entitiesExistenceListeners;



        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);

            if (listener is ICombatPreparationListener preparationListener)
                _combatPreparationListeners.Add(preparationListener);

            if(listener is ICombatStatesListener combatStatesListener)
                _combatStatesListeners.Add(combatStatesListener);

            if (listener is ITempoTickListener tickListener)
                SubscribeTempo(tickListener);

        }

        protected abstract void SubscribeTempo(ITempoTickListener tickListener);


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var listener in _combatPreparationListeners)
            {
                listener.OnCombatPrepares(allMembers, playerTeam, enemyTeam);
            }
        }

        public void OnCombatStart()
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatStart();
            }
        }

        public void OnCombatFinish()
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatFinish();
            }
        }

        public void OnCombatQuit()
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatQuit();
            }
        }

        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var listener in _entitiesExistenceListeners)
            {
                listener.OnCreateEntity(in entity, in isPlayers);
            }
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var listener in _entitiesExistenceListeners)
            {
                listener.OnDestroyEntity(in entity, in isPlayers);
            }
        }
    }

    public class CombatEntityEventsHolder : ITempoEntityStatesListener, ISkillUsageListener
    {
        public CombatEntityEventsHolder()
        {
            _tempoEntityListeners = new HashSet<ITempoEntityStatesListener>();
            _skillUsageListeners = new HashSet<ISkillUsageListener>();
        }

        [ShowInInspector] private readonly ICollection<ITempoEntityStatesListener> _tempoEntityListeners;
        [ShowInInspector] private readonly ICollection<ISkillUsageListener> _skillUsageListeners;


        public virtual void Subscribe(ICombatEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Event listener can't be Null");


            if (listener is ITempoEntityStatesListener tempoEntityListener)
                _tempoEntityListeners.Add(tempoEntityListener);

            if (listener is ISkillUsageListener skillUsageListener)
                _skillUsageListeners.Add(skillUsageListener);
        }

        public void ManualSubscribe(ITempoEntityStatesListener tempoEntityListener)
        {
            _tempoEntityListeners.Add(tempoEntityListener);
        }
        public void ManualSubscribe(ISkillUsageListener skillUsageListener)
        {
            _skillUsageListeners.Add(skillUsageListener);
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityRequestSequence(entity, canAct);
            }
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityRequestControl(entity);
            }
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishAction(entity);
            }
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishSequence(entity);
            }
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillSubmit(in performer, in usedSkill, in target);
            }
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillPerform(in performer, in usedSkill, in target);
            }
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnEffectPerform(in performer, in usedSkill, in target, in effect);
            }
        }

        public void OnSkillFinish()
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillFinish();
            }
        }
    }


    public interface IEventsHolder : ICombatPreparationListener, ICombatStatesListener,
        ICombatEntityExistenceListener,
        ITempoEntityStatesListener,
        ISkillUsageListener
    { }


    public interface ICombatEntityExistenceListener : ICombatEventListener
    {
        void OnCreateEntity(in CombatEntity entity, in bool isPlayers);
        void OnDestroyEntity(in CombatEntity entity, in bool isPlayers);
    }

    /// <summary>
    /// EventsHolder that will be summon if the associated [<see cref="CombatEntity"/>] belongs to them
    /// </summary>
    public interface IDiscriminationEventsHolder
    {
        CombatEntityEventsHolder GetDiscriminationEventsHolder();
    }


    /// <summary>
    /// An eventListener interface to extend from (others eventListeners types should extend from this to
    /// be assorted correctly)
    /// </summary>
    public interface ICombatEventListener
    {

    }
}
