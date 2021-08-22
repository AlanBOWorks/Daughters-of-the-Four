using _Team;
using Characters;
using CombatConditions;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "TEAM IsBurst [Condition]",
        menuName = "Combat/Conditions/Team Is Burst")]
    public class STeamIsBurstCondition : SCondition
    {
        public override bool CanApply(CombatingEntity target, float checkValue)
        {
            var userTeam = target.CharacterGroup.Team;
            return userTeam.State.IsBurstStance;
        }
    }
}
