using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.Player
{
    public class UEscapeButtonInvoker : MonoBehaviour
    {
        [SerializeField] private InputActionReference pauseAction;


        private void Awake()
        {
            pauseAction.action.performed += InvokeOnEscapeButtonPressed;
        }

        private void OnDestroy()
        {
            pauseAction.action.performed -= InvokeOnEscapeButtonPressed;
        }


        private void InvokeOnEscapeButtonPressed(InputAction.CallbackContext context)
        {
            var eventWrapper = PlayerCombatSingleton.CombatEscapeButtonHandler;
            eventWrapper.InvokeEscapeButtonAction();
        }


    }
}
