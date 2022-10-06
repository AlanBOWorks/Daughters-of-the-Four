using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Handlers
{
    public sealed class EntitiesOnTempoChangesHandler 
    {
        public static void OnStanceChange(CombatTeam team, bool isForcedChange)
        {
            if(isForcedChange) return;

            var activeEntities = team.GetControllingMembers();
            foreach (var entity in activeEntities)
            {
                UtilsCombatStats.ResetActions(entity.Stats);
            }
        }
    }
}
