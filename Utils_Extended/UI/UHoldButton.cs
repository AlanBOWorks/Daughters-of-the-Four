using System.Collections.Generic;
using MEC;
using MPUIKIT;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Utils_Extended.UI
{
    public abstract class UHoldButton : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler
    {
        [Title("References")]
        [SerializeField] private MPImage percentFiller;

        [Title("Params")]
        [SerializeField, Range(0, 3), SuffixLabel("seg")]
        private float holdAmount = .4f;



        private void Awake()
        {
            FillImage(0);
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
            OnPointerDown();
        }

        public void OnPointerDown()
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
                float timer = 0;
                while (timer < holdAmount)
                {
                    float delta = Timing.DeltaTime;
                    yield return delta;
                    timer += delta;

                    if (!percentFiller) continue;

                    float percent = SRange.Percent(timer, holdAmount);
                    if (percent < MinPercentHoldFillImage) percent = MinPercentHoldFillImage;

                    percentFiller.fillAmount = percent;
                }
                FillImage(0);
                DoAction();
            }
        }

        public void OnPointerUp()
        {
            ResetHoldState();
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            ResetHoldState();
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
    }

    public abstract class UltHoldButton : UHoldButton
    {
        [SerializeField]
        private UltEvent onHoldSuccess = new UltEvent();

        protected override void DoAction()
        {
            onHoldSuccess.Invoke();
        }
    }
}
