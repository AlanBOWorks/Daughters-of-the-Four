using CombatEntity;
using Sirenix.OdinInspector;
using Stats;
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
        public SkillValuesHolders() {} //this is just for seeing the references amount

        public SkillValuesHolders(CombatingEntity target)
        {
            Target = target;
        }

        [ShowInInspector]
        public CombatingSkill UsedSkill { get; private set; }
        [ShowInInspector]
        public CombatingEntity Performer { get; private set; }
        [ShowInInspector]
        public CombatingEntity Target { get; private set; }
        [ShowInInspector]
        public bool IsCritical { get; private set; }


        public void SwitchTarget(CombatingEntity newTarget) => Target = newTarget;
        public void Inject(CombatingEntity performer) => Performer = performer;
        public void Inject(SkillUsageValues values)
        {
            UsedSkill = values.UsedSkill;
            Target = values.Target;
        }

        public void RollForCritical(CombatingEntity performer)
        {
            IsCritical = UtilsRandomStats.IsCritical(performer.CombatStats, UsedSkill);
        }
        public void RollForCritical() => RollForCritical(Performer);

        /// <summary>
        /// Rolls for critical only using the raw values of the Stats (<see cref="RollForCritical"/> used the <see cref="UsedSkill"/>)
        /// </summary>
        public void RollForCriticalRawStats(CombatingEntity performer)
        {
            IsCritical = UtilsRandomStats.IsCritical(performer.CombatStats);

        }
        public void RollForCriticalRawStats() => RollForCriticalRawStats(Performer);


        public void OnActionClear()
        {
            UsedSkill = null;
            Target = null;
            IsCritical = false;
        }
        public void Clear()
        {
            UsedSkill = null;
            Performer = null;
            Target = null;
            IsCritical = false;
        }

        public bool IsValid() => UsedSkill != null && Performer != null && Target != null;
    }

    public interface ISkillValues
    {
        CombatingEntity Performer { get; }
        CombatingEntity Target { get; }
        bool IsCritical { get; }
    }

    public interface ISkillParameters : ISkillValues
    {
        CombatingSkill UsedSkill { get; }
       

    }
}
