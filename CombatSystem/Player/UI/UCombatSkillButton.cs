using System.Collections.Generic;
using CombatSystem._Core;
using MEC;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillButton : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private CoroutineHandle _fadeHandle;
        private const float FadeSpeed = 8f;
        internal void ShowButton()
        {
            gameObject.SetActive(true);

            _fadeHandle = Timing.RunCoroutine(_FadeAlpha());
            CombatSystemSingleton.LinkCoroutineToMaster(_fadeHandle);
            IEnumerator<float> _FadeAlpha()
            {
                canvasGroup.alpha = 0;
                while (canvasGroup.alpha < .98f)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * FadeSpeed);

                    yield return Timing.WaitForOneFrame;
                }

                canvasGroup.alpha = 1;
            }
        }

        internal void HideButton()
        {
            Timing.KillCoroutines(_fadeHandle);
            gameObject.SetActive(false);
            canvasGroup.alpha = 0;
        }
    }
}
