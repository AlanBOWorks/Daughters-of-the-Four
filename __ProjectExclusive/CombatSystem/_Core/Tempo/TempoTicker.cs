using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem.Events;
using CombatTeam;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSystem
{
    public class TempoTicker : ICombatDisruptionListener
    {
#if UNITY_EDITOR
        [ShowInInspector, TextArea] private const string BehaviourExplanation =
            "TempoTicker increase in ticksSteps the [CombatingEntities] initiative.\n" +
            "Once an entity reachs 100% triggers save it in a Queue and then" +
            "invokes the initiative trigger events for each one.\n" +
            "(It just simulates time progression and turn order)";

        private bool RemoveWarningFromUnityConsole(string nonUsedStringInReal)
            => nonUsedStringInReal == BehaviourExplanation;
#endif

        public TempoTicker()
        {
            TickListeners = new HashSet<ITempoTickListener>();
        }

        public readonly HashSet<ITempoTickListener> TickListeners;



        [Title("Condition")] 
        [ShowInInspector]
        private ICombatEndConditionProvider _conditionProvider;
        private static readonly ICombatEndConditionProvider ProvisionalConditionProvider = new GenericWinCondition();

        public void InjectCondition(ICombatEndConditionProvider conditionProvider)
        {
            _conditionProvider = conditionProvider;
        }


        public void StartTicking()
        {
            _tickingCoroutine = Timing.RunCoroutine(_TickingLoop());
        }

        public bool IsFinish() => !_tickingCoroutine.IsRunning;

        private const float TickPeriodSeconds = .2f;
        private CoroutineHandle _tickingCoroutine;
        private IEnumerator<float> _TickingLoop()
        {
#if UNITY_EDITOR
            Debug.Log("Starting (TICKING)");
#endif

            //Just to give a space to breath before tempo's ticking
            yield return Timing.WaitForSeconds(1);

            if (_conditionProvider == null) 
                _conditionProvider = ProvisionalConditionProvider;

            while (!_conditionProvider.IsCombatFinish())
            {
#if UNITY_EDITOR
                Debug.Log("TICKING...");
#endif


                yield return Timing.WaitForSeconds(TickPeriodSeconds);

                foreach (var tickListener in TickListeners)
                {
                    tickListener.OnTickStep(TickPeriodSeconds);
                }
                yield return Timing.WaitForOneFrame;

            }
        }


        private CoroutineHandle _pausingCoroutineHandle;
        public void PauseTickingUntil(IEnumerator<float> pausingCoroutine)
        {
            if(_pausingCoroutineHandle.IsRunning)
                throw new NotSupportedException($"{typeof(TempoTicker)} can't handle two pausing coroutines at the same time;" +
                                                "Let the current pausing coroutine finish before requesting another pause.");

            if (_tickingCoroutine.IsRunning)
            {
                _pausingCoroutineHandle = Timing.RunCoroutine(pausingCoroutine);
                Timing.WaitForOtherHandles(_tickingCoroutine, _pausingCoroutineHandle);
            }
        }

        public void OnCombatPause()
        {
            _tickingCoroutine.IsAliveAndPaused = true;
            _pausingCoroutineHandle.IsAliveAndPaused = true;
        }
        public void OnCombatResume()
        {
           
            _tickingCoroutine.IsAliveAndPaused = false;
            _pausingCoroutineHandle.IsAliveAndPaused = false;

        }
        public void OnCombatExit()
        {
            _tickingCoroutine.IsRunning = false;
            _pausingCoroutineHandle.IsRunning = false;
        }
    }
    
}
