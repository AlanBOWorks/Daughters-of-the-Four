using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using MEC;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Utils_Extended.UI
{
    public class UButtonColorChanger : MonoBehaviour
    {
        [SerializeField] private  IconColorChanger colorChangerBehaviour = new IconColorChanger();

        public void Animate(Color initialColor, Color targetColor)
            => colorChangerBehaviour.Animate(initialColor, targetColor);

        private void OnDisable()
        {
            Timing.PauseCoroutines(colorChangerBehaviour.CurrentHandle);
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(colorChangerBehaviour.CurrentHandle);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(colorChangerBehaviour.CurrentHandle);
        }
    }

    [Serializable]
    public sealed class IconColorChanger
    {
        [SerializeField] private Image imageHolder;
        [SerializeField] private float deltaSpeed = 4f;
        [SerializeField] private SCurve curve;

        private float _currentLerp;

        public void ResetState()
        {
            _currentLerp = 0;
        }

        public void Injection(Sprite imageSprite)
        {
            imageHolder.sprite = imageSprite;
        }

        public void InstantColorChange(Color imageColor)
        {
            var coroutine = CurrentHandle;
            if(coroutine.IsRunning)
                Timing.KillCoroutines(CurrentHandle);
            imageHolder.color = imageColor;
        }

        public CoroutineHandle CurrentHandle { get; private set; }

        public void Animate(Color initialColor, Color targetColor)
        {
            Timing.KillCoroutines(CurrentHandle);
            CurrentHandle = Timing.RunCoroutine(_DoAnimation(initialColor, targetColor));
        }

        public void Animate(Color targetColor) => Animate(imageHolder.color, targetColor);

        private IEnumerator<float> _DoAnimation(Color initialColor, Color targetColor)
        {
            _currentLerp = 0;
            do
            {
                float deltaVariation = Timing.DeltaTime * deltaSpeed;
                yield return Timing.WaitForOneFrame;
                _currentLerp += deltaVariation;
                float targetLerp = curve.Evaluate(_currentLerp);
                Color currentColor = Color.LerpUnclamped(initialColor, targetColor, targetLerp);
                imageHolder.color = currentColor;
            } while (_currentLerp < 1);

            imageHolder.color = targetColor;
        }
    }
}


