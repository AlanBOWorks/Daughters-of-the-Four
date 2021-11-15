using System;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class TranslatorTeam
    {
        private const string ProvisionalAttackingStance = "Attacking";
        private const string ProvisionalNeutralStance = "Neutral";
        private const string ProvisionalDefendingStance = "Defending";
        public static string GetText(EnumTeam.TeamStance stance)
        {
            switch (stance)
            {
                case EnumTeam.TeamStance.Neutral:
                    return ProvisionalNeutralStance;
                case EnumTeam.TeamStance.Attacking:
                    return ProvisionalAttackingStance;
                case EnumTeam.TeamStance.Defending:
                    return ProvisionalDefendingStance;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stance), stance, null);
            }
        }
    }
}
