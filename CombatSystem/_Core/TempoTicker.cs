using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class TempoTicker : ICombatStatesListener, ICombatPreparationListener, ITempoEntityStatesListener
    {
        [ShowInInspector] 
        internal readonly HashSet<ITempoTickListener> TickListeners;
        [ShowInInspector] internal readonly CombatEntitiesTempoTicker EntitiesTempoTicker;

        public TempoTicker()
        {
            EntitiesTempoTicker = new CombatEntitiesTempoTicker();
            TickListeners = new HashSet<ITempoTickListener>();
        }

        public void Subscribe(ITempoTickListener tickListener)
        {
            if (TickListeners.Contains(tickListener))
            {
                throw new ArgumentOutOfRangeException(nameof(tickListener),$"A copy of [{nameof(ITempoTickListener)}] was" +
                                                                     $"added to the Ticker; Only one copy of each can" +
                                                                     $"be added to the System");
            }

            TickListeners.Add(tickListener);
        }

        public void UnSubscribe(ITempoTickListener listener)
            => TickListeners.Remove(listener);


        private CoroutineHandle _tickingHandle;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
            _tickingHandle = Timing.RunCoroutine(_TickingLoop(), Segment.RealtimeUpdate);
            CombatSystemSingleton.LinkCoroutineToMaster(in _tickingHandle);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            EntitiesTempoTicker.ResetState();
        }

        public void OnCombatQuit()
        {
            EntitiesTempoTicker.ResetState();
        }


        public const float TickPeriodSeconds = .2f;
        public const int LoopThreshold = 23;
        /// <summary>
        /// Instead being 23; it becomes 23+1 for more intuitive and persistent behaviors
        /// </summary>
        public const int LoopThresholdAsIntended = LoopThreshold + 1;

        private int _roundTickCount;
        public int GetCurrentRoundTicks() => _roundTickCount;
        private IEnumerator<float> _TickingLoop()
        {
            var controllers = CombatSystemSingleton.TeamControllers;
            foreach (var listener in TickListeners)
            {
                listener.OnStartTicking();
            }
            while (true)
            {
                yield return Timing.WaitForSeconds(TickPeriodSeconds);
                _roundTickCount++;
                foreach (var listener in TickListeners)
                {
                    listener.OnTick();
                }

                do
                {
                    yield return Timing.WaitForOneFrame;
                } while (controllers.CurrentControllerIsActive());

                //Safe wait
                yield return Timing.WaitForOneFrame;



                if (_roundTickCount < LoopThreshold) continue;
                {
                    foreach (var listener in TickListeners)
                    {
                        listener.OnRoundPassed();
                    }

                    _roundTickCount = 0;
                }
            }
        }


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            EntitiesTempoTicker.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
        }

        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
        }

    }

    public sealed class CombatEntitiesTempoTicker : 
        ICombatPreparationListener,
        ITempoTickListener,
        ITempoEntityStatesListener
    {
        public CombatEntitiesTempoTicker()
        {
            _tickingTrackers = new HashSet<CombatEntity>();
        }

        [ShowInInspector]
        private readonly HashSet<CombatEntity> _tickingTrackers;




        public void ResetState()
        {
            _tickingTrackers.Clear();
        }

        public void AddEntities(CombatTeam team)
        {
            foreach (var member in team)
            {
                AddEntity(member);
            }
        }

        private void AddEntity(CombatEntity entity)
        {
            _tickingTrackers.Add(entity);
        }




        public void OnStartTicking()
        { }
        public void OnStopTicking()
        { }

        public void OnTick()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var entity in _tickingTrackers)
            {
                HandleTickEntity(entity);
            }


            void HandleTickEntity(CombatEntity entity)
            {
                CombatStats stats = entity.Stats;

                UtilsCombatStats.TickInitiative(stats, out var entityInitiativeAmount);
                const float initiativeThreshold = TempoTicker.LoopThreshold;
                float initiativePercent = entityInitiativeAmount / initiativeThreshold;

                eventsHolder.OnEntityTick(in entity, in entityInitiativeAmount, in initiativePercent);
                //Acting Check
                if (entityInitiativeAmount < initiativeThreshold)
                    return;

                bool isTrinityRole = UtilsTeam.IsTrinityRole(in entity);
                if (isTrinityRole)
                    HandleMainRole();
                else
                    entity.Team.StandByMembers.PutOnStandBy(in entity);


                void HandleMainRole()
                {
                    bool canAct = UtilsCombatStats.CanRequestActing(entity);
                    eventsHolder.OnMainEntityRequestSequence(entity, canAct);
                }
            }

        }


        public void OnRoundPassed()
        {
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            AddEntities(playerTeam);
            AddEntities(enemyTeam);
        }

        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            var stats = entity.Stats;
            stats.UsedActions = 0;
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {

        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
                bool canAct = UtilsCombatStats.CanActRequest(entity);
                if(!canAct)
                    CombatSystemSingleton.EventsHolder.OnEntityFinishSequence(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            entity.Stats.CurrentInitiative = 0;
        }
    }

    public interface ITempoTickListener : ICombatEventListener
    {
        void OnStartTicking();
        void OnTick();
        void OnRoundPassed();
        void OnStopTicking();
    }

    public interface ITempoEntityStatesListener : ICombatEventListener
    {
        /// <summary>
        /// It's send once per sequence and the first time the entity's
        /// [<seealso cref="CombatStats.CurrentInitiative"/>] triggers. This is invoked even if the
        /// entity can't act; To verify if can act use [<see cref="canAct"/>] or use
        /// [<seealso cref="OnEntityRequestAction"/>] instead
        /// </summary>
        void OnMainEntityRequestSequence(CombatEntity entity, bool canAct);
        /// <summary>
        /// Invoked per [<seealso cref="CombatStats.UsedActions"/>] left.<br></br>
        /// If there's no actions left [<see cref="OnEntityFinishSequence"/>] will be invoked instead.
        /// </summary>
        void OnEntityRequestAction(CombatEntity entity);

        /// <summary>
        /// Invoked after the action is finish; It's the very last call of the action (after animations) and
        /// it's called before [<see cref="OnEntityRequestAction"/>] and [<see cref="OnEntityFinishSequence"/>].
        /// </summary>
        void OnEntityFinishAction(CombatEntity entity);
        /// <summary>
        /// The very last event invoked when there's no [<seealso cref="CombatStats.UsedActions"/>] left or when
        /// the [<see cref="CombatEntity"/>] passes its actions somehow. <br></br>
        /// </summary>
        void OnEntityFinishSequence(CombatEntity entity);

        
    }

    public interface ITempoTeamStatesListener : ICombatEventListener
    {
        void OnTempoStartControl(in CombatTeamControllerBase controller);
        /// <summary>
        /// Invoked when the Entity's Controller decides that there's no more actions to make
        /// </summary>
        void OnTempoFinishControl(in CombatTeamControllerBase controller);
    }

    public interface ITempoEntityPercentListener : ICombatEventListener
    {
        void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative);
    }
}
