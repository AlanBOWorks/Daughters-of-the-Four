using CombatSystem.Skills;
using Localization.Combat;

namespace CombatSystem.Localization
{
    public static class LocalizeSkills 
    {
        public static string LocalizeSkill(CombatSkill skill)
        {
            var skillTag = skill.Preset.GetSkillName();
            return CombatLocalizations.LocalizeSkillName(in skillTag);
        }
    }
}
