using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem.CombatSkills;
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
        public SkillValuesHolders()
        {
            
        } //this is just for seeing the references amount

        
        [ShowInInspector]
        public CombatingEntity Performer { get; private set; }
        [ShowInInspector]
        public CombatingEntity Target { get; private set; }
        [ShowInInspector]
        public bool IsCritical { get; private set; }

        [ShowInInspector]
        public CombatingSkill UsedSkill { get; private set; }

        public ICollection<CombatingEntity> EffectTargets { get; private set; }

       

        public void SwitchTarget(CombatingEntity newTarget) => Target = newTarget;
        public void Inject(CombatingEntity performer) => Performer = performer;
        public void Inject(SkillUsageValues values)
        {
            if(Performer == null)
                throw new NullReferenceException($"Performer is null for [{typeof(SkillValuesHolders)}]");

            UsedSkill = values.UsedSkill;
            Target = values.Target;
            EffectTargets = UtilsTarget.GetPossibleTargets(UsedSkill.GetEffectTargetType(), Performer, Target);
        }

        


        public void RollForCritical(CombatingEntity performer)
        {
            IsCritical = UtilsRandomStats.IsCritical(performer.CombatStats, UsedSkill);
        }
        public void RollForCritical() => RollForCritical(Performer);

        /// <summary>
        /// Rolls for critical only using the raw values of the Stats
        /// (<see cref="RollForCritical()"/> used the <see cref="UsedSkill"/>)
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
            Performer = null;
            OnActionClear();
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
        ICollection<CombatingEntity> EffectTargets { get; }

    }
}
