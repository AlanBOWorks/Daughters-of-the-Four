using CombatSystem.Entity;
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

    public interface IEffectTypeStructureRead<out T>
    {
        T OffensiveEffectType { get; }
        T SupportEffectType { get; }
        T TeamEffectType { get; }
    }

    public interface IEffectBasicInfo
    {
        string EffectTag { get; }
    }

}
