
namespace CombatSystem.Player
{
    public class UFinishControlButton : UCombatHoldButton
    {
        private static void PassTurn()
        {
            PlayerCombatSingleton.PlayerTeamController.InvokeFinishControl();
        }
    }
}
