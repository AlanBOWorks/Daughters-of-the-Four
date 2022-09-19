using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils_Project.Scene
{
    public class ULoadingDotsAnimator : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] private TextMeshProUGUI dotsTextHolder;
        [SerializeField, Range(1, 20), SuffixLabel("deltas")] private int dotsUpdateFrameRate = 4;


        private void Update()
        {
            UpdateDotsText();
        }

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
