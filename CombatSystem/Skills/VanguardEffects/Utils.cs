using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Skills.VanguardEffects
{
    internal static class UtilsVanguardEffects
    {
      

        public static T GetElement<T>(EnumsVanguardEffects.VanguardEffectType type,  IVanguardEffectStructureRead<T> structure)
        {
            return type switch
            {
                EnumsVanguardEffects.VanguardEffectType.Counter => structure.VanguardCounterType,
                EnumsVanguardEffects.VanguardEffectType.Punish => structure.VanguardPunishType,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }



        public static IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable<TKey, TValue>(
            IVanguardEffectStructureRead<TKey> keyStructure,
            IVanguardEffectStructureRead<TValue> valueStructure)
        {
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardCounterType,valueStructure.VanguardCounterType);
            yield return new KeyValuePair<TKey, TValue>(keyStructure.VanguardPunishType,valueStructure.VanguardPunishType);
        }
    }
}
