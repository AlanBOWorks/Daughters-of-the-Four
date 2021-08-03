using _Team;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N - Team Stance" + SkillUseConditionPrefix,
        menuName = "Combat/Conditions/Skill/Team Stance")]
    public class SSkillConditionOnTeamStance : SSkillUseConditionBase
    {
        [SerializeField] private TeamCombatState.Stance checkStance =
            TeamCombatState.Stance.Neutral;

        public override bool CanUseSkill(CombatingEntity user, float conditionalCheck)
        {
            var userTeam = user.CharacterGroup.Team;
            return userTeam.State.stance == checkStance;
        }

        protected override string GenerateAssetName()
        {
            return "Team State Check_" + checkStance.ToString().ToUpper() + base.GenerateAssetName();
        }
    }
}
