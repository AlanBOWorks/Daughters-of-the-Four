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
        /// <param name="effectValue">The effect value could be modify by passives and reactive effects (thus the ref)</param>
        void DoEffect(EntityPairInteraction entities, ref float effectValue);

        string GetEffectTooltip(CombatStats performerStats, float effectValue);

    }

    public interface IOffensiveEffect : IEffect { }
    public interface ISupportEffect : IEffect { }
    public interface ITeamEffect : IEffect { }

    public interface IDeBuffEffect : IOffensiveEffect
    {
        bool IsBurstEffect();
    }

    public interface IBuffEffect : ISupportEffect
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
