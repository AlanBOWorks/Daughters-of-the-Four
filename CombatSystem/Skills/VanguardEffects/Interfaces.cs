using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Skills.VanguardEffects
{
    public interface IVanguardEffectStructureRead<out T>
    {
        T VanguardRevengeType { get; }
        T VanguardPunishType { get; }
    }


    public interface IVanguardEffectUsageListener : ICombatEventListener
    {
        void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values);
        /// <summary>
        /// Increment done after the enemy attacks and there's at least one vanguard effect in the target type.
        /// </summary>
        void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker);
        void OnVanguardEffectPerform(VanguardSkillUsageValues values);
    }
}
