using System;
using CombatSystem._Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UPauseCombatTicking : MonoBehaviour
    {
        [SerializeField] private Image pauseButton;
        [SerializeField] private Image tickingButton;
        
        private const float OnDisableAlpha = .3f;
        private const float OnEnableAlpha = 1f;

        private void OnEnable()
        {
            HandleButtons();
        }

        private void Start()
        {
            var actionsHolder = CombatShortcutsSingleton.InputActions;
            var pauseAction = actionsHolder.PauseTickingShortCutElement;
            pauseAction.action.performed += OnPauseButtonPress;
        }

        private void OnDestroy()
        {
            var actionsHolder = CombatShortcutsSingleton.InputActions;
            var pauseAction = actionsHolder.PauseTickingShortCutElement;
            pauseAction.action.performed -= OnPauseButtonPress;
        }

        private void HandleButtons()
        {
            HandleButtons(CombatSystemSingleton.TempoTicker.PauseTicking);
        }

        public void HandleButtons(bool isPaused)
        {
            ExtractIcons(isPaused,out var enableButton, out var disableButton);
            HandleImageAlpha(enableButton,OnEnableAlpha);
            HandleImageAlpha(disableButton,OnDisableAlpha);
        }

        private void ExtractIcons(bool isPaused, out Image enableButton, out Image disableButton)
        {
            if (isPaused)
            {
                enableButton = pauseButton;
                disableButton = tickingButton;
            }
            else
            {
                enableButton = tickingButton;
                disableButton = pauseButton;
            }
        }

        private static void HandleImageAlpha(Graphic button, float alpha)
        {
            var color = button.color;
            color.a = alpha;
            button.color = color;
        }

        private void OnPauseButtonPress(InputAction.CallbackContext context)
        {
            var tempoTicker = CombatSystemSingleton.TempoTicker;
            var isPaused = tempoTicker.PauseTicking = !tempoTicker.PauseTicking;
            HandleButtons(isPaused);
        }
    }
}
