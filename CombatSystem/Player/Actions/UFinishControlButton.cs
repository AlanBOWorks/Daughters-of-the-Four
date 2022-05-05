
namespace CombatSystem.Player
{
    public class UFinishControlButton : UHoldButton
    {
        private static void PassTurn()
        {
            PlayerCombatSingleton.PlayerTeamController.ForceFinish();
        }


        protected override void DoAction()
        {
            PassTurn();
        }
    }
}
