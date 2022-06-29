using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.AI.Enemy;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyCombatEventsHolder : ControllerCombatEventsHolder, ITempoTickListener, IDiscriminationEventsHolder
    {
        public EnemyCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);



#if UNITY_EDITOR
#endif
        }

    }

}

