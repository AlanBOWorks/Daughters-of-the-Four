using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Common;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class TempoTicker : ICombatStartListener, ICombatTerminationListener
    {
        [ShowInInspector] 
        internal readonly HashSet<ITempoTickListener> TickListeners;
        [ShowInInspector] 
        internal readonly CombatEntitiesTempoTicker EntitiesTempoTicker;


        private readonly IPlayerPauseValues _playerPauseValues;

        public TempoTicker()
        {
            EntitiesTempoTicker = new CombatEntitiesTempoTicker();
            TickListeners = new HashSet<ITempoTickListener>();

            _playerPauseValues = PlayerCommonControlValuesSingleton.GetPauseValues();
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
        public CoroutineHandle GetHandle() => _tickingHandle;
        public bool IsTicking() => _tickingHandle.IsRunning;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
            Timing.KillCoroutines(_tickingHandle); //safe kill

            PauseTicking = false;

            _tickingHandle = CombatCoroutinesTracker.StartCombatCoroutineAsMain(_TickingLoop());
        }


        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            Timing.KillCoroutines(_tickingHandle);
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
        }


        public const float TickPeriodSeconds = .2f;
        public const int LoopThreshold = 23;
        /// <summary>
        /// Instead being 23; it becomes 23+1 for more intuitive and persistent behaviors
        /// </summary>
        public const int LoopThresholdAsIntended = LoopThreshold + 1;

        private int _roundTickCount;
        public int GetCurrentRoundTicks() => _roundTickCount;


        [Button, HideIf("PauseTicking")]
        private void PauseNextTick() => PauseTicking = true;
        [Button, ShowIf("PauseTicking")]
        private void ResumeNextTick() => PauseTicking = false;

        public bool PauseTicking = false; 

        private IEnumerator<float> _TickingLoop()
        {
            var teamControllersHandler = CombatSystemSingleton.TeamControllers;
            var skillQueuePerformer = CombatSystemSingleton.SkillQueuePerformer;
            var currentFinishHandler = CombatSystemSingleton.CombatFinishHandler;
            var combatEvents = CombatSystemSingleton.EventsHolder;

            foreach (var listener in TickListeners)
            {
                listener.OnStartTicking();
            }
            while (!currentFinishHandler.IsCombatFinish())
            {
                yield return Timing.WaitForSeconds(TickPeriodSeconds);
                _roundTickCount++;

                while (_playerPauseValues.IsGamePaused || PauseTicking)
                {
                    yield return Timing.WaitForOneFrame;
                }



                EntitiesTempoTicker.TickEntities();
                yield return Timing.WaitForOneFrame;

                foreach (var listener in TickListeners)
                {
                    listener.OnTick();
                }

                yield return Timing.WaitForOneFrame;

                
                bool controllersWaiting = teamControllersHandler.HasTeamWaiting();
                if (controllersWaiting)
                {
                    var controllersEnumerator = teamControllersHandler.GetActiveControllers();

                    // ----- Wait For Animations
                    do
                    {
                        yield return Timing.WaitForOneFrame;
                    } while (skillQueuePerformer.IsActing());
                    

                    // ------ CONTROLLERS
                    foreach (var controller in controllersEnumerator)
                    {
                        // ----- Wait For VanguardEffects
                        yield return Timing.WaitForOneFrame;
                        var controllerTeam = controller.ControllingTeam;
                        combatEvents.OnTempoPreStartControl(controller, GetFirstActor());
                        CombatEntity GetFirstActor()
                        {
                            var actingMembers = controllerTeam.GetControllingMembers();
                            return actingMembers[0];
                        }
                        yield return Timing.WaitForOneFrame;



                        teamControllersHandler.TryInvokeControl(in controller);
                        do
                        {
                            yield return Timing.WaitForOneFrame;
                        } while (teamControllersHandler.IsControlling());
                        yield return Timing.WaitForOneFrame;

                    }

                    // ----- Wait For Animations
                    do
                    {
                        yield return Timing.WaitForOneFrame;
                    } while (skillQueuePerformer.IsActing());
                }


                if (_roundTickCount < LoopThreshold) continue;
                {
                    foreach (var listener in TickListeners)
                    {
                        listener.OnRoundPassed();
                    }

                    _roundTickCount = 0;
                }

                yield return Timing.WaitForOneFrame;
            }



            bool isPlayerWin = currentFinishHandler.CheckIfPlayerWon();
            var finishType = UtilsCombatFinish.GetEnumByBoolean(isPlayerWin);
            combatEvents.OnCombatFinish(finishType);
        }

    }

    public readonly struct TempoTickValues
    {
        public static TempoTickValues ZeroValues = new TempoTickValues(null,0,0,0);

        public readonly CombatEntity Entity; 
        public readonly float CurrentTick;
        public readonly float CurrentPercent;
        public readonly int RemainingSteps;

        public TempoTickValues(CombatEntity entity, float currentTick, float currentPercent, int remainingSteps)
        {
            Entity = entity;
            CurrentTick = currentTick;
            CurrentPercent = currentPercent;
            RemainingSteps = remainingSteps;
        }

        public TempoTickValues(CombatEntity entity, float entityInitiativeAmount)
        {
            var stats = entity.Stats;

            Entity = entity;
            CurrentTick = entityInitiativeAmount;
            CurrentPercent = UtilsCombatStats.CalculateTempoPercent(entityInitiativeAmount);
            RemainingSteps = UtilsCombatStats.CalculateRemainingInitiativeSteps(stats);
        }
        public TempoTickValues(CombatEntity entity) : this(entity, entity.Stats.TotalInitiative)
        { }

        public void ExtractValues(out float currentTick, out float currentPercent)
        {
            currentTick = CurrentTick;
            currentPercent = CurrentPercent;
        }

        public void ExtractValues(out float currentTick, out float currentPercent, out int remainingSteps)
        {
            ExtractValues(out currentTick, out currentPercent);
            remainingSteps = RemainingSteps;
        }

    }
    public interface ITempoTickListener : ICombatEventListener
    {
        void OnStartTicking();
        void OnTick();
        void OnRoundPassed();
        void OnStopTicking();
    }


    public interface ITempoEntityMainStatesListener : ICombatEventListener
    {
        /// <summary>
        /// It's send once per sequence and the first time the entity's
        /// [<seealso cref="CombatStats.CurrentInitiative"/>] triggers. 
        /// </summary>
        void OnEntityRequestSequence(CombatEntity entity, bool canControl);

        /// <summary>
        /// The very last event invoked when there's no [<seealso cref="CombatStats.UsedActions"/>] left or when
        /// the [<see cref="CombatEntity"/>] passes its actions somehow. <br></br><br></br>
        /// Note:<br></br>
        /// In this events, the entity is not removed from [<seealso cref="CombatTeam._controlMembers"/>].<br></br>
        /// For that subscribe to [<seealso cref="ITempoEntityStatesExtraListener.OnAfterEntitySequenceFinish"/>]
        /// </summary>
        void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController);
    }

    public interface ITempoEntityActionStatesListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked per [<seealso cref="CombatStats.UsedActions"/>] left and if it can act.<br></br>
        /// If there's no actions left [<see cref="ITempoEntityMainStatesListener.OnEntityFinishSequence"/>] will be invoked instead.
        /// </summary>
        void OnEntityRequestAction(CombatEntity entity);

        void OnEntityBeforeSkill(CombatEntity entity);

        /// <summary>
        /// Invoked after the action is finish; It's the very last call of the action (after animations) and
        /// it's called before [<see cref="OnEntityRequestAction"/>]
        /// and [<see cref="ITempoEntityMainStatesListener.OnEntityFinishSequence"/>].
        /// </summary>
        void OnEntityFinishAction(CombatEntity entity);

        /// <summary>
        /// Invoked when the (<paramref name="entity"/>) has not actions left.<br></br>
        /// It's invoked before [<seealso cref="ITempoEntityMainStatesListener.OnEntityFinishSequence"/>] and SkillPerform;<br></br>
        /// Resets and buff's behaviors are not performed yet
        /// </summary>
        void OnEntityEmptyActions(CombatEntity entity);
    }


    public interface ITempoEntityStatesExtraListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked after [<seealso cref="ITempoEntityStatesListener.OnEntityRequestSequence"/>] and
        /// only it can act;
        /// </summary>
        void OnAfterEntityRequestSequence(CombatEntity entity);
        /// <summary>
        /// Invoked after [<seealso cref="ITempoEntityStatesListener.OnEntityFinishSequence"/>]; <br></br>
        /// This events is called after removing the entity from the [<seealso cref="CombatTeam._controlMembers"/>].<br></br>
        /// </summary>
        void OnAfterEntitySequenceFinish(CombatEntity entity);

        /// <summary>
        /// Invoked after the entity reaches initiation but doesn't have enough actions
        /// </summary>
        void OnNoActionsForcedFinish(CombatEntity entity);
    }

    public interface ITempoDedicatedEntityStatesListener : ICombatEventListener
    {
        /// <summary>
        /// <inheritdoc cref="OnOffEntityRequestSequence"/> and
        /// [<seealso cref="OnOffEntityRequestSequence"/>]
        /// </summary>
        void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct);
        /// <summary>
        /// <inheritdoc cref="ITempoEntityStatesListener.OnEntityRequestSequence"/><br></br><br></br>
        /// This is invoked after [<seealso cref="ITempoEntityStatesListener.OnEntityRequestSequence"/>
        /// </summary>
        void OnOffEntityRequestSequence(CombatEntity entity, bool canAct);


        void OnTrinityEntityFinishSequence(CombatEntity entity);
        void OnOffEntityFinishSequence(CombatEntity entity);

    }

    public interface ITempoControlStatesListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked when the controller starts; It's after [<seealso cref="ITempoControlStatesExtraListener.OnTempoPreStartControl"/>]
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="firstControl"></param>
        void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl);
        /// <summary>
        /// Invoked when all actors had finish acting(no action left)
        /// </summary>
        /// <param name="lastActor"></param>
        void OnAllActorsNoActions(CombatEntity lastActor);

        /// <summary>
        /// Invoked when the Entity's Controller decides that there's no more actions to make
        /// </summary>
        void OnTempoFinishControl(CombatTeamControllerBase controller);
    }

    public interface ITempoControlStatesExtraListener : ICombatEventListener
    {
        /// <summary>
        /// First call; It's before [<seealso cref="ITempoEntityStatesListener.OnEntityRequestSequence"/>]
        /// and [<seealso cref="ITempoControlStatesListener.OnTempoStartControl"/>]
        /// </summary>
        void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity);


        void LateOnAllActorsNoActions(CombatEntity lastActor);

        /// <summary>
        /// The very last call of this events; when everything was removed and invoked
        /// </summary>
        void OnTempoFinishLastCall(CombatTeamControllerBase controller);
    }


    public interface ITempoEntityPercentListener : ICombatEventListener
    {
        void OnEntityTick(in TempoTickValues tempoValues);
    }

    public static class UtilsTempo
    {
        public static bool IsInitiativeTrigger(CombatEntity entity)
        {
            var entityInitiativeAmount = entity.Stats.CurrentInitiative;
            const float initiativeThreshold = TempoTicker.LoopThreshold;
            return entityInitiativeAmount >= initiativeThreshold;
        }
    }
}
