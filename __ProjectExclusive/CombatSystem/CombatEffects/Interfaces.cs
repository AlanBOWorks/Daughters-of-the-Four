using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    /// <summary>
    /// Mainly to keep track of the key reference for events [<seealso cref="CombatSystem.Events.SystemEventsHolder"/>]
    /// </summary>
    public interface ISkillComponent
    {
        EnumSkills.SkillInteractionType GetComponentType();
        Color GetDescriptiveColor();
        string GetEffectValueText(float effectValue);
    }

    public interface IEffect : ISkillComponent
    {
        //SkillValuesHolders is a reference used for calculations; for events is better use CombatEntityPairAction
        void DoEffect(ISkillParameters parameters, float effectModifier, bool isEffectCrit);
    }

    public interface IBuff : ISkillComponent
    {

        void DoBuff(ISkillParameters parameters,
            EnumStats.BuffType buffType,
            float effectValue, bool isCritical);
    }
}
