using System;
using System.Collections.Generic;
using MEC;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CharacterSelector
{
    public class UStartRunHandler : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private MPImage fillerImage;

        private USelectedCharactersHolder _holder;
        public void Injection(USelectedCharactersHolder holder) => _holder = holder;

        private bool _isReady;
        private const float OnDisableAlpha = .3f;
        public void EnableControl()
        {
            canvasGroup.alpha = 1;
            _isReady = true;
        }
        public void DisableControl()
        {
            canvasGroup.alpha = OnDisableAlpha;
            _isReady = false;
        }

        private bool _confirmingSelection;
        private float _currentConfirmationAmount;

        private const float CheckStopAfter = .4f;
        private const float DeltaSpeed = 2f;

        private const float InitialPercentOnClick = .2f;
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!_isReady || _confirmingSelection) return;

            _currentConfirmationAmount = InitialPercentOnClick;
            _confirmingSelection = true;
            if (!_confirmationHandle.IsRunning)
                _confirmationHandle = Timing.RunCoroutine(_CheckConfirmation());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _confirmingSelection = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _confirmingSelection = false;
        }

        private CoroutineHandle _confirmationHandle;
        private IEnumerator<float> _CheckConfirmation()
        {
            while (_currentConfirmationAmount < 1)
            {
                yield return Timing.WaitForOneFrame;

                float deltaStep = Timing.DeltaTime * DeltaSpeed;
                _currentConfirmationAmount += deltaStep;
                if (
                    (!_isReady || !_confirmingSelection) 
                    && (_currentConfirmationAmount > CheckStopAfter))
                {
                    CancelConfirmation();
                    yield break;
                }

                fillerImage.fillAmount = _currentConfirmationAmount;
            }
            DoConfirmation();
        }

        private void CancelConfirmation()
        {
            fillerImage.fillAmount = 0;
        }

        private void DoConfirmation()
        {
            fillerImage.fillAmount = 1;
            _holder.ConfirmTeamAndSendToSingleton();
        }

    }
}
