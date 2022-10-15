using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using JetBrains.Annotations;
using UnityEngine;

namespace CombatSystem.Skills
{
    public static class UtilsSkill
    {
        public static T GetElement<T>(EnumsSkill.TeamTargeting type, ISkillArchetypeStructureRead<T> structure)
        {
            switch (type)
            {
                case EnumsSkill.TeamTargeting.Self:
                    return structure.SelfSkillType;
                case EnumsSkill.TeamTargeting.Offensive:
                    return structure.OffensiveSkillType;
                case EnumsSkill.TeamTargeting.Support:
                    return structure.SupportSkillType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static EnumsSkill.TeamTargeting GetReceiveSkillType([NotNull] ISkill skill, CombatEntity performer,
            CombatEntity target)
        {
            if (performer == null || target == null)
            {
                return skill.TeamTargeting;
            }

            var type = skill.TeamTargeting;
            if (performer == target) return EnumsSkill.TeamTargeting.Self;
            if (type != EnumsSkill.TeamTargeting.Self) return type;

            // if the performer acts as a self but effect are for groups then:
            bool isAlly = UtilsTeam.IsAllyEntity(performer, target);
            if (isAlly) return EnumsSkill.TeamTargeting.Support;
            return EnumsSkill.TeamTargeting.Offensive;
        }

        public static EnumsSkill.TeamTargeting GetReceiveSkillType(CombatEntity performer,
            CombatEntity target)
        {
            if (performer == target) return EnumsSkill.TeamTargeting.Self;

            bool isAlly = UtilsTeam.IsAllyEntity(performer, target);
            if (isAlly) return EnumsSkill.TeamTargeting.Support;
            return EnumsSkill.TeamTargeting.Offensive;
        }

        public static bool IsOffensiveSkill(ISkill skill)
        {
            return skill.TeamTargeting == EnumsSkill.TeamTargeting.Offensive || skill is IAttackerSkill;
        }
    }


    public static class UtilsTarget
    {
        private static readonly List<CombatEntity> TargetSelectionHelper = new List<CombatEntity>();
        
        private static IReadOnlyList<CombatEntity> GetIgnoreSelfSupportPossibleTargets(CombatEntity performer)
        {
            TargetSelectionHelper.Clear();
            HandleSupportMembers();
            return TargetSelectionHelper;

            void HandleSupportMembers()
            {
                var team = performer.Team;
                foreach (var member in team.GetAllMembers())
                {
                    if (member == performer)
                        continue;

                    TargetSelectionHelper.Add(member);
                }

            }
        }

        private static IReadOnlyList<CombatEntity> GetSelfAsCollectionTarget(CombatEntity performer)
        {
            TargetSelectionHelper.Clear();
            TargetSelectionHelper.Add(performer);
            return TargetSelectionHelper;
        }

        public static IReadOnlyList<CombatEntity> GetPossibleTargets(ISkill skill, CombatEntity performer)
        {
            var type = skill.TeamTargeting;
            switch (type)
            {
                case EnumsSkill.TeamTargeting.Self:
                    return GetSelfAsCollectionTarget(performer);
                case EnumsSkill.TeamTargeting.Offensive:
                    return performer.Team.EnemyTeam.GetAllMembers();
                case EnumsSkill.TeamTargeting.Support:
                    return GetSupportTargetType();
                default:
                    throw new ArgumentOutOfRangeException();
            }


            IReadOnlyList<CombatEntity> GetSupportTargetType()
            {
                bool isSelfIgnore = skill.IgnoreSelf;
                return (isSelfIgnore)
                    ? GetIgnoreSelfSupportPossibleTargets(performer)
                    : performer.Team.GetAllMembers();
            }
        }


        public static IEnumerable<CombatEntity> GetEffectTargets(
            EnumsEffect.TargetType targetType,
            CombatEntity performer,
            CombatEntity target)
        {
            var targetTeam = target.Team;
            bool isEnemy = targetTeam.Contains(performer);
            switch (targetType)
            {
                case EnumsEffect.TargetType.Target:
                    return GetSingleTargetEnumerable(target);
                case EnumsEffect.TargetType.TargetLine:
                    return GetTargetLine();
                case EnumsEffect.TargetType.TargetTeam:
                    return target.Team.GetAllMembers();

                case EnumsEffect.TargetType.Performer:
                    return GetSingleTargetEnumerable(performer);
                case EnumsEffect.TargetType.PerformerLine:
                    return GetSupportLine(performer);
                case EnumsEffect.TargetType.PerformerTeam:
                    return performer.Team.GetAllMembers();


                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }



            IEnumerable<CombatEntity> GetTargetLine()
            {
                return (isEnemy)
                    ? GetSupportLine(target)
                    : GetOffensiveLine(target);
            }
        }

        private static IEnumerable<CombatEntity> GetSingleTargetEnumerable(CombatEntity target)
        {
            yield return target;
        }
        internal static IEnumerable<CombatEntity> GetOffensiveLine(CombatEntity targetEntity)
        {
            ExtractTargetEffectsValues(in targetEntity, out var team, out var positioning);
            var membersPositions = team.GetAllPositions();
            switch (positioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    return membersPositions.FrontLineType;
                case EnumTeam.Positioning.FlexLine:
                case EnumTeam.Positioning.MidLine:
                    return membersPositions.MidLineType.Concat(membersPositions.FlexLineType);
                case EnumTeam.Positioning.BackLine:
                    return membersPositions.BackLineType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }
        }

        internal static IEnumerable<CombatEntity> GetSupportLine(CombatEntity targetEntity)
        {
            ExtractTargetEffectsValues(in targetEntity,out var team, out var positioning);
            var membersPositions = team.GetAllPositions();
            switch (positioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    return ConcatLineToFlex(membersPositions.FrontLineType);
                case EnumTeam.Positioning.FlexLine:
                case EnumTeam.Positioning.MidLine:
                    return ConcatLineToFlex(membersPositions.MidLineType);
                case EnumTeam.Positioning.BackLine:
                    return ConcatLineToFlex(membersPositions.BackLineType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }

            IEnumerable<CombatEntity> ConcatLineToFlex(IEnumerable<CombatEntity> line) =>
                line.Concat(membersPositions.FlexLineType);
        }

        private static void ExtractTargetEffectsValues(
            in CombatEntity targetEntity,
            out CombatTeam targetTeam, 
            out EnumTeam.Positioning positioning)
        {
            targetTeam = targetEntity.Team;
            positioning = targetEntity.PositioningType;
        }
    }

}
