using CombatSystem.Entity;

namespace CombatSystem.Skills
{

    public struct PerformEffectValues
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

    public struct SkillUsageValues
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
    }
}
