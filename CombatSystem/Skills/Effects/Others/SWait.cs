using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public class SWait : SEffect
    {
        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            UtilsCombatTeam.PutOnStandBy(in target);
        }
    }
}
