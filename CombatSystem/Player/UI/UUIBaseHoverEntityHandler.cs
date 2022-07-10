using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIBaseHoverEntityHandler : UTeamElementSpawner<UUIBaseHoverEntityHolder>
    {
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }



        protected override void OnCreateElement(CombatEntity entity, UUIBaseHoverEntityHolder element,
            int index)
        {
            element.Show();
            element.EntityInjection(entity);
        }
    }
}
