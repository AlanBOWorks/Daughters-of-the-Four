using System;
using System.Collections.Generic;
using MEC;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils_Project.Scene
{
    public class ULoadingIcon : MonoBehaviour
    {
        [Title("Icon")]
        [SerializeField] private MPImage loadingIcon;
        [SerializeField, SuffixLabel("deltas")] private float iconSpeed = 2f;

        private float _lerpAmount;
        private bool _reverseIcon;
        private IEnumerator<float> _Ticking()
        {
            while (enabled)
            {
                if (_reverseIcon)
                    DoDecrementAnimation();
                else
                    DoIncrementAnimation();

                loadingIcon.fillAmount = _lerpAmount;

                UpdateDotsText();

                yield return Timing.WaitForOneFrame;
            }
        }

        private void OnEnable()
        {
            ResetIconState();

            _dotsCounter = 0;
            _dotsTextCounter = 0;

            Timing.RunCoroutine(_Ticking(), Segment.SlowUpdate);
        }

        private void DoIncrementAnimation()
        {
            _lerpAmount += Time.deltaTime * iconSpeed;
            if (_lerpAmount < 1) return;

            FullIconState();
        }

        private void FullIconState()
        {
            _lerpAmount = 1;
            _reverseIcon = true;
            loadingIcon.fillClockwise = false;
        }

        private void DoDecrementAnimation()
        {
            _lerpAmount -= Time.deltaTime * iconSpeed;
            if (_lerpAmount > 0) return;

            ResetIconState();
        }

        private void ResetIconState()
        {
            _lerpAmount = 0;
            _reverseIcon = false;
            loadingIcon.fillClockwise = true;
        }


        [Title("Text")]
        [SerializeField] private TextMeshProUGUI dotsTextHolder;
        [SerializeField, Range(1, 20), SuffixLabel("deltas")] private int dotsUpdateFrameRate = 4;

        private const string OneDotsText = ".";
        private const string TwoDotsText = "..";
        private const string ThreeDotsText = "...";
        private const int OneDotsIndex = 0;
        private const int TwoDotsIndex = 1;
        private const int ThreeDotsIndex = 2;


        private int _dotsCounter;
        private int _dotsTextCounter;
        private void UpdateDotsText()
        {
            _dotsCounter++;
            if(_dotsCounter < dotsUpdateFrameRate) return;

            _dotsCounter = 0;
            string targetDotsText;
            switch (_dotsTextCounter)
            {
                case OneDotsIndex:
                    targetDotsText = OneDotsText;
                    _dotsTextCounter++;
                    break;
                case TwoDotsIndex:
                    targetDotsText = TwoDotsText;
                    _dotsTextCounter++;
                    break;
                default:
                    targetDotsText = ThreeDotsText;
                    _dotsTextCounter = 0;
                    break;
            }

            dotsTextHolder.text = targetDotsText;
        }
    }
}
