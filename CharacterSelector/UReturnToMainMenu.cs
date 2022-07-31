using CombatSystem.Player;
using UnityEngine;
using Utils_Extended.UI;

namespace CharacterSelector
{
    public class UReturnToMainMenu : UHoldButton
    {
        private static void ReturnToMainMenu()
        {
            Utils_Project.UtilsScene.LoadMainMenuScene();
        }

        protected override void DoAction()
        {
            ReturnToMainMenu();
        }
    }
}
