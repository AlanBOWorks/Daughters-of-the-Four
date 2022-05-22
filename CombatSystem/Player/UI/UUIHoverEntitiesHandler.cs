using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UTeamElementSpawner<UUIHoverEntity>
    {
        private void Start()
        {
            PlayerCombatSingleton.Injection(base.GetDictionary());
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
