using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Team
{
    internal static class UtilsTeam 
    {

        public static bool IsAllyEntity(in CombatEntity entity, in CombatEntity control)
        {
            var entityTeam = entity.Team;
            return entityTeam.Contains(control);
        }

        public static bool IsAllyEntity(in CombatEntity entity, in CombatTeam inTeam)
        {
            return inTeam.Contains(entity);
        }

        public static bool IsPlayerTeam(in CombatTeam team)
        {
            return team == CombatSystemSingleton.PlayerTeam;
        }

        public static bool IsPlayerTeam(in CombatEntity entity)
        {
            return IsPlayerTeam(entity.Team);
        }

        public static bool IsMainRole(in CombatEntity entity, in CombatTeam team)
        {
            return team.IsMainRole(in entity);

        }
        public static bool IsMainRole(in CombatEntity entity)
        {
            var entityTeam = entity.Team;
            return IsMainRole(in entity, in entityTeam);
        }

        public static bool IsTrinityRole(in CombatEntity entity, in CombatTeam team)
        {
            return team.IsTrinityRole(in entity);
        }
        public static bool IsTrinityRole(in CombatEntity entity)
        {
            var entityTeam = entity.Team;
            return IsTrinityRole(in entity, in entityTeam);
        }


        public static int GetRoleIndex(ICombatEntityProvider entity)
        {
            var entityRole = entity.GetAreaData().RoleType;
            return (int) entityRole;
        }

        public static bool IsTeamInStancePosition(in EnumTeam.Positioning targetPositioning, in EnumTeam.StanceFull stance)
        {
            switch (stance)
            {
                case EnumTeam.StanceFull.Neutral:
                    return targetPositioning == EnumTeam.Positioning.BackLine;
                case EnumTeam.StanceFull.Attacking:
                    return targetPositioning == EnumTeam.Positioning.MidLine;
                case EnumTeam.StanceFull.Defending:
                    return targetPositioning == EnumTeam.Positioning.FrontLine;
                default:
                    return false;
            }
        }


        public static T GetElement<T>(CombatEntity member, IOppositionTeamStructureRead<T> structure)
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            return playerTeam.Contains(member)
                ? structure.PlayerTeamType
                : structure.EnemyTeamType;
        }

        public static T GetElement<T>(EnumTeam.Positioning positioning, ITeamFlexPositionStructureRead<T> structure)
        {
            return positioning switch
            {
                EnumTeam.Positioning.FrontLine => structure.FrontLineType,
                EnumTeam.Positioning.MidLine => structure.MidLineType,
                EnumTeam.Positioning.BackLine => structure.BackLineType,
                EnumTeam.Positioning.FlexLine => structure.FlexLineType,
                _ => throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null)
            };
        }

        public static TeamAreaData GenerateAreaData(EnumTeam.Role fromRole)
        {
            EnumTeam.Positioning positioning = GetEquivalent(fromRole);

            return new TeamAreaData(fromRole, positioning);
        }

        public static EnumTeam.Positioning GetEquivalent(EnumTeam.Role fromRole)
        {
            return fromRole switch
            {
                EnumTeam.Role.Vanguard => EnumTeam.Positioning.FrontLine,
                EnumTeam.Role.Attacker => EnumTeam.Positioning.MidLine,
                EnumTeam.Role.Support => EnumTeam.Positioning.BackLine,
                EnumTeam.Role.Flex => EnumTeam.Positioning.FlexLine,
                _ => throw new ArgumentOutOfRangeException(nameof(fromRole), fromRole, null)
            };
        }

        public static T GetElement<T>(EnumTeam.Role role, ITeamFlexRoleStructureRead<T> structure)
        {
            return role switch
            {
                EnumTeam.Role.Vanguard => structure.VanguardType,
                EnumTeam.Role.Attacker => structure.AttackerType,
                EnumTeam.Role.Support => structure.SupportType,
                EnumTeam.Role.Flex => structure.FlexType,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }

        public static T GetElement<T>(EnumTeam.Stance stance, IStanceStructureRead<T> structure)
        {
            return stance switch
            {
                EnumTeam.Stance.Neutral => structure.NeutralStance,
                EnumTeam.Stance.Attacking => structure.AttackingStance,
                EnumTeam.Stance.Defending => structure.DefendingStance,
                _ => throw new ArgumentOutOfRangeException(nameof(stance), stance, null)
            };
        }

        public static T GetElement<T>(EnumTeam.StanceFull stance, IFullStanceStructureRead<T> structure)
        {
            return stance switch
            {
                EnumTeam.StanceFull.Neutral => structure.NeutralStance,
                EnumTeam.StanceFull.Attacking => structure.AttackingStance,
                EnumTeam.StanceFull.Defending => structure.DefendingStance,
                _ => structure.DisruptionStance
            };
        }
        
        public static EnumTeam.StanceFull ParseStance(EnumTeam.Stance basicStance)
        {
            switch (basicStance)
            {
                case EnumTeam.Stance.Neutral:
                    return EnumTeam.StanceFull.Neutral;
                case EnumTeam.Stance.Attacking:
                    return EnumTeam.StanceFull.Attacking;
                case EnumTeam.Stance.Defending:
                    return EnumTeam.StanceFull.Defending;
                default:
                    return EnumTeam.StanceFull.Disrupted;
            }
        }


        public static T GetElement<T>(EnumTeam.ActiveRole role, ITeamFullRolesStructureRead<T> structure)
        {
            switch (role)
            {
                case EnumTeam.ActiveRole.MainVanguard:
                    return structure.VanguardType;
                case EnumTeam.ActiveRole.MainAttacker:
                    return structure.AttackerType;
                case EnumTeam.ActiveRole.MainSupport:
                    return structure.SupportType;
                case EnumTeam.ActiveRole.MainFlex:
                    return structure.FlexType;
                case EnumTeam.ActiveRole.SecondaryVanguard:
                    return structure.SecondaryVanguardElement;
                case EnumTeam.ActiveRole.SecondaryAttacker:
                    return structure.SecondaryVanguardElement;
                case EnumTeam.ActiveRole.SecondarySupport:
                    return structure.SecondarySupportElement;
                case EnumTeam.ActiveRole.SecondaryFlex:
                    return structure.SecondaryFlexElement;

                case EnumTeam.ActiveRole.ThirdVanguard:
                    return structure.ThirdVanguardElement;
                case EnumTeam.ActiveRole.ThirdAttacker:
                    return structure.ThirdAttackerElement;
                case EnumTeam.ActiveRole.ThirdSupport:
                    return structure.ThirdSupportElement;
                case EnumTeam.ActiveRole.ThirdFlex:
                    return structure.ThirdFlexElement;
                
                case EnumTeam.ActiveRole.InvalidRole:
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        public static T GetElement<T>(int index, ITeamFullRolesStructureRead<T> structure)
        {
            return index switch
            {
                EnumTeam.VanguardIndex => structure.VanguardType,
                EnumTeam.AttackerIndex => structure.AttackerType,
                EnumTeam.SupportIndex => structure.SupportType,
                EnumTeam.FlexIndex => structure.FlexType,
                EnumTeam.SecondaryVanguardIndex => structure.SecondaryVanguardElement,
                EnumTeam.SecondaryAttackerIndex => structure.SecondaryAttackerElement,
                EnumTeam.SecondarySupportIndex => structure.SecondarySupportElement,
                EnumTeam.SecondaryFlexIndex => structure.SecondaryFlexElement,
                EnumTeam.ThirdVanguardIndex => structure.ThirdVanguardElement,
                EnumTeam.ThirdAttackerIndex => structure.ThirdAttackerElement,
                EnumTeam.ThirdSupportIndex => structure.ThirdSupportElement,
                EnumTeam.ThirdFlexIndex => structure.ThirdFlexElement,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        public static T SafeGetElement<T>(int index, ITeamFullRolesStructureRead<T> structure)
        {
            return index switch
            {
                EnumTeam.VanguardIndex => structure.VanguardType,
                EnumTeam.AttackerIndex => structure.AttackerType,
                EnumTeam.SupportIndex => structure.SupportType,
                EnumTeam.FlexIndex => structure.FlexType,

                EnumTeam.SecondaryVanguardIndex => structure.SecondaryVanguardElement ?? structure.VanguardType,
                EnumTeam.SecondaryAttackerIndex => structure.SecondaryAttackerElement ?? structure.AttackerType,
                EnumTeam.SecondarySupportIndex => structure.SecondarySupportElement ?? structure.SupportType,
                EnumTeam.SecondaryFlexIndex => structure.SecondaryFlexElement ?? structure.FlexType,

                EnumTeam.ThirdVanguardIndex => structure.ThirdVanguardElement ?? structure.VanguardType,
                EnumTeam.ThirdAttackerIndex => structure.ThirdAttackerElement ?? structure.AttackerType,
                EnumTeam.ThirdSupportIndex => structure.ThirdSupportElement ?? structure.SupportType,
                EnumTeam.ThirdFlexIndex => structure.ThirdFlexElement ?? structure.FlexType,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }


        public static T GetElement<T>(int index, ITeamOffStructureRead<T> structure)
        {
            var offRoleIndex = ConvertIndexIntoOffRoleIndex();
            return offRoleIndex switch
            {
                EnumTeam.SecondaryVanguardIndex => structure.SecondaryVanguardElement,
                EnumTeam.SecondaryAttackerIndex => structure.SecondaryAttackerElement,
                EnumTeam.SecondarySupportIndex => structure.SecondarySupportElement,
                EnumTeam.SecondaryFlexIndex => structure.SecondaryFlexElement,

                EnumTeam.ThirdVanguardIndex => structure.ThirdVanguardElement,
                EnumTeam.ThirdAttackerIndex => structure.ThirdAttackerElement,
                EnumTeam.ThirdSupportIndex => structure.ThirdSupportElement,
                EnumTeam.ThirdFlexIndex => structure.ThirdFlexElement,
                _ => throw new ArgumentOutOfRangeException(nameof(offRoleIndex), offRoleIndex, null)
            };

            int ConvertIndexIntoOffRoleIndex()
            {
                return index + EnumTeam.RoleTypesAmount;
            }
        }

        public static IEnumerable<T> GetEnumerable<T>(ITeamFullRolesStructureRead<T> structure)
        {
            yield return structure.VanguardType;
            yield return structure.AttackerType;
            yield return structure.SupportType;
            yield return structure.FlexType;
            yield return structure.SecondaryVanguardElement;
            yield return structure.SecondaryAttackerElement;
            yield return structure.SecondarySupportElement;
            yield return structure.SecondaryFlexElement;
            yield return structure.ThirdVanguardElement;
            yield return structure.ThirdAttackerElement;
            yield return structure.ThirdSupportElement;
            yield return structure.ThirdFlexElement;
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable<TKey, TValue>(
            ITeamFullRolesStructureRead<TKey> keys, ITeamFullRolesStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.VanguardType, values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.AttackerType, values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.SupportType, values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.FlexType, values.FlexType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryVanguardElement, values.SecondaryVanguardElement);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryAttackerElement, values.SecondaryAttackerElement);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondarySupportElement, values.SecondarySupportElement);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryFlexElement, values.SecondaryFlexElement);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdVanguardElement, values.ThirdVanguardElement);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdAttackerElement, values.ThirdAttackerElement);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdSupportElement, values.ThirdSupportElement);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdFlexElement, values.ThirdFlexElement);
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetSafeEnumerable<TKey, TValue>(
            ITeamFullRolesStructureRead<TKey> keys, ITeamFullRolesStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.VanguardType, values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.AttackerType, values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.SupportType, values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.FlexType, values.FlexType);

            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryVanguardElement, values.SecondaryVanguardElement ?? values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryAttackerElement, values.SecondaryAttackerElement ?? values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondarySupportElement, values.SecondarySupportElement ?? values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryFlexElement, values.SecondaryFlexElement ?? values.FlexType);

            yield return new KeyValuePair<TKey, TValue>(keys.ThirdVanguardElement, values.ThirdVanguardElement ?? values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdAttackerElement, values.ThirdAttackerElement ?? values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdSupportElement, values.ThirdSupportElement ?? values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdFlexElement, values.ThirdFlexElement ?? values.FlexType);
        }


        public static IEnumerable<KeyValuePair<TKey, TValue>> GetSafeUnityEnumerable<TKey, TValue>(
            ITeamFullRolesStructureRead<TKey> keys, ITeamFullRolesStructureRead<TValue> values)
        where TValue : UnityEngine.Object
        {
            yield return new KeyValuePair<TKey, TValue>(keys.VanguardType, values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.AttackerType, values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.SupportType, values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.FlexType, values.FlexType);


            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryVanguardElement,
                values.SecondaryVanguardElement ? values.SecondaryVanguardElement : values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryAttackerElement,
                values.SecondaryAttackerElement ? values.SecondaryAttackerElement : values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondarySupportElement,
                values.SecondarySupportElement ? values.SecondarySupportElement : values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.SecondaryFlexElement,
                values.SecondaryFlexElement ? values.SecondaryFlexElement : values.FlexType);


            yield return new KeyValuePair<TKey, TValue>(keys.ThirdVanguardElement,
                values.ThirdVanguardElement ? values.ThirdVanguardElement : values.VanguardType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdAttackerElement,
                values.ThirdAttackerElement ? values.ThirdAttackerElement : values.AttackerType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdSupportElement,
                values.ThirdSupportElement ? values.ThirdSupportElement : values.SupportType);
            yield return new KeyValuePair<TKey, TValue>(keys.ThirdFlexElement,
                values.ThirdFlexElement ? values.ThirdFlexElement : values.FlexType);

        }
    }

    public static class UtilsCombatTeam
    {
        public static void SwitchStance(in CombatTeam team, in EnumTeam.Stance targetStance)
        {
            var fullStance = UtilsTeam.ParseStance(targetStance);
            SwitchStance(in team, in fullStance);
        }
        public static void SwitchStance(in CombatTeam team, in EnumTeam.StanceFull targetStance)
        {
            team.DataValues.CurrentStance = targetStance;
            CombatSystemSingleton.EventsHolder.OnStanceChange(in team, in targetStance);
        }

        public static void GainControl(in CombatTeam team, in float controlVariation)
        {
            var teamData = team.DataValues;
            float controlAmount = teamData.NaturalControl + controlVariation;
            controlAmount = Mathf.Clamp01(controlAmount);
            teamData.NaturalControl = controlAmount;

            var enemyTeam = team.EnemyTeam;
            var enemyTeamData = enemyTeam.DataValues;
            float enemyControl = 1-controlAmount; //By design: team vs enemy control goes in percent.
            enemyTeamData.NaturalControl = enemyControl;

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlChange(in team, in controlVariation, false);
            float enemyControlVariation = -controlVariation;
            eventsHolder.OnControlChange(in enemyTeam, in enemyControlVariation, false);
        }

        public static void BurstControl(in CombatTeam team, in float controlVariation)
        {
            team.DataValues.BurstControl += controlVariation;
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnControlChange(in team, in controlVariation, true);
        }



        public static void PutOnStandBy(in CombatEntity target)
        {
            target.Team.StandByMembers.PutOnStandBy(in target);
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnEntityWaitSequence(target);
        }
    }
}
