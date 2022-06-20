using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team.VanguardEffects
{
    internal static class UtilsVanguardEffects
    {
      

        public static T GetElement<T>(EnumsVanguardEffects.VanguardEffectType type,  IVanguardEffectsStructureRead<T> structure)
        {
            return type switch
            {
                EnumsVanguardEffects.VanguardEffectType.DelayImprove => structure.VanguardDelayImproveType,
                EnumsVanguardEffects.VanguardEffectType.Revenge => structure.VanguardRevengeType,
                EnumsVanguardEffects.VanguardEffectType.Punish => structure.VanguardPunishType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable<TKey, TValue>(
            IVanguardEffectStructureBaseRead<TKey> keyStructure,
            IVanguardEffectStructureBaseRead<TValue> valueStructure)
        {
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardRevengeType,valueStructure.VanguardRevengeType);
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardPunishType,valueStructure.VanguardPunishType);
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable<TKey, TValue>(
            IVanguardEffectsStructureRead<TKey> keyStructure,
            IVanguardEffectsStructureRead<TValue> valueStructure)
        {
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardDelayImproveType, valueStructure.VanguardDelayImproveType);
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardRevengeType, valueStructure.VanguardRevengeType);
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardPunishType, valueStructure.VanguardPunishType);
        }
    }
}
