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
        void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue);

    }

    public interface IOffensiveEffect : IEffect { }
    public interface ISupportEffect : IEffect { }
    public interface ITeamEffect : IEffect { }


    public interface IEffectBasicInfo
    {
        string EffectTag { get; }
        string EffectSmallPrefix { get; }
        EnumsEffect.ConcreteType EffectType { get; }
        Sprite GetIcon();
        GameObject GetSecondaryParticlesPrefab();
    }

}
