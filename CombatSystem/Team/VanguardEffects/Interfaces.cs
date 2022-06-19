using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team.VanguardEffects
{
    public interface IVanguardEffectsStructuresRead<out T> : IVanguardEffectStructureBaseRead<T>
    {
        T VanguardDelayImproveType { get; }
    }

    public interface IVanguardEffectStructureBaseRead<out T>
    {
        T VanguardRevengeType { get; }
        T VanguardPunishType { get; }
    }


    public interface IVanguardEffectUsageListener : ICombatEventListener
    {
        void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker);
        void OnVanguardRevengeEffectPerform(IVanguardSkill skill, int iterations);
        void OnVanguardPunishEffectPerform(IVanguardSkill skill, int iterations);
    }
}
