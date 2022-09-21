using System;
using System.Collections.Generic;
using MEC;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils_Project.Scene
{
    [Serializable]
    internal sealed class MainSceneLoadTransitionWrapper : ILoadSceneAnimator
    {
        [Title("References")] 
        [SerializeField] private GameObject rootActiveToggle;
        [SerializeField] private MPImage fillerImageMask;
        [SerializeField] private MPImage percentImage;
        [SerializeField] private RectTransform mainIconRoot;

        [Title("Params")]
        [SerializeField] private float hideDeltaSpeed = 4;

        public void Awake()
        {
            _mainIconInitialPosition = mainIconRoot.anchoredPosition;
            rootActiveToggle.SetActive(false);
        }

        public void HandleFillFromLeft(bool isLeftFill)
        {
            var fillIndex = isLeftFill ? 1 : 0;
            fillerImageMask.fillOrigin = fillIndex;
        }

        private void DoFilling(float deltaStep)
        {
            fillerImageMask.fillAmount += deltaStep;
        }

        private void DoMainIconMovement(float percent)
        {
            mainIconRoot.anchoredPosition = Vector2.LerpUnclamped(_mainIconMovement, _mainIconInitialPosition, percent);
        }

        private bool _isLeftFill;
        public void InitializeState(bool isLeftFill)
        {
            _isLeftFill = isLeftFill;
        }

        private Vector2 _mainIconInitialPosition;
        private Vector2 _mainIconMovement;
        private const float MainIconLateralPosition = 100;
        private void PrepareFillingState(bool isLeftFill)
        {
            if (isLeftFill)
            {
                fillerImageMask.fillOrigin = 0;
                _mainIconMovement = new Vector2(-MainIconLateralPosition,_mainIconInitialPosition.y); //From Left
            }
            else
            {
                fillerImageMask.fillOrigin = 1;
                _mainIconMovement = new Vector2(MainIconLateralPosition, _mainIconInitialPosition.y); //From Right
            }
        }

        public void SetActive(bool active)
        {
            rootActiveToggle.SetActive(active);
        }

        public IEnumerator<float> _DoInitialAnimation()
        {
            PrepareFillingState(_isLeftFill);
            percentImage.fillAmount = 0;
            fillerImageMask.fillAmount = 0;

            do
            {
                float deltaStep = Timing.DeltaTime * hideDeltaSpeed;
                DoFilling(deltaStep);
                if (fillerImageMask.fillAmount > 1) fillerImageMask.fillAmount = 1;

                DoMainIconMovement(fillerImageMask.fillAmount);

                yield return Timing.WaitForOneFrame;
            }while(fillerImageMask.fillAmount < 1);
        }

        public void TickingLoad(float currentPercent)
        {
            percentImage.fillAmount = currentPercent;
        }

        public IEnumerator<float> _OnAfterLoadAnimation()
        {
            PrepareFillingState(!_isLeftFill);
            percentImage.fillAmount = 1;
            fillerImageMask.fillAmount = 1;

            do
            {
                float deltaStep = -Timing.DeltaTime * hideDeltaSpeed;
                DoFilling(deltaStep);
                if (fillerImageMask.fillAmount < 0) fillerImageMask.fillAmount = 0;

                DoMainIconMovement(fillerImageMask.fillAmount);
                yield return Timing.WaitForOneFrame;
            } while (fillerImageMask.fillAmount > 0);
        }
    }

    [Serializable]
    internal sealed class CombatLoadTransitionWrapper : ILoadSceneAnimator
    {
        [Title("References")]
        [SerializeField] private GameObject rootGameObject;
        [SerializeField] private CanvasGroup alphaGroup;
        [SerializeField] private RectTransform mainIconRoot;
        [SerializeField] private MPImage percentLoadImage;

        [Title("Params")] 
        [SerializeField] private float deltaSpeed = 4f;
        [SerializeField] private AnimationCurve alphaCurve = new AnimationCurve();

        public void Awake()
        {
            rootGameObject.SetActive(false);
        }

        public void SetActive(bool active)
        {
            rootGameObject.SetActive(active);
        }


        private void Animate(float percent)
        {
            mainIconRoot.localScale = Vector3.LerpUnclamped(_mainIconInitialScale, Vector3.one, percent);
            alphaGroup.alpha = alphaCurve.Evaluate(percent);
        }

        private static Vector3 _mainIconInitialScale = new Vector3(.5f,.5f,1);
        public IEnumerator<float> _DoInitialAnimation()
        {
            float percent = 0;
            do
            {
                percent += Timing.DeltaTime * deltaSpeed;
                if (percent > 1) percent = 1;
                Animate(percent);
                yield return Timing.WaitForOneFrame;
            }
            while (percent < 1);
        }

        public void TickingLoad(float currentPercent)
        {
            percentLoadImage.fillAmount = currentPercent;
        }

        public IEnumerator<float> _OnAfterLoadAnimation()
        {
            float percent = 1;
            do
            {
                percent -= Timing.DeltaTime * deltaSpeed;
                if (percent < 0) percent = 0;
                Animate(percent);
                yield return Timing.WaitForOneFrame;
            }
            while (percent > 0);

        }
    }
}
