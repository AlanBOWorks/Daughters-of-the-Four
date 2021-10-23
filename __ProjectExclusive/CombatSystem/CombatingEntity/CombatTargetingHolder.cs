using System.Collections.Generic;
using CombatTeam;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatEntity
{
    public class CombatTargetingHolder : ITeamTargetingStructure<List<CombatingEntity>>
    {
        public CombatTargetingHolder(CombatingEntity self)
        {
            SelfElement = new List<CombatingEntity>{self};
        }

        public void Inject(CombatingTeam team)
        {
            SelfAlliesElement = new List<CombatingEntity>(team.Count);
            SelfTeamElement = team.LivingEntitiesTracker;

            foreach (var member in team)
            {
                SelfAlliesElement.Add(member);
            }
            foreach (var self in SelfElement)
            {
                SelfAlliesElement.Remove(self);
            }
        }

        [ShowInInspector,HorizontalGroup()]
        public List<CombatingEntity> SelfElement { get; }
        [ShowInInspector, HorizontalGroup()]
        public List<CombatingEntity> SelfTeamElement { get; private set; }

        [ShowInInspector, HorizontalGroup()]
        public List<CombatingEntity> SelfAlliesElement { get; private set; }
    }

}
