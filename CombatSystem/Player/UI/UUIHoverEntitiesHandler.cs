using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UOnEntityCreatedSpawnerWithListeners<UUIHoverEntity>
    {
        protected override void Awake()
        {
            base.Awake();
            PlayerCombatSingleton.Injection(base.ActiveElementsDictionary);
        }

        protected override IEntityExistenceElementListener<UUIHoverEntity>[] GetListeners()
        {
            return GetComponents<IUIHoverListener>();
        }

        protected override void OnElementCreated(in UUIHoverEntity element, in CombatEntity entity)
        {
            element.Show();
            base.OnElementCreated(in element, in entity);
        }

    }

    /// <summary>
    /// An [<seealso cref="IEntityExistenceElementListener{T}"/>] where T: [<see cref="UUIHoverEntity"/>];<br></br>
    /// Becomes a listener for [<see cref="UUIHoverEntitiesHandler"/>] if in same component level.
    /// </summary>
    public interface IUIHoverListener : IEntityExistenceElementListener<UUIHoverEntity>
    {
       
    }
}
