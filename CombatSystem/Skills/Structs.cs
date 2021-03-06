using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using Sirenix.OdinInspector;

namespace CombatSystem.Skills
{
    /// <summary>
    /// Contains necessary information for performing <see cref="IEffect"/>] (ReadOnly)
    /// </summary>
    public readonly struct PerformEffectValues
    {
        public PerformEffectValues(IEffect effect, float effectValue, EnumsEffect.TargetType targetType)
        {
            Effect = effect;
            EffectValue = effectValue;
            TargetType = targetType;
        }

        public readonly IEffect Effect;
        public readonly float EffectValue;
        public readonly EnumsEffect.TargetType TargetType;


        public static PerformEffectValues operator ++(PerformEffectValues a)
        {
            float effectValue = a.EffectValue;
            return new PerformEffectValues(a.Effect,effectValue *2, a.TargetType);
        }
        public static PerformEffectValues operator *(PerformEffectValues a, float modifier)
        {
            return new PerformEffectValues(a.Effect, a.EffectValue * modifier, a.TargetType);
        }
    }

    public readonly struct SubmitEffectValues
    {
        [ShowInInspector]
        public readonly IEffect Effect;
        [ShowInInspector]
        public readonly float EffectValue;

        public SubmitEffectValues(IEffect effect, float effectValue)
        {
            Effect = effect;
            EffectValue = effectValue;
        }

        public SubmitEffectValues(PerformEffectValues values) : this(values.Effect,values.EffectValue)
        { }

    }


    /// <summary>
    /// Contains important information of the [<see cref="ICombatSkill"/>] usage. (ReadOnly)
    /// </summary>
    public readonly struct SkillUsageValues
    {
        public readonly CombatEntity Performer;
        public readonly CombatEntity Target;
        public readonly ICombatSkill UsedSkill;

        public SkillUsageValues(CombatEntity performer, CombatEntity target, ICombatSkill usedSkill)
        {
            Performer = performer;
            Target = target;
            UsedSkill = usedSkill;
        }

        public void Extract(out CombatEntity performer, out CombatEntity target, out ICombatSkill usedSkill)
        {
            Extract(out performer, out target);
            usedSkill = UsedSkill;
        }

        public void Extract(out CombatEntity performer, out CombatEntity target)
        {
            performer = Performer;
            target = Target;
        }
    }

    public readonly struct VanguardSkillUsageValues
    {
        public readonly VanguardEffectsHolder EffectsHolder;
        public readonly IVanguardSkill UsedSkill;
        public readonly int Accumulation;

        public VanguardSkillUsageValues(
            VanguardEffectsHolder effectsHolder, 
            IVanguardSkill usedSkill, 
            int accumulation)
        {
            EffectsHolder = effectsHolder;
            UsedSkill = usedSkill;
            Accumulation = accumulation;
        }
    }
}
