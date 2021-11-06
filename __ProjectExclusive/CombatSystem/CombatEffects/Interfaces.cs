using CombatEntity;
using CombatSkills;
using Stats;

namespace CombatEffects
{
    /// <summary>
    /// Mainly to keep track of the key reference for events [<seealso cref="CombatSystem.Events.SystemEventsHolder"/>]
    /// </summary>
    public interface ISkillComponent
    {
        EnumSkills.SkillInteractionType GetComponentType();
    }

    public interface IEffect : ISkillComponent
    {
        //SkillValuesHolders is a reference used for calculations; for events is better use CombatEntityPairAction
        void DoEffect(CombatEntityPairAction entities, EnumEffects.TargetType effectTargetType, float effectModifier, bool isEffectCrit);
    }

    public interface IBuff : ISkillComponent
    {

        void DoBuff(CombatEntityPairAction entities, 
            EnumStats.BuffType buffType, EnumEffects.TargetType effectTargetType,
            float effectValue, bool isCritical);
    }
}
