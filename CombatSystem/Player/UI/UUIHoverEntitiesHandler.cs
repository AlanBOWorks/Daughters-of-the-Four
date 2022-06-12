using CombatSystem.Entity;
using CombatSystem.Team;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UTeamElementSpawner<UUIHoverEntity>
    {
        private void Start()
        {
            PlayerCombatSingleton.Injection(this);
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }



        protected override void OnCreateElement(CombatEntity entity, UUIHoverEntity element,
            int index)
        {
            element.Show();
            element.EntityInjection(in entity);
        }
    }
}
