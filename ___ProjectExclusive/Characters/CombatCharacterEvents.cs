using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using CombatEffects;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// It's an invoker for [<see cref="ICharacterEventListener"/>] that will be invoked several times
    /// by any way that revolves a 'Character' (such HP changes, stats, buffs and others).<br></br>
    /// Unlike [<seealso cref="ITempoListener"/>] that requires Ticking and will
    /// be invoked in the specific [<seealso cref="CombatingEntity"/>]'s turn, this listeners
    /// can be invoked by anyone and in anytime. Thus this won't be included in the
    /// [<seealso cref="TempoHandler"/>] since this Handler only triggers the User (and not targets)
    /// and that's not the desired behaviour.
    /// <br></br>
    /// <br></br> _____ TL;DR _____ <br></br>
    /// [<see cref="ICharacterEventListener"/>]:
    /// can be invoked by anyone and anywhere (generally an User will invoke 
    /// a specific target's listeners)<br></br>
    /// [<seealso cref="ITempoListener"/>]:
    /// is deterministic and will only be invoked in one specific Entity that was triggered.
    /// </summary>
    public class CombatCharacterEventsBase : ITempoListenerVoid, IHealthZeroListener
    {
        [ShowInInspector]
        private List<ITempoListenerVoid> _onTempoListeners;
        [ShowInInspector]
        private List<IVitalityChangeListener> _onVitalityChange;
        [ShowInInspector] 
        private List<ITemporalStatChangeListener> _onTemporalStatChange;
        [ShowInInspector]
        private List<ICombatHealthChangeListener> _onCombatHealthChange;
        [ShowInInspector]
        private List<IAreaStateChangeListener> _onAreaChange;

        [ShowInInspector]
        private List<IHealthZeroListener> _onHealthZeroListeners;

        public CombatCharacterEventsBase()
        {
            _onTempoListeners = new List<ITempoListenerVoid>();
            
            _onVitalityChange = new List<IVitalityChangeListener>();
            _onTemporalStatChange = new List<ITemporalStatChangeListener>();
            _onCombatHealthChange = new List<ICombatHealthChangeListener>();
            _onAreaChange = new List<IAreaStateChangeListener>();
            
            _onHealthZeroListeners = new List<IHealthZeroListener>();
        }

        public void Subscribe(ICharacterEventListener listener)
        {
            if (listener is ITempoListenerVoid tempoListener)
                _onTempoListeners.Add(tempoListener);

            if (listener is IVitalityChangeListener vitalityListener)
                _onVitalityChange.Add(vitalityListener);
            if (listener is ITemporalStatChangeListener temporalStatListener)
                _onTemporalStatChange.Add(temporalStatListener);
            if (listener is ICombatHealthChangeListener healthChangeListener)
                _onCombatHealthChange.Add(healthChangeListener);
            if (listener is IAreaStateChangeListener areaStateListener)
                _onAreaChange.Add(areaStateListener);

            if (listener is IHealthZeroListener healthCheckListener)
                _onHealthZeroListeners.Add(healthCheckListener);
        }



        public void InvokeVitalityChange(CombatingEntity entity)
        {
            if(_onVitalityChange is null) return;

            IVitalityStatsData<float> onStats = entity.CombatStats;
            foreach (IVitalityChangeListener listener in _onVitalityChange)
            {
                listener.OnVitalityChange(onStats);
            }
        }

        public void InvokeTemporalStatChange(CombatingEntity entity)
        {
            if(_onCombatHealthChange is null) return;

            ICombatHealthStatsData<float> onStats = entity.CombatStats;
            foreach (ICombatHealthChangeListener listener in _onCombatHealthChange)
            {
                listener.OnTemporalStatsChange(onStats);
            }
        }
        public void InvokeAreaChange(CombatingEntity entity)
        {
            if(_onAreaChange is null) return;

            CharacterCombatAreasData areasData = entity.AreasDataTracker;
            foreach (IAreaStateChangeListener listener in _onAreaChange)
            {
                listener.OnAreaStateChange(areasData);
            }
        }

        public void OnInitiativeTrigger()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnInitiativeTrigger();
            }
        }

        public void OnDoMoreActions()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnDoMoreActions();
            }
        }

        public void OnFinisAllActions()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnFinisAllActions();
            }
        }

        public void OnHealthZero(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnHealthZero(entity);
            }
        }

        public void OnMortalityZero(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnMortalityZero(entity);
            }
        }

        public void OnRevive(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnRevive(entity);
            }
        }

        public void OnTeamHealthZero(CombatingTeam losingTeam)
        {
            if (_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnTeamHealthZero(losingTeam);
            }
        }
    }

    public class CombatCharacterEvents : CombatCharacterEventsBase, IHitEventHandler
    {
        private readonly CombatingEntity _user;
        public readonly OnHitEventHandler OnHitEvent;
        public CombatCharacterEvents(CombatingEntity user) : base()
        {
            _user = user;
            OnHitEvent = new OnHitEventHandler(user);
            base.Subscribe(OnHitEvent);
        }

        private static CombatCharacterEventsBase GlobalEvents()
        {
            return CombatSystemSingleton.GlobalCharacterChangesEvent;
        }

        public void InvokeVitalityChange()
        {
            // This is to loop Global listeners (like unique UI than is not exclusive to each Entity)
            GlobalEvents().InvokeVitalityChange(_user);
            // This is to loop the listeners in the base (the UI over the character for example)
            InvokeVitalityChange(_user); 
        }

        public void InvokeTemporalStatChange()
        {
            //TODO _user.HarmonyBuffInvoker?.OnTemporalStatsChange();
            GlobalEvents().InvokeTemporalStatChange(_user);
            InvokeTemporalStatChange(_user);
        }

        public void InvokeAreaChange()
        {
            GlobalEvents().InvokeAreaChange(_user);
            InvokeAreaChange(_user);
        }

        public void Subscribe(ICombatHitListener listener)
            => OnHitEvent.Subscribe(listener);


        public void UnSubscribe(ICombatHitListener listener)
            => OnHitEvent.UnSubscribe(listener);
       
    }

    /// <summary>
    /// Tracks all events than need to ve invoked and rises them all in one go.
    /// This avoid clusters of events
    /// </summary>
    public class CharacterEventsTracker
    {
        private readonly Queue<CombatingEntity> _temporalStatsEvents;
        [ShowInInspector]
        private readonly EntityValuesQueue _onDamageEvents;
        private readonly Queue<CombatingEntity> _healthZeroEvents;
        private readonly Queue<CombatingEntity> _mortalityZeroEvents;
        private readonly Queue<CombatingTeam> _groupZeroHealthEvents;

        public CharacterEventsTracker()
        {
            const int amountOfCharacterPerTeam = GlobalCombatParams.PredictedAmountOfTeamCharacters;
            const int amountOfTeams = GlobalCombatParams.PredictedAmountOfTeams;
            _temporalStatsEvents = new Queue<CombatingEntity>(amountOfCharacterPerTeam);
            _healthZeroEvents = new Queue<CombatingEntity>(amountOfCharacterPerTeam);
            _onDamageEvents = new EntityValuesQueue(amountOfCharacterPerTeam);

            _mortalityZeroEvents = new Queue<CombatingEntity>(); // MortalityZero happens less frequently
            _groupZeroHealthEvents = new Queue<CombatingTeam>(amountOfTeams);
        }

        public void EnqueueTemporalChangeListener(CombatingEntity entity)
            => EnqueueListener(entity, _temporalStatsEvents);

        public void EnqueueOnDamageListener(CombatingEntity entity, float damage)
            => EnqueueListener(entity, _onDamageEvents, damage);

        public void EnqueueZeroHealthListener(CombatingEntity entity)
        {
            EnqueueListener(entity,_healthZeroEvents);
            EnqueueTeamParticipants(entity.CharacterGroup.Team);
        }

        public void EnqueueZeroMortalityListener(CombatingEntity entity)
            => EnqueueListener(entity, _mortalityZeroEvents);

        private static void EnqueueListener(CombatingEntity entity, Queue<CombatingEntity> onQueue)
        {
            if(onQueue.Contains(entity)) return;
            onQueue.Enqueue(entity);
        }

        private static void EnqueueListener(CombatingEntity entity, EntityValuesQueue onQueue, float value)
        {
            onQueue.EnQueue(entity,value);
        }

        private void EnqueueTeamParticipants(CombatingTeam team)
        {
            if(_groupZeroHealthEvents.Contains(team)) return;

            _groupZeroHealthEvents.Enqueue(team);
        }

        // This avoids GC alloc
        private Action<CombatingEntity> _queueAction;
        public void Invoke()
        {
            _queueAction = InvokeTemporalStatsAction;
            InvokeQueue(_temporalStatsEvents);

            _queueAction = InvokeZeroHealthAction;
            InvokeQueue(_healthZeroEvents);

            _queueAction = InvokeMortalityZeroAction;
            InvokeQueue(_mortalityZeroEvents);

            InvokeDamageQueue();

            InvokeTeamQueue(_groupZeroHealthEvents);

            _queueAction = null;

            ////////////////////
            void InvokeQueue(Queue<CombatingEntity> queue)
            {
                while (queue.Count > 0)
                {
                    var entity = queue.Dequeue();
                    _queueAction(entity);
                }
            }

            // this can be transformed into a generic Invoke
            void InvokeDamageQueue()
            {
                var queue = _onDamageEvents;
                while (queue.Count > 0)
                {
                    var element = queue.DeQueue();
                    var entity = element.Entity;
                    var damage = element.Value;
                    InvokeOnDamageAction(entity,damage);
                }
            }

            void InvokeTeamQueue(Queue<CombatingTeam> teamQueue)
            {
                while (teamQueue.Count > 0)
                {
                    var team = teamQueue.Dequeue();
                    // TODO
                }
            }
        }



        private static void InvokeTemporalStatsAction(CombatingEntity entity)
        {
            entity.Events.InvokeTemporalStatChange();
            CombatSystemSingleton.GlobalCharacterChangesEvent.InvokeTemporalStatChange(entity);
        }

        private static void InvokeOnDamageAction(CombatingEntity entity, float totalDamage)
        {
            entity.Events.OnHitEvent.OnDamage(totalDamage);
        }

        private static void InvokeZeroHealthAction(CombatingEntity entity)
        {
            entity.Events.OnHealthZero(entity);
            CombatSystemSingleton.GlobalCharacterChangesEvent.OnHealthZero(entity);
        }

        private static void InvokeMortalityZeroAction(CombatingEntity entity)
        {
            entity.Events.OnMortalityZero(entity);
            CombatSystemSingleton.GlobalCharacterChangesEvent.OnMortalityZero(entity);
        }

        private class EntityValuesQueue 
        {
            [ShowInInspector]
            private readonly List<QueueValues> _elements;

            public EntityValuesQueue()
            {
                _elements = new List<QueueValues>();
            }

            public EntityValuesQueue(int collectionSize) 
            {
                _elements = new List<QueueValues>(collectionSize);
            }

            public int Count => _elements.Count;

            private int IndexOf(CombatingEntity entity)
            {
                int indexOf = -1;
                for (int i = 0; i < _elements.Count; i++)
                {
                    var elementEntity = _elements[i].Entity;
                    if (elementEntity == entity)
                        return i;
                }
                return indexOf;
            }
            public void EnQueue(CombatingEntity entity, float value)
            {
                int index = IndexOf(entity);

                if (index < 0)
                {
                    QueueValues values = new QueueValues(entity,value);
                    _elements.Add(values);
                    return;
                }

                _elements[index].Increment(value);
            }

            public QueueValues DeQueue()
            {
                int index = _elements.Count - 1;
                QueueValues element = _elements[index];
                _elements.RemoveAt(index);

                return element;
            }
        }

        private struct QueueValues
        {
            public readonly CombatingEntity Entity;
            public float Value;

            public QueueValues(CombatingEntity entity, float value)
            {
                Entity = entity;
                Value = value;
            }

            public void Increment(float addition)
            {
                Value += addition;
            }
        }
    }

    /// <summary>
    /// A listener of stats changes (instead of every frame)
    /// </summary>
    public interface ICharacterEventListener { }

    public interface IVitalityChangeListener : ICharacterEventListener
    {
        void OnVitalityChange(IVitalityStatsData<float> currentStats);
    }

    public interface ITemporalStatChangeListener : ICharacterEventListener
    {
        void OnConcentrationChange(ITemporalStatsData<float> currentStats);
    }
    public interface ICombatHealthChangeListener : ICharacterEventListener
    {
        void OnTemporalStatsChange(ICombatHealthStatsData<float> currentStats);
    }
    public interface IAreaStateChangeListener : ICharacterEventListener
    {
        void OnAreaStateChange(CharacterCombatAreasData data);
    }


    public interface IHealthZeroListener : ICharacterEventListener
    {
        void OnHealthZero(CombatingEntity entity);
        void OnMortalityZero(CombatingEntity entity);
        void OnRevive(CombatingEntity entity);
        void OnTeamHealthZero(CombatingTeam losingTeam);
    }

    public interface IBuffDoneListener : ICharacterEventListener
    {
        void OnBuffDone(CombatingEntity entity);
    }

    public interface ICriticalActionListener : ICharacterEventListener
    {
        void OnCriticalAction(CombatingEntity entity); 
    }
}
