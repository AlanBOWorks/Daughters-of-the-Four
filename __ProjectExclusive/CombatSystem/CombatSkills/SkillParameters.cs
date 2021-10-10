using CombatEntity;
using Sirenix.OdinInspector;
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

        [ShowInInspector]
        public CombatingSkill UsedSkill { get; private set; }
        [ShowInInspector]
        public CombatingEntity User { get; private set; }
        [ShowInInspector]
        public CombatingEntity Target { get; private set; }
        [ShowInInspector]
        public bool IsCritical { get; private set; }

        public void Inject(CombatingEntity user) => User = user;
        public void Inject(SkillUsageValues values)
        {
            UsedSkill = values.UsedSkill;
            Target = values.Target;
        }

        public void RollForCritical()
        {
            IsCritical = UtilsRandomStats.IsCritical(User.CombatStats,UsedSkill);
        }

        public void OnActionClear()
        {
            UsedSkill = null;
            Target = null;
            IsCritical = false;
        }
        public void Clear()
        {
            UsedSkill = null;
            User = null;
            Target = null;
            IsCritical = false;
        }

        public bool IsValid() => UsedSkill != null && User != null && Target != null;
    }



    internal interface ISkillParameters
    {
        CombatingSkill UsedSkill { get; }
        CombatingEntity User { get; }
        CombatingEntity Target { get; }
    }
}
