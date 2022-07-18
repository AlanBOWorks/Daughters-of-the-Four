using CombatSystem.Skills;

namespace CombatSystem.Localization
{
    public static class LocalizeSkills 
    {
        public static string LocalizeSkill(ICombatSkill skill)
        {
            var skillTag = skill.Preset.GetSkillName();
            return CombatLocalizations.LocalizeSkillName(skillTag);
        }

        private const string TensCostText = "X";
        private const string HundredsCostText = "XX";
        public static string LocalizeSkillCost(ISkill skill)
        {
            int cost = skill.SkillCost;
            if (cost > 99) return HundredsCostText;
            return cost.ToString();
        }

        public static string LocalizeSkillCostSingle(ISkill skill)
        {
            int cost = skill.SkillCost;
            if (cost > 9) return TensCostText;
            return cost.ToString();
        }

    }
}
