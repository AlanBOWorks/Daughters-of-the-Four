
namespace CombatSystem.Player
{
    public class UFinishControlButton : UHoldButton
    {
        private static void PassTurn()
        {
            PlayerCombatSingleton.PlayerTeamController.InvokeFinishControl();
        }
    }
}
