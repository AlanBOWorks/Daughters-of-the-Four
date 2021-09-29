using System;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSystem.Enemy;
using UnityEngine;

namespace CombatSystem
{
    internal class EntityTempoHandlerSelector : IEntityTempoHandler
    {
        public IEnumerator<float> _RequestFinishAction(CombatingEntity entity)
        {
            return CombatSystemSingleton.VolatilePlayerTeam.Contains(entity) //Is Players?

                    ? PlayerCombatSingleton.EntityTempoHandler._RequestFinishAction(entity) 
                    : EnemyCombatSingleton.EntityTempoHandler._RequestFinishAction(entity);
        }
    }
}
