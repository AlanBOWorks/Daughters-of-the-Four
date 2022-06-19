using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using MPUIKIT;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace CombatSystem.Player
{
    public abstract class UHoldButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        ITempoTeamStatesListener,
        IOverridePauseElement
    {
        [Title("References")]
        [SerializeField] private MPImage percentFiller;

        [Title("Params")]
        [SerializeField, Range(0, 3), SuffixLabel("seg")] 
        private float holdAmount = .4f;

        [SerializeField] 
        private UltEvent onHoldSuccess = new UltEvent();


        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.ManualSubscribe(this);
            gameObject.SetActive(false);
            FillImage(0);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        private void OnDisable()
        {
            ResetHoldState();
        }

        private void ResetHoldState()
        {
            if (!_pointerHandle.IsRunning) return;

            Timing.KillCoroutines(_pointerHandle);
            FillImage(0);
            RemoveFromPauseStateHandler();
        }

        public void OnPauseInputReturnState(IOverridePauseElement lastElement)
        {
            ResetHoldState();
        }

        private void AddsToPauseStateHandler()
        {
            PlayerCombatSingleton.GetCombatEscapeButtonHandler().PushOverridingAction(this);

            // Reminder: The PauseHandler already clears the stack once the Control is Finish 
        }
        private void RemoveFromPauseStateHandler()
        {
            PlayerCombatSingleton.GetCombatEscapeButtonHandler().RemoveIfLast(this);
        }

        private void FillImage(float percent)
        {
            if (!percentFiller) return;
            percentFiller.fillAmount = percent;
        }


        private CoroutineHandle _pointerHandle;
        private const float MinPercentHoldFillImage = .12f;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (holdAmount <= 0)
            {
                DoAction();
                return;
            }
            if (_pointerHandle.IsRunning) return;
            _pointerHandle = Timing.RunCoroutine(_HoldClick(), Segment.RealtimeUpdate);

            IEnumerator<float> _HoldClick()
            {
                AddsToPauseStateHandler();
                float timer = 0;
                while (timer < holdAmount)
                {
                    float delta = Timing.DeltaTime;
                    yield return delta;
                    timer += delta;

                    if(!percentFiller) continue;

                    float percent = SRange.Percent(timer, holdAmount);
                    if (percent < MinPercentHoldFillImage) percent = MinPercentHoldFillImage;

                    percentFiller.fillAmount = percent;
                }
                FillImage(0);
                DoAction();
            }
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            ResetHoldState();
        }

        protected virtual void DoAction()
        {
            onHoldSuccess.Invoke();
            RemoveFromPauseStateHandler();
        }


        protected void Show()
        {
            gameObject.SetActive(true);
        }

        protected void Hide()
        {
            gameObject.SetActive(false);
        }


        public virtual void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            Show();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            
        }

        public virtual void OnControlFinishAllActors(CombatEntity lastActor)
        {
        }

        public virtual void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            Hide();
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }

    }
}
