using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class EntityTempoStepper 
    {
        private static void HandleTempoStats(CombatEntity entity)
        {
            var entityTeam = entity.Team;
            entityTeam.OnEntityRequestSequence(entity);

            var stats = entity.Stats;
            UtilsCombatStats.ResetTempoStats(stats);
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            if(!canControl) return;
            HandleTempoStats(entity);
        }
    }
}
