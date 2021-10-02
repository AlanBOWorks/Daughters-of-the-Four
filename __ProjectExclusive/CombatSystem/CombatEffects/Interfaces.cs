using CombatEntity;
using CombatSkills;

namespace CombatEffects
{
    public interface IEffect
    {
        void DoEffect(SkillValuesHolders values, float effectModifier);
    }
}