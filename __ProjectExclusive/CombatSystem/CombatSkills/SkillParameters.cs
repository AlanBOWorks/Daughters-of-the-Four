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
            PreComputedOffensiveTargets = new HashSet<CombatingEntity>();
            PreComputedSupportTargets = new HashSet<CombatingEntity>();
        } //this is just for seeing the references amount

        public SkillValuesHolders(CombatingEntity target) : this()
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

        /// <summary>
        /// The virtual amount of targets to handle (it's not the real targets handle by the skill). <br></br>
        /// This was made primarily for [<seealso cref="CombatSystem.Animator"/>]
        /// </summary>
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public readonly HashSet<CombatingEntity> PreComputedOffensiveTargets;
        /// <summary>
        /// <inheritdoc cref="PreComputedOffensiveTargets"/>
        /// </summary>
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public readonly HashSet<CombatingEntity> PreComputedSupportTargets;

        public void SwitchTarget(CombatingEntity newTarget) => Target = newTarget;
        public void Inject(CombatingEntity performer) => Performer = performer;
        public void Inject(SkillUsageValues values)
        {
            UsedSkill = values.UsedSkill;
            Target = values.Target;
            PreComputeTargets();
        }

        /* Problem: animator can't know the whole set of targets (if is just an individual, a group or all entities).
         * Solution: pre-compute the possibles targets in a Collection and use it.
         * Note: since the skills will not hold more than 10 effects at once, this operation should not be that expensive
         */
        private void PreComputeTargets()
        {
            var skill = UsedSkill;
            bool isOffensive = skill.GetTargetType() == EnumSkills.TargetType.Offensive;
            if(isOffensive)
                InjectTargetsAsOffensive();
            else
                InjectTargetsAsSupport();
        }

        private void InjectTargetsAsOffensive()
        {
            foreach (var effectParameter in UsedSkill.GetEffects())
            {
                var targets = UtilsTarget.GetPossibleTargets(effectParameter, Performer, Target);
                foreach (var target in targets)
                {
                    InjectTarget(target);
                }
            }
        }

        private void InjectTargetsAsSupport()
        {
            foreach (var effectParameter in UsedSkill.GetEffects())
            {
                var targets = UtilsTarget.GetPossibleTargets(effectParameter, Performer, Target);
                foreach (var target in targets)
                {
                    InjectTarget(PreComputedSupportTargets,target);
                }
            }
        }

        private void InjectTarget(CombatingEntity target)
        {
            bool isAlly = Performer.Team.Contains(target);
            var collection = (isAlly)
                ? PreComputedSupportTargets
                : PreComputedOffensiveTargets;
            InjectTarget(collection,target);
        }

        private static void InjectTarget(ISet<CombatingEntity> collection, CombatingEntity target)
        {
            if (collection.Contains(target)) return;
            collection.Add(target);
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
            PreComputedOffensiveTargets.Clear();
            PreComputedSupportTargets.Clear();
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
       

    }
}
