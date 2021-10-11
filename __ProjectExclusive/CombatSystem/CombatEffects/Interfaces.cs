using CombatEntity;
using CombatSkills;

namespace CombatEffects
{
    /// <summary>
    /// Mainly to keep track of the key reference for events [<seealso cref="CombatSystem.Events.SystemEventsHolder"/>]
    /// </summary>
    public interface ISkillComponent { }

    public interface IEffect : ISkillComponent
    {
        void DoEffect(SkillValuesHolders values, EnumEffects.TargetType effectTargetType, float effectModifier, bool isEffectCrit);
    }

    public interface IBuff : ISkillComponent
    {
        void DoBuff(SkillValuesHolders values, EnumEffects.TargetType effectTargetType, float effectValue,
            bool isCritical);
    }
}
