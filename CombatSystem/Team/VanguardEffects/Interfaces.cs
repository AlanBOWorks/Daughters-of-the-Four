using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team.VanguardEffects
{
    public interface IVanguardEffectsStructureRead<out T> : IVanguardEffectStructureBaseRead<T>
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
        void OnVanguardEffectPerform(VanguardSkillUsageValues values);
    }
}
