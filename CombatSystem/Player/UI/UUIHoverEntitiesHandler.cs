using System;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

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


        protected override void OnCreateElement(in CreationValues creationValues)
        {
            var element = creationValues.Element;
            var entity = creationValues.Entity;

            base.OnCreateElement(in creationValues);
            element.Show();
            element.EntityInjection(entity);
        }


    }
}
