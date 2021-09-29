using System;
using UnityEngine;

namespace CombatEffects
{
    public static class EnumEffects
    {
        public enum TargetType
        {
            Self,
            SelfExclude,
            SelfTeam,
            Target,
            TargetExclude,
            TargetTeam,
            All
        }
    }
}
