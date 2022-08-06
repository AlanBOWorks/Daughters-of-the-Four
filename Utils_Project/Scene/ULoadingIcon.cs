using System;
using System.Collections.Generic;
using MEC;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils_Project.Scene
{
    public class ULoadingIcon : MonoBehaviour, ILoadPercentListener
    {
        [Title("References")] 
        [SerializeField]private ULoadSceneManager manager;

        [Title("Loading")]
        [SerializeField] private MPImage loadingIcon;

        private void Awake()
        {
            manager.SubscribeListener(this);
        }

        private IEnumerator<float> _Ticking()
        {
            while (enabled)
            {
                UpdateDotsText();

                yield return Timing.WaitForOneFrame;
            }
        }

        private void OnEnable()
        {
            _dotsCounter = 0;
            _dotsTextCounter = 0;
            loadingIcon.fillAmount = 0;

            Timing.RunCoroutine(_Ticking(), Segment.SlowUpdate);
        }


        public void OnPercentTick(float loadPercent)
        {
            loadingIcon.fillAmount = loadPercent;
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
