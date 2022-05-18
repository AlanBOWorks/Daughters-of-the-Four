using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem.Player
{
    public abstract class UHoldButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        ITempoTeamStatesListener
    {
        [SerializeField, Range(0, 3), SuffixLabel("seg")] private float holdAmount = .4f;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.ManualSubscribe(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        private void OnDisable()
        {
            Timing.KillCoroutines(_pointerHandle);
        }

        private CoroutineHandle _pointerHandle;
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
                yield return Timing.WaitForSeconds(holdAmount);
                DoAction();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Timing.KillCoroutines(_pointerHandle);
        }

        protected abstract void DoAction();


        protected void Show()
        {
            gameObject.SetActive(true);
        }

        protected void Hide()
        {
            gameObject.SetActive(false);
        }


        public virtual void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            Show();
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            
        }

        public virtual void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public virtual void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            Hide();
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }
    }
}
