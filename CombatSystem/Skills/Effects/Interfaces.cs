using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{

    public interface IEffectPreset
    {
        IEffect GetPreset();
        EnumsEffect.TargetType TargetType { get; }
        float GetValue();
    }

    public interface IEffect : IEffectBasicInfo
    {
        /// <param name="entities"></param>
        /// <param name="effectValue">The effect value could be modify by passives and reactive effects (thus the ref)</param>
        /// <param name="luckModifier"></param>
        void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier);

        float CalculateEffectTooltipValue(CombatStats performerStats, float effectValue);
        bool IsPercentSuffix();
        bool IsPercentTooltip();

    }

    public interface IOffensiveEffect : IEffect { }
    public interface ISupportEffect : IEffect { }
    public interface ITeamEffect : IEffect { }

    public interface IDeBuffEffect 
    {
        bool IsBurstEffect();
    }

    public interface IBuffEffect
    {
        bool IsBurstEffect();
    }


    public interface IEffectBasicInfo
    {
        string EffectTag { get; }
        string EffectSmallPrefix { get; }
        EnumsEffect.ConcreteType EffectType { get; }
        Sprite GetIcon();
        GameObject GetSecondaryParticlesPrefab();
    }

}
