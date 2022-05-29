using CombatSystem.Entity;
using CombatSystem.Skills.Effects;

namespace CombatSystem.Skills
{
    /// <summary>
    /// Contains necessary information for performing <see cref="IEffect"/>] (ReadOnly)
    /// </summary>
    public readonly struct PerformEffectValues
    {
        public PerformEffectValues(in IEffect effect, in float effectValue, in EnumsEffect.TargetType targetType)
        {
            Effect = effect;
            EffectValue = effectValue;
            TargetType = targetType;
        }

        public readonly IEffect Effect;
        public readonly float EffectValue;
        public readonly EnumsEffect.TargetType TargetType;
    }

    /// <summary>
    /// Contains important information of the [<see cref="CombatSkill"/>] usage. (ReadOnly)
    /// </summary>
    public readonly struct SkillUsageValues
    {
        public readonly CombatEntity Performer;
        public readonly CombatEntity Target;
        public readonly CombatSkill UsedSkill;

        public SkillUsageValues(CombatEntity performer, CombatEntity target, CombatSkill usedSkill)
        {
            Performer = performer;
            Target = target;
            UsedSkill = usedSkill;
        }

        public void Extract(out CombatEntity performer, out CombatEntity target, out CombatSkill usedSkill)
        {
            performer = Performer;
            target = Target;
            usedSkill = UsedSkill;
        }
    }
}
