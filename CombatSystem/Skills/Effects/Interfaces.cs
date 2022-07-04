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
        void DoEffect(EntityPairInteraction entities, float effectValue);

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
