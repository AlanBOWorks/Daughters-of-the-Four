using CombatSystem.Player;
using UnityEngine;
using Utils_Extended.UI;
using Utils_Project;

namespace CharacterSelector
{
    public class UReturnToMainMenu : UHoldButton
    {
        private static void ReturnToMainMenu()
        {
            Utils_Project.UtilsScene.LoadMainMenuScene(false, LoadCallBacks.NullCallBacks);
        }

        protected override void DoAction()
        {
            ReturnToMainMenu();
        }
    }
}
