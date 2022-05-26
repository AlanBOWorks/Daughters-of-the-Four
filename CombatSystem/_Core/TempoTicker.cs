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
    public sealed class TempoTicker : ICombatStatesListener
    {
        [ShowInInspector] 
        internal readonly HashSet<ITempoTickListener> TickListeners;
        [ShowInInspector] internal readonly CombatEntitiesTempoTicker EntitiesTempoTicker;

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
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
            Timing.KillCoroutines(_tickingHandle); //safe kill

            _tickingHandle = Timing.RunCoroutine(_TickingLoop(), Segment.RealtimeUpdate);
            CombatSystemSingleton.LinkCoroutineToMaster(in _tickingHandle);
        }

        public void OnCombatEnd()
        {
            Timing.KillCoroutines(_tickingHandle);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
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

#if UNITY_EDITOR
        [Button, HideIf("_pauseTicking")]
        private void PauseNextTick() => _pauseTicking = true;
        [Button, ShowIf("_pauseTicking")]
        private void ResumeNextTick() => _pauseTicking = false;

        private bool _pauseTicking = false; 
#endif
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

#if UNITY_EDITOR
                while (_playerPauseValues.IsGamePaused || _pauseTicking)
#else
                while (_playerPauseValues.IsGamePaused)
#endif
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


                // ------ CONTROLLERS
                controllers.TickPlayerController();
                do
                {
                    yield return Timing.WaitForOneFrame;
                } while (controllers.IsControlling());

                yield return Timing.WaitForOneFrame;

                controllers.TickEnemyController();
                do
                {
                    yield return Timing.WaitForOneFrame;
                } while (controllers.IsControlling());
                // ------ CONTROLLERS





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
        /// [<seealso cref="CombatStats.CurrentInitiative"/>] triggers. 
        /// </summary>
        void OnEntityRequestSequence(CombatEntity entity, bool canControl);

        /// <summary>
        /// Invoked per [<seealso cref="CombatStats.UsedActions"/>] left and if it can act.<br></br>
        /// If there's no actions left [<see cref="OnEntityFinishSequence"/>] will be invoked instead.
        /// </summary>
        void OnEntityRequestAction(CombatEntity entity);

        void OnEntityBeforeSkill(CombatEntity entity);

        /// <summary>
        /// Invoked after the action is finish; It's the very last call of the action (after animations) and
        /// it's called before [<see cref="OnEntityRequestAction"/>] and [<see cref="OnEntityFinishSequence"/>].
        /// </summary>
        void OnEntityFinishAction(CombatEntity entity);

        /// <summary>
        /// Invoked when the (<paramref name="entity"/>) has not actions left.<br></br>
        /// It's invoked before [<seealso cref="OnEntityFinishSequence"/>] and SkillPerform;<br></br>
        /// Resets and buff's behaviors are not performed yet
        /// </summary>
        void OnEntityEmptyActions(CombatEntity entity);

        /// <summary>
        /// The very last event invoked when there's no [<seealso cref="CombatStats.UsedActions"/>] left or when
        /// the [<see cref="CombatEntity"/>] passes its actions somehow. <br></br><br></br>
        /// Note:<br></br>
        /// In this events, the entity is not removed from [<seealso cref="CombatTeam._controlMembers"/>].<br></br>
        /// For that subscribe to [<seealso cref="ITempoEntityStatesExtraListener.OnAfterEntitySequenceFinish"/>]
        /// </summary>
        void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController);
    }

    public interface ITempoEntityStatesExtraListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked after [<seealso cref="ITempoEntityStatesListener.OnEntityRequestSequence"/>] and
        /// only it can act;
        /// </summary>
        void OnAfterEntityRequestSequence(in CombatEntity entity);
        /// <summary>
        /// Invoked after [<seealso cref="ITempoEntityStatesListener.OnEntityFinishSequence"/>]; <br></br>
        /// This events is called after removing the entity from the [<seealso cref="CombatTeam._controlMembers"/>].<br></br>
        /// </summary>
        void OnAfterEntitySequenceFinish(in CombatEntity entity);

        /// <summary>
        /// Invoked after the entity reaches initiation but doesn't have enough actions
        /// </summary>
        void OnNoActionsForcedFinish(in CombatEntity entity);
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

    public interface ITempoTeamStatesListener : ICombatEventListener
    {
        /// <summary>
        /// First call; It's before [<seealso cref="ITempoEntityStatesListener.OnEntityRequestSequence"/>]
        /// </summary>
        void OnTempoPreStartControl(in CombatTeamControllerBase controller);

        /// <summary>
        /// Similar to [<seealso cref="OnControlFinishAllActors"/>] but before the skills performing
        /// </summary>
        /// <param name="lastActor"></param>
        void OnAllActorsNoActions(in CombatEntity lastActor);

        /// <summary>
        /// Event send after all members had finished; this is invoked before [<seealso cref="OnTempoFinishControl"/>]
        /// </summary>
        void OnControlFinishAllActors(in CombatEntity lastActor);
        /// <summary>
        /// Invoked when the Entity's Controller decides that there's no more actions to make
        /// </summary>
        void OnTempoFinishControl(in CombatTeamControllerBase controller);

        /// <summary>
        /// The very last call of this events; when everything was removed and invoked
        /// </summary>
        void OnTempoFinishLastCall(in CombatTeamControllerBase controller);
    }

    public interface ITempoEntityPercentListener : ICombatEventListener
    {
        void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative);
    }

    public static class UtilsTempo
    {
        public static bool IsInitiativeTrigger(in CombatEntity entity)
        {
            var entityInitiativeAmount = entity.Stats.CurrentInitiative;
            const float initiativeThreshold = TempoTicker.LoopThreshold;
            return entityInitiativeAmount >= initiativeThreshold;
        }
    }
}
