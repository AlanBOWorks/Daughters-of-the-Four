using System;
using CombatSystem.Entity;

namespace CombatSystem.Team.VanguardEffects
{
    internal static class UtilsVanguardEffects
    {
        public static T GetElement<T>(EnumsVanguardEffects.VanguardEffectType type,  IVanguardEffectsStructuresRead<T> structure)
        {
            return type switch
            {
                EnumsVanguardEffects.VanguardEffectType.DelayImprove => structure.VanguardDelayImproveType,
                EnumsVanguardEffects.VanguardEffectType.Revenge => structure.VanguardRevengeType,
                EnumsVanguardEffects.VanguardEffectType.Punish => structure.VanguardPunishType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }


        public static void HandleVanguardOffensive(CombatEntity attacker, CombatEntity onTarget)
        {
            var targetTeam = onTarget.Team;
            if(targetTeam.Contains(attacker)) return;
            
            targetTeam.VanguardEffectsHolder.OnOffensiveDone(attacker,onTarget);
        }
    }
}
