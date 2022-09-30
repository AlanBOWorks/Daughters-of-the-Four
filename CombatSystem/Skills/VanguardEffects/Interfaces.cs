using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Skills.VanguardEffects
{
    public interface IVanguardEffectStructureRead<out T>
    {
        T VanguardCounterType { get; }
        T VanguardPunishType { get; }
    }


    public interface IVanguardEffectUsageListener : ICombatEventListener
    {
        void OnVanguardSkillSubscribe(IVanguardSkill skill, CombatEntity performer);
        void OnVanguardEffectsPerform(CombatEntity attacker, CombatEntity onTarget);
        void OnVanguardEffectPerform(EnumsEffect.TargetType targetType, VanguardEffectUsageValues values);
    }
}
