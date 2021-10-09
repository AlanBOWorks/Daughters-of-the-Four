using CombatEntity;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public struct SkillParameters : ISkillParameters
    {
        public SkillParameters(CombatingSkill skill, CombatingEntity user, CombatingEntity target)
        {
            UsedSkill = skill;
            User = user;
            Target = target;
        }

        public CombatingSkill UsedSkill { get; }
        public CombatingEntity User { get; }
        public CombatingEntity Target { get; set; } //Set because: This can be switch by guarding skills
    }

    public struct SkillUsageValues
    {
        public readonly CombatingSkill UsedSkill;
        public readonly CombatingEntity Target;

        public SkillUsageValues(CombatingSkill usedSkill, CombatingEntity target)
        {
            UsedSkill = usedSkill;
            Target = target;
        }
    }

    public sealed class SkillValuesHolders : ISkillParameters
    {
        public SkillValuesHolders() {} //this is just for seeing the references amount

        public CombatingSkill UsedSkill { get; private set; }
        public CombatingEntity User { get; private set; }
        public CombatingEntity Target { get; private set; }
        public bool IsCritical { get; private set; }

        public void Inject(SkillParameters parameters)
        {
            UsedSkill = parameters.UsedSkill;
            User = parameters.User;
            Target = parameters.Target;
        }

        public void RollForCritical()
        {
            IsCritical = UtilsRandomStats.IsCritical(User.CombatStats,UsedSkill);
        }

        public void Clear()
        {
            UsedSkill = null;
            User = null;
            Target = null;
            IsCritical = false;
        }
    }



    internal interface ISkillParameters
    {
        CombatingSkill UsedSkill { get; }
        CombatingEntity User { get; }
        CombatingEntity Target { get; }
    }
}
