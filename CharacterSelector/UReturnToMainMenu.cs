using CombatSystem.Player;
using UnityEngine;
using Utils_Extended.UI;

namespace CharacterSelector
{
    public class UReturnToMainMenu : UHoldButton
    {
        private void ReturnToMainMenu()
        {
            Debug.Log("Return to MAIN");
        }

        protected override void DoAction()
        {
            ReturnToMainMenu();
        }
    }
}
