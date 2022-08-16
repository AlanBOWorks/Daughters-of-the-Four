using System;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace ExplorationSystem.Elements
{
    public class UWorldElementAnimator : MonoBehaviour
    {
        [Title("Main Image")]
        [SerializeField] private RectTransform iconHolder;

        [SerializeField, SuffixLabel("deltas")] private float deltaPulsationSpeed = 4;
        [SerializeField] private SCurve pulsationCurve;

        [Title("Borders")]
        [SerializeField] private float deltaRotationSpeed = 24;
        [SerializeField] private RectTransform[] rotationElements;


        private float _currentLerp;
        private Vector2 _targetSize;
        private Vector2 _initialSize;
        private const float IncrementPercent = 2f;

        private void Awake()
        {
            _initialSize = iconHolder.sizeDelta;
            _targetSize = _initialSize * IncrementPercent;
        }

        private void Update()
        {
            TickIcon();
            TickRotations();
        }


        private void TickIcon()
        {
            float deltaStep = Time.deltaTime * deltaPulsationSpeed;
            _currentLerp += deltaStep;
            float targetLerp = pulsationCurve.Evaluate(_currentLerp);
            iconHolder.sizeDelta = Vector2.LerpUnclamped(_initialSize, _targetSize, targetLerp);
        }

        private void TickRotations()
        {
            float deltaStep = Time.deltaTime * deltaRotationSpeed;
            for (int i = 0; i < rotationElements.Length; i++)
            {
                var element = rotationElements[i];
                float rotationAngle = (i + 1) * deltaStep;
                Quaternion deltaRotation = Quaternion.Euler(0,0, rotationAngle);
                element.localRotation *= deltaRotation;
            }

        }
    }
}
