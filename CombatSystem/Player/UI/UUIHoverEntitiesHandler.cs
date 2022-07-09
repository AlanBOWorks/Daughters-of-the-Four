using CombatSystem.Entity;
using CombatSystem.Team;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UTeamElementSpawner<UUIHoverEntityHolder>
    {
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }



        protected override void OnCreateElement(CombatEntity entity, UUIHoverEntityHolder element,
            int index)
        {
            element.Show();
            element.EntityInjection(entity);
        }
    }
}
