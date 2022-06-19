using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team.VanguardEffects
{
    public sealed class VanguardEventsHandler : ISkillUsageListener
    {
        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            var skill = values.UsedSkill;
            if (!UtilsSkill.IsOffensiveSkill(skill)) return;

            var attacker = values.Performer;
            var target = values.Target;
            UtilsVanguardEffects.HandleVanguardOffensive(attacker,target);
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }
    }
}
