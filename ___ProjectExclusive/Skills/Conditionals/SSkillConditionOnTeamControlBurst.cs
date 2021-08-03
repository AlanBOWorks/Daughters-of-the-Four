using _Team;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N - Team BURST Check" + SkillUseConditionPrefix,
        menuName = "Combat/Conditions/Skill/Team Burst check")]
    public class SSkillConditionOnTeamControlBurst : SSkillUseConditionBase
    {
        public override bool CanUseSkill(CombatingEntity user, float conditionalCheck)
        {
            var userTeam = user.CharacterGroup.Team;
            return userTeam.State.IsBurstStance;
        }

        protected override string GenerateAssetName()
        {
            return "Team BURST Check" + base.GenerateAssetName();
        }
    }
}
