using CombatEntity;
using CombatSkills;

namespace CombatEffects
{
    public interface IEffect
    {
        void DoEffect(SkillValuesHolders values, EnumEffects.TargetType effectTargetType, float effectModifier, bool isEffectCrit);
    }
}
