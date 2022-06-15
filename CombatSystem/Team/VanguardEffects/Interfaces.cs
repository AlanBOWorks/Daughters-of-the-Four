using CombatSystem._Core;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Team.VanguardEffects
{
    public interface IVanguardSkill
    {

    }

    public interface IVanguardEffectsStructuresRead<out T> : IVanguardEffectStructureBaseRead<T>
    {
        T VanguardDelayImproveType { get; }
    }

    public interface IVanguardEffectStructureBaseRead<out T>
    {
        T VanguardRevengeType { get; }
        T VanguardPunishType { get; }
    }


    public interface IVanguardEffectTriggerListener : ICombatEventListener
    {
    }
}
