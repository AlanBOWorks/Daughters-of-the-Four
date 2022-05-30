using UnityEngine;

namespace Localization.Combat
{
    public static class CombatLocalizations 
    {
        public static string LocalizeEffectName(in string effectTag)
        {
            return effectTag;
        }

        public static string LocalizeStance(in string stanceString)
        {
            return stanceString;
        }
    }
}
