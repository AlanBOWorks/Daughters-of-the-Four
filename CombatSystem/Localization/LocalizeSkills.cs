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
    }
}
