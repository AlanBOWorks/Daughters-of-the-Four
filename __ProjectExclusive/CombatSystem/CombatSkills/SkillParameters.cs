using CombatEntity;
using UnityEngine;

namespace CombatSkills
{

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
        public CombatingEntity User { get; private set; }
        public CombatingEntity Target { get; private set; }
        public bool IsCritical { get; private set; }

        public void Inject(SkillParameters parameters)
        {
            User = parameters.User;
            Target = parameters.Target;
            IsCritical = parameters.IsCritical;
        }
    }

    public struct SkillParameters : ISkillParameters
    {
        public SkillParameters(CombatingEntity user, CombatingEntity target, bool isCritical = false)
        {
            User = user;
            Target = target;
            IsCritical = isCritical;
        }

        public CombatingEntity User { get; }
        public CombatingEntity Target { get; set; } //Set because: This can be switch by guarding skills
        public bool IsCritical { get; set; } //Set because: This can be altered by special randomness
    }


    internal interface ISkillParameters
    {
        CombatingEntity User { get; }
        CombatingEntity Target { get; }
        bool IsCritical { get; }
    }
}
