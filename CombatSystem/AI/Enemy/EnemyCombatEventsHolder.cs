using CombatSystem._Core;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyCombatEventsHolder : ControllerCombatEventsHolder, ITempoTickListener, IDiscriminationEventsHolder
    {
        public EnemyCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);
        }

    }
}

