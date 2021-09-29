using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffect : ScriptableObject, IEffect
    {
        public abstract void DoEffect(SkillValuesHolders values, float effectModifier);
    }
}
