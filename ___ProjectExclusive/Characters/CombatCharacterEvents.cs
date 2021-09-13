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
    /// [<seealso cref="TempoTicker"/>] since this Handler only triggers the User (and not targets)
    /// and that's not the desired behaviour.
    /// <br></br>
    /// <br></br> _____ TL;DR _____ <br></br>
    /// [<see cref="ICharacterEventListener"/>]:
    /// can be invoked by anyone and anywhere (generally an User will invoke 
    /// a specific target's listeners)<br></br>
    /// [<seealso cref="ITempoListener"/>]:
    /// is deterministic and will only be invoked in one specific Entity that was triggered.
    /// </summary>
    public class CombatCharacterEventsBase 
    {
        [ShowInInspector] public readonly List<ITempoListener> onTempoListeners;
        [ShowInInspector] public readonly List<IVitalityChangeListener> onVitalityChange;
        [ShowInInspector] public readonly List<ITemporalStatChangeListener> onTemporalStatChange;
        [ShowInInspector] public readonly List<ICombatHealthChangeListener> onCombatHealthChange;
        [ShowInInspector] public readonly List<IAreaStateChangeListener> onAreaChange;

        [ShowInInspector] public readonly List<IHealthZeroListener> onHealthZeroListeners;

        public CombatCharacterEventsBase()
        {
            onTempoListeners = new List<ITempoListener>();
            
            onVitalityChange = new List<IVitalityChangeListener>();
            onTemporalStatChange = new List<ITemporalStatChangeListener>();
            onCombatHealthChange = new List<ICombatHealthChangeListener>();
            onAreaChange = new List<IAreaStateChangeListener>();
            
            onHealthZeroListeners = new List<IHealthZeroListener>();
        }

        public void Subscribe(ICharacterEventListener listener)
        {
            if (listener is ITempoListener tempoListener)
                onTempoListeners.Add(tempoListener);

            if (listener is IVitalityChangeListener vitalityListener)
                onVitalityChange.Add(vitalityListener);
            if (listener is ITemporalStatChangeListener temporalStatListener)
                onTemporalStatChange.Add(temporalStatListener);
            if (listener is ICombatHealthChangeListener healthChangeListener)
                onCombatHealthChange.Add(healthChangeListener);
            if (listener is IAreaStateChangeListener areaStateListener)
                onAreaChange.Add(areaStateListener);

            if (listener is IHealthZeroListener healthCheckListener)
                onHealthZeroListeners.Add(healthCheckListener);
        }



    }

    public class CombatCharacterEvents : CombatCharacterEventsBase, IHitEventHandler
    {
        public readonly OnHitEventHandler OnHitEvent;
        public CombatCharacterEvents(CombatingEntity user) : base()
        {
            OnHitEvent = new OnHitEventHandler(user);
            base.Subscribe(OnHitEvent);
        }

        private static CombatCharacterEventsBase GlobalEvents()
        {
            return CombatSystemSingleton.GlobalCharacterChangesEvent;
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

        private readonly Queue<CombatingEntity> _healthChangeEvents;
        private readonly Queue<CombatingEntity> _healthZeroEvents;
        private readonly Queue<CombatingEntity> _mortalityZeroEvents;
        private readonly Queue<CombatingTeam> _groupZeroHealthEvents;

        public CharacterEventsTracker()
        {
            const int amountOfCharacterPerTeam = GlobalCombatParams.PredictedAmountOfTeamCharacters;
            const int amountOfTeams = GlobalCombatParams.PredictedAmountOfTeams;

            _temporalStatsEvents = new Queue<CombatingEntity>(amountOfCharacterPerTeam);
            _healthZeroEvents = new Queue<CombatingEntity>(amountOfCharacterPerTeam);
            _healthChangeEvents = new Queue<CombatingEntity>(amountOfCharacterPerTeam);
            _onDamageEvents = new EntityValuesQueue(amountOfCharacterPerTeam);

            _mortalityZeroEvents = new Queue<CombatingEntity>(); // MortalityZero happens less frequently
            _groupZeroHealthEvents = new Queue<CombatingTeam>(amountOfTeams);
        }

        public void EnqueueTemporalChangeListener(CombatingEntity entity)
            => EnqueueListener(entity, _temporalStatsEvents);

        public void EnqueueOnDamageListener(CombatingEntity entity, float damage)
        => EnqueueListener(entity, _onDamageEvents, damage);
         
        public void EnqueueOnHealthChangeListener(CombatingEntity entity)
            => EnqueueListener(entity, _healthChangeEvents);

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

            InvokeDamageQueue();
            _queueAction = InvokeOnHealthAction;
            InvokeQueue(_healthChangeEvents);

            _queueAction = InvokeZeroHealthAction;
            InvokeQueue(_healthZeroEvents);

            _queueAction = InvokeMortalityZeroAction;
            InvokeQueue(_mortalityZeroEvents);


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
            => CombatSystemSingleton.CombatEventsInvoker.InvokeTemporalStatChange(entity);

        private static void InvokeOnDamageAction(CombatingEntity entity, float totalDamage)
        {
            entity.Events.OnHitEvent.OnDamage(totalDamage);
            InvokeOnHealthAction(entity);
        }

        private static void InvokeOnHealthAction(CombatingEntity entity)
            => CombatSystemSingleton.CombatEventsInvoker.InvokeOnHealthChange(entity);

        private static void InvokeZeroHealthAction(CombatingEntity entity)
            => CombatSystemSingleton.CombatEventsInvoker.InvokeOnHealthZero(entity);
        

        private static void InvokeMortalityZeroAction(CombatingEntity entity)
            => CombatSystemSingleton.CombatEventsInvoker.InvokeOnMortalityZero(entity);
        

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
