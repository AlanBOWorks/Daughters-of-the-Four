using System;
using _CombatSystem;
using _Team;
using Skills;
using UnityEngine;

namespace Characters
{

    public static class EnumCharacter
    {
        /// <summary>
        /// Is the more specific/encapsulated type of [<see cref="EnumTeam.GroupPositioning"/>] for more
        /// clear usages
        /// </summary>
        public enum RoleArchetype
        {
            Vanguard = EnumTeam.GroupPositioning.FrontLine,
            Attacker = EnumTeam.GroupPositioning.MidLine,
            Support = EnumTeam.GroupPositioning.BackLine
        }

        /// <summary>
        /// Specify where in the 'field'/combat is the entity (used for area/target calculation)
        /// </summary>
        public enum FieldPosition
        {
            InTeam,
            InEnemyTeam,
            OutFormation
        }

        public enum RangeType
        {
            /// <summary>
            /// Can only target closeRange foes
            /// </summary>
            Melee,
            /// <summary>
            /// Can only target ranged foes
            /// </summary>
            Range,
            /// <summary>
            /// <inheritdoc cref="HybridRange"/>
            /// </summary>
            HybridMelee,
            /// <summary>
            /// Is the combination of <see cref="Melee"/> and <see cref="Range"/>
            /// </summary>
            HybridRange
        }
    }

    public static class UtilsCharacter
    {
        public const int PredictedAmountOfCharactersInBattle = UtilsCharacterArchetypes.AmountOfArchetypesAmount * 2;

        public static bool IsAPlayerEntity(CombatingEntity entity)
        {
            CombatingTeam playerCharacters = CombatSystemSingleton.Characters.PlayerFaction;
            return playerCharacters.Contains(entity);
        }

        public static T GetElement<T>(ICharacterArchetypesData<T> elements, EnumCharacter.RoleArchetype archetype)
        {
            return archetype switch
            {
                EnumCharacter.RoleArchetype.Vanguard => elements.Vanguard,
                EnumCharacter.RoleArchetype.Attacker => elements.Attacker,
                EnumCharacter.RoleArchetype.Support => elements.Support,
                _ => throw new ArgumentOutOfRangeException(nameof(archetype), archetype, null)
            };
        }

        public static T GetElement<T>(ICharacterRangesData<T> elements, EnumCharacter.RangeType rangeType)
        {
            switch (rangeType)
            {
                case EnumCharacter.RangeType.Melee:
                case EnumCharacter.RangeType.HybridMelee:
                    return elements.MeleeRange;
                case EnumCharacter.RangeType.Range:
                case EnumCharacter.RangeType.HybridRange:
                    return elements.RangedRange;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rangeType), rangeType, null);
            }
        }
    }


    public static class UtilsArea
    {
        public static void InvokeAreaChange(CombatingEntity entity)
            => CombatSystemSingleton.CombatEventsInvoker.InvokeTemporalStatChange(entity);
        

        public static EnumTeam.Stances ParseStance(float stanceEquivalent)
        {
            EnumTeam.Stances stance;
            if (stanceEquivalent == 0) stance = EnumTeam.Stances.Neutral;
            else if (stanceEquivalent > 0) stance = EnumTeam.Stances.Attacking;
            else stance = EnumTeam.Stances.Defending;

            return stance;
        }

        public static void ToggleStance(CombatingEntity entity, EnumTeam.Stances targetStance)
        {
            var areaData = entity.AreasDataTracker;
            if (areaData.IsForceStance)
            {
                areaData.ForceStateFinish();
            }
            else
            {
                areaData.ForceState(targetStance);
            }

            InvokeAreaChange(entity);
        }

        public static void TeamToggleStance(CombatingEntity teamHolder, EnumTeam.Stances targetStance)
        {
            var team = teamHolder.CharacterGroup.Team;
            var teamData = team.control;
            if (teamData.IsForcedStance)
            {
                teamData.FinishForceStance();
            }
            else
            {
                teamData.DoForceStance(targetStance);
            }

            CombatSystemSingleton.CombatEventsInvoker.InvokeTemporalStatChange(teamHolder);

        }

        public static void TogglePosition(CombatingEntity entity, EnumCharacter.FieldPosition targetPosition)
        {
            var areaTracker = entity.AreasDataTracker;
            var currentPosition = areaTracker.CombatFieldPosition;
            var finalPosition 
                = currentPosition != EnumCharacter.FieldPosition.InTeam 
                ? EnumCharacter.FieldPosition.InTeam 
                : targetPosition;


            areaTracker.CombatFieldPosition = finalPosition;

            InvokeAreaChange(entity);
        }
    }
}
