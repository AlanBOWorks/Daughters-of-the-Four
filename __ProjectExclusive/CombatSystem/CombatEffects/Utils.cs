using System;
using CombatTeam;
using UnityEngine;

namespace CombatEffects
{
    public static class EnumEffects
    {
        public enum TargetType
        {
            Self,
            SelfAllies,
            SelfTeam,
            Target,
            TargetAllies,
            TargetTeam,
            All
        }
    }

    public static class UtilsEffects
    {
        public static T GetElement<T>(EnumEffects.TargetType targetType, IFullTargetingStructureRead<T> structure)
        {
            return targetType switch
            {
                EnumEffects.TargetType.Self => structure.SelfElement,
                EnumEffects.TargetType.SelfAllies => structure.SelfAlliesElement,
                EnumEffects.TargetType.SelfTeam => structure.SelfTeamElement,
                EnumEffects.TargetType.Target => structure.TargetElement,
                EnumEffects.TargetType.TargetAllies => structure.TargetAlliesElement,
                EnumEffects.TargetType.TargetTeam => structure.TargetTeamElement,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
            };
        }

        public static T GetElement<T>(EnumEffects.TargetType targetType,
            ITeamTargetingStructureRead<T> selfStructure, ITeamTargetingStructureRead<T> targetStructure)
        {
            return targetType switch
            {
                EnumEffects.TargetType.Self => selfStructure.SelfAlliesElement,
                EnumEffects.TargetType.SelfAllies => selfStructure.SelfAlliesElement,
                EnumEffects.TargetType.SelfTeam => selfStructure.SelfTeamElement,
                EnumEffects.TargetType.Target => targetStructure.SelfElement,
                EnumEffects.TargetType.TargetAllies => targetStructure.SelfAlliesElement,
                EnumEffects.TargetType.TargetTeam => targetStructure.SelfTeamElement,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
            };
        }
    }
}
