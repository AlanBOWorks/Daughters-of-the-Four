using System;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils_Project.UI
{
    public class UButtonVisualEventsHolder : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Title("Params")] 
        [SerializeField] private bool animateOnHoverExit = false;

        [SerializeField] private float hoverDeltaSpeed = 4f;
        [SerializeField] private float clickDeltaSpeed = 4f;

        [SerializeReference,HorizontalGroup("Listeners")]
        private List<IPointerHoverAnimationListener> _hoverListeners = new List<IPointerHoverAnimationListener>();
        [SerializeReference, HorizontalGroup("Listeners")]
        private List<IPointerClickAnimationListener> _clickListeners = new List<IPointerClickAnimationListener>();


        private void Start()
        {
            foreach (var listener in _hoverListeners)
            {
                listener.InitializationHoverListener();
            }

            foreach (var listener in _clickListeners)
            {
                listener.InitializationClickListener();
            }
        }


        [ShowInInspector, HideInEditorMode, HorizontalGroup("Lerps")]
        private float _hoverAnimationLerp;
        private CoroutineHandle _hoverAnimationHandle;
        private IEnumerator<float> _HoverAnimationLoop()
        {
            do
            {
                float delta = Timing.DeltaTime * hoverDeltaSpeed;
                yield return Timing.WaitForOneFrame;
                _hoverAnimationLerp += delta;
                CallEvents();
            } while (_hoverAnimationLerp < 1);

            _hoverAnimationLerp = 1;
            CallEvents();


            void CallEvents()
            {
                foreach (var listener in _hoverListeners)
                {
                    listener.OnTickHoverAnimation(_hoverAnimationLerp);
                }

            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_hoverListeners == null || _hoverListeners.Count == 0) return;

            _hoverAnimationLerp = 0;
            CallEvents();
            if(_hoverAnimationHandle.IsRunning) return;
            _hoverAnimationHandle = Timing.RunCoroutine(_HoverAnimationLoop());

            void CallEvents()
            {
                foreach (var listener in _hoverListeners)
                {
                    listener.OnPointerEnter();
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_hoverListeners == null || _hoverListeners.Count == 0) return;

            _hoverAnimationLerp = 0;
            CallEvents();
            if (!animateOnHoverExit || _hoverAnimationHandle.IsRunning) return;
            _hoverAnimationHandle = Timing.RunCoroutine(_HoverAnimationLoop());

            void CallEvents()
            {
                foreach (var listener in _hoverListeners)
                {
                    listener.OnPointerExit(animateOnHoverExit);
                }
            }
        }



        [ShowInInspector,HideInEditorMode, HorizontalGroup("Lerps")]
        private float _clickAnimationLerp;
        private CoroutineHandle _clickAnimationHandle;
        private IEnumerator<float> _ClickAnimationLoop()
        {
            _clickAnimationLerp = 0;
            do
            {
                float delta = Timing.DeltaTime * clickDeltaSpeed;
                yield return Timing.WaitForOneFrame;
                _clickAnimationLerp += delta;
                CallEvents();
            } while (_clickAnimationLerp < 1);

            _clickAnimationLerp = 1;
            CallEvents();


            void CallEvents()
            {
                foreach (var listener in _clickListeners)
                {
                    listener.OnTickClickAnimation(_clickAnimationLerp);
                }

            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if(_clickListeners == null || _clickListeners.Count == 0) return;

            _clickAnimationLerp = 0;
            CallEvents();
            if (_clickAnimationHandle.IsRunning) return;
            _clickAnimationHandle = Timing.RunCoroutine(_ClickAnimationLoop());

            void CallEvents()
            {
                foreach (var listener in _clickListeners)
                {
                    listener.OnPointerClick();
                }
            }

        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_hoverAnimationHandle);
            Timing.ResumeCoroutines(_clickAnimationHandle);
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_hoverAnimationHandle);
            Timing.PauseCoroutines(_clickAnimationHandle);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_hoverAnimationHandle);
            Timing.KillCoroutines(_clickAnimationHandle);
        }

        private static readonly AnimationCurve EaseOutCurve = new AnimationCurve(
            new Keyframe(0,0), new Keyframe(.3f,1), new Keyframe(1,0));
        private static readonly AnimationCurve EaseInCurve = new AnimationCurve(
            new Keyframe(0,0), new Keyframe(.7f,1), new Keyframe(1,0));


        [Serializable]
        private sealed class ScaleAnimation : IPointerHoverAnimationListener, IPointerClickAnimationListener
        {
            [SerializeField] 
            private RectTransform onTransform;

            [SerializeField, SuffixLabel("px")]
            private Vector2 scaleVariation;

            private Vector2 _initialSize;
            private Vector2 _targetScale;

            public void InitializationHoverListener()
            {
                _initialSize = onTransform.sizeDelta;
                _targetScale = _initialSize + scaleVariation;
            }

            public void InitializationClickListener() => InitializationHoverListener();

            public void OnTickHoverAnimation(float lerpTowardsEnter)
            {
                float targetLerp = EaseOutCurve.Evaluate(lerpTowardsEnter);
                onTransform.sizeDelta = Vector2.Lerp(_initialSize, _targetScale, targetLerp);
            }
            public void OnTickClickAnimation(float currentLerp)
            {
                OnTickHoverAnimation(currentLerp);
            }


            public void OnPointerEnter()
            {
                onTransform.sizeDelta = _initialSize;
            }
            public void OnPointerExit(bool animatesExit)
            {
                OnPointerEnter();
            }
            public void OnPointerClick()
            {
                OnPointerEnter();
            }
        }
        [Serializable]
        private sealed class ScaleUpAnimation : IPointerHoverAnimationListener
        {
            [SerializeField]
            private RectTransform onTransform;

            [SerializeField, SuffixLabel("px")]
            private Vector2 scaleVariation;


            private Vector2 _initialSize;
            private Vector2 _targetScale;
            private bool _isScaleUp;
            public void InitializationHoverListener()
            {
                _initialSize = onTransform.sizeDelta;
                _targetScale = _initialSize + scaleVariation;
            }


            public void OnTickHoverAnimation(float lerpTowardsEnter)
            {
                Vector2 targetScale = _isScaleUp ? _targetScale : _initialSize;
                onTransform.sizeDelta = Vector2.LerpUnclamped(onTransform.sizeDelta, targetScale, lerpTowardsEnter);
            }

            public void OnPointerEnter()
            {
                _isScaleUp = true;
            }

            public void OnPointerExit(bool animatesExit)
            {
                if(animatesExit)
                    _isScaleUp = false;
                else
                    onTransform.sizeDelta = _initialSize;
            }
        }
    }

    /// <summary>
    /// Subscribes to [<see cref="UButtonVisualEventsHolder"/>]
    /// </summary>
    public interface IPointerHoverAnimationListener
    {
        void InitializationHoverListener();
        /// <summary>
        /// Ticks a lerp value; <br></br><br></br>
        /// Note: on exit this lerp value goes from 1 to 0.
        /// </summary>
        /// <param name="lerpTowardsEnter">Unit (0-1) lerp value [Inactive(0) > Active (1)]</param>
        void OnTickHoverAnimation(float lerpTowardsEnter);
        void OnPointerEnter();
        /// <param name="animatesExit">Some animations doesn't perform an exit Animation (snap change)</param>
        void OnPointerExit(bool animatesExit);
    }
    /// <summary>
    /// Subscribes to [<see cref="UButtonVisualEventsHolder"/>]
    /// </summary>
    public interface IPointerClickAnimationListener
    {
        void InitializationClickListener();

        /// <summary>
        /// Ticks a lerp value; <br></br><br></br>
        /// Note: reset from zero on click;
        /// </summary>
        void OnTickClickAnimation(float currentLerp);
        void OnPointerClick();
    }
}
