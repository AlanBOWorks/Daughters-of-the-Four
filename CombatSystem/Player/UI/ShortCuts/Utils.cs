using System.Collections.Generic;

namespace CombatSystem.Player.UI
{
    public static class UtilsShortcuts
    {
        public static IEnumerable<T> GetEnumerable<T>(ISwitchStanceShortcutCommandStructureRead<T> structure)
        {
            yield return structure.SupportStanceShortCutElement;
            yield return structure.AttackStanceShortCutElement;
            yield return structure.DefendStanceShortCutElement;
        }
        public static IEnumerable<KeyValuePair<TKey,TValue>> GetEnumerable<TKey,TValue>(
            ISwitchStanceShortcutCommandStructureRead<TKey> keys,
            ISwitchStanceShortcutCommandStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.SupportStanceShortCutElement,values.SupportStanceShortCutElement);
            yield return new KeyValuePair<TKey, TValue>(keys.AttackStanceShortCutElement,values.AttackStanceShortCutElement);
            yield return new KeyValuePair<TKey, TValue>(keys.DefendStanceShortCutElement,values.DefendStanceShortCutElement);
        }


        public static IEnumerator<T> GetEnumerator<T>(ISwitchStanceShortcutCommandStructureRead<T> structure)
        {
            yield return structure.SupportStanceShortCutElement;
            yield return structure.AttackStanceShortCutElement;
            yield return structure.DefendStanceShortCutElement;
        }
        public static IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator<TKey, TValue>(
            ISwitchStanceShortcutCommandStructureRead<TKey> keys,
            ISwitchStanceShortcutCommandStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.SupportStanceShortCutElement, values.SupportStanceShortCutElement);
            yield return new KeyValuePair<TKey, TValue>(keys.AttackStanceShortCutElement, values.AttackStanceShortCutElement);
            yield return new KeyValuePair<TKey, TValue>(keys.DefendStanceShortCutElement, values.DefendStanceShortCutElement);
        }
    }


    public static class EnumShortCuts
    {
        public const int SupportStanceIndex = 0;
        public const int AttackerStanceIndex = SupportStanceIndex + 1;
        // Defense is third because it makes more sense in the keyboard being vanguard in the front most position
        public const int DefendStanceIndex = AttackerStanceIndex + 1;
        public const int StanceShortcutsCount = DefendStanceIndex + 1;

        public const int SkillShortcutsCount = 7;
        public const int SwitchEntityShortcutsCount = 2;
    }
}

