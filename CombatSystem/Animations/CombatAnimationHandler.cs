using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Animations
{
    public sealed class CombatAnimationHandler : ISkillUsageListener 
    {
        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish(in CombatEntity performer)
        {
        }
    }
}
