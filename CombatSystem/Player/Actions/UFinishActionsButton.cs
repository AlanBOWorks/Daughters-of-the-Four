using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem.Player
{
    public class UFinishActionsButton : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler,
        ITempoEntityStatesListener
    {
        [SerializeField, Range(0,3), SuffixLabel("seg")] private float holdAmount = .4f;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.ManualSubscribe(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        private CoroutineHandle _pointerHandle;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (holdAmount <= 0)
            {
                //todo invoke
                return;
            }
            if (_pointerHandle.IsRunning) return;
            _pointerHandle = Timing.RunCoroutine(_HoldClick(), Segment.RealtimeUpdate);

            IEnumerator<float> _HoldClick()
            {
                yield return Timing.WaitForSeconds(holdAmount);
                PassTurn();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Timing.KillCoroutines(_pointerHandle);
        }

        private static void PassTurn()
        {
            PlayerCombatSingleton.PlayerTeamController.ForceFinishControl();
            Debug.Log("Passing Turn [PLAYER]");
        }


        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if (canAct)
            {
                Show();
            }
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            Hide();
        }

        public void OnTempoFinishControl(CombatEntity mainEntity)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
