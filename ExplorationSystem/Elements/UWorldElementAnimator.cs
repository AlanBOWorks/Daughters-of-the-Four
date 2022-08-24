using System;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace ExplorationSystem.Elements
{
    public class UWorldElementAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Title("Main Holders")] 
        [SerializeField] private RectTransform onHoverSizeHolder;
        [SerializeField] private RectTransform iconHolder;
        
        [Title("Borders")]
        [SerializeField] private MPImage[] rotationElements;

        [Title("Anim. Params.")]
        [SerializeField, SuffixLabel("deltas")] private float deltaPulsationSpeed = 4;
        [SerializeField] private SCurve pulsationCurve;
        [SerializeField, SuffixLabel("deltas")] private float deltaRotationSpeed = 24;



        private float _currentIconLerp;
        private Vector2 _targetSize;
        private Vector2 _initialSize;
        private const float IncrementPercent = 2f;

        private void Awake()
        {
            _initialSize = iconHolder.sizeDelta;
            _targetSize = _initialSize * IncrementPercent;
            VariateDeltas();
        }
        private void VariateDeltas()
        {
            float randomUnit = Random.value;
            _currentIconLerp = randomUnit;
            TickRotations(randomUnit * 75f);

            randomUnit *= .25f;
            randomUnit += 1;

            deltaRotationSpeed *= randomUnit;
            deltaPulsationSpeed *= randomUnit;
        }


        private void Update()
        {
            TickIcon();
            TickRotations();
            TickHover();
        }


        private void TickIcon()
        {
            float deltaStep = Time.deltaTime * deltaPulsationSpeed;
            _currentIconLerp += deltaStep;
            float targetLerp = pulsationCurve.Evaluate(_currentIconLerp);
            iconHolder.sizeDelta = Vector2.LerpUnclamped(_initialSize, _targetSize, targetLerp);
        }



        private const float HoverSizeModifier = 1.25f;
        private static readonly Vector3 HoverTargetScale = new Vector3(HoverSizeModifier,HoverSizeModifier);
        private void TickHover()
        {
            _currentHoverLerp = Mathf.Lerp(_currentHoverLerp, _hoverTargetLerp, Time.deltaTime * 4);

            onHoverSizeHolder.localScale = Vector3.LerpUnclamped(
                Vector3.one, HoverTargetScale, _currentHoverLerp);
        }

        private void TickRotations()
        {
            float deltaStep = Time.deltaTime * deltaRotationSpeed;
            TickRotations(deltaStep);
        }
        private void TickRotations(float deltaStep)
        {
            for (int i = 0; i < rotationElements.Length; i++)
            {
                var element = rotationElements[i];
                float rotationAngle = (i + 1) * deltaStep;
                element.ShapeRotation += rotationAngle;
            }

        }

        private float _hoverTargetLerp;
        private float _currentHoverLerp;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoverTargetLerp = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hoverTargetLerp = 0;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            _currentHoverLerp = 0.3f;
        }
    }
}
