using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.Player.UI
{
    public class UFinishControlShortcutHandler : MonoBehaviour
    {
        [SerializeField] private UFinishControlButton button;

        private void Awake()
        {
            var actionsReference = CombatShortcutsSingleton.InputActions;
            var nextRoundActionReference = actionsReference.EndControlShortCutElement.action;

            nextRoundActionReference.performed += OnPointerDown;
            nextRoundActionReference.canceled += OnPointerUp;
        }

        private void OnDestroy()
        {
            var actionsReference = CombatShortcutsSingleton.InputActions;
            var nextRoundActionReference = actionsReference.EndControlShortCutElement.action;

            nextRoundActionReference.performed -= OnPointerDown;
            nextRoundActionReference.canceled -= OnPointerUp;
        }

        private void OnPointerDown(InputAction.CallbackContext context)
        {
            button.OnPointerDown();
        }

        private void OnPointerUp(InputAction.CallbackContext context)
        {
            button.OnPointerUp();
        }
    }
}
