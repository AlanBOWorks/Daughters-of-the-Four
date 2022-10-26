using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Localization
{
    public static class LocalizationsCombat 
    {
        public static string LocalizeSkillName(string skillTag)
        {
            return skillTag;
        }

        public static string LocalizeEffectTag(string effectTag)
        {
            return effectTag;
        }

        public static string LocalizeStance(string stanceString)
        {
            return stanceString;
        }

        public static string LocalizeStance(EnumTeam.Stance stance)
        {
            return LocalizeStance(stance.ToString());
        }

        public static string LocalizeLuck(float luckAmount)
        {
            string luckAmountText = luckAmount.ToString("P0");
            return luckAmountText;
        }

    }
}
