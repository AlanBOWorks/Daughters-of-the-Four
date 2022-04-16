using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UOnEntityCreatedSpawnerWithListeners<UUIHoverEntityHolder>
    {
        protected override IEntityExistenceElementListener<UUIHoverEntityHolder>[] GetListeners()
        {
            return GetComponents<IUIHoverListener>();
        }

        protected override void OnElementCreated(in UUIHoverEntityHolder element, in CombatEntity entity)
        {
            element.Show();
            base.OnElementCreated(in element, in entity);
        }

        /*
        public override void OnCombatStart()
        {
            Show();
        }

        public override void OnCombatQuit()
        {
            Hide();
        }

        public override void OnCombatFinish(bool isPlayerWin)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }*/
    }

    /// <summary>
    /// An [<seealso cref="IEntityExistenceElementListener{T}"/>] where T: [<see cref="UUIHoverEntityHolder"/>];<br></br>
    /// Becomes a listener for [<see cref="UUIHoverEntitiesHandler"/>] if in same component level.
    /// </summary>
    public interface IUIHoverListener : IEntityExistenceElementListener<UUIHoverEntityHolder>
    {
       
    }
}
