using CombatSystem._Core;
using ExplorationSystem;

namespace _Injectors.ExplorationCombat
{
    public class SceneTransitionHandler : IExplorationSubmitListener, ICombatTerminationListener
    {
        public void OnExplorationRequest(EnumExploration.ExplorationType type)
        {
            // todo move the load combat here
        }

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            // todo Invoke the transitions here with the ICallback
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            // nothing?
        }
    }
}
