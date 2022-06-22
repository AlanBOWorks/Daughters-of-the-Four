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

        public static bool IsOffensiveSkill(ISkill skill)
        {
            return skill.TeamTargeting == EnumsSkill.TeamTargeting.Offensive || skill is IAttackerSkill;
        }
    }


    public static class UtilsTarget
    {
        private static readonly List<CombatEntity> TargetsHelper = new List<CombatEntity>();


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
            bool isAlly = UtilsTeam.IsAllyEntity(in performer, in target);
            if (isAlly) return EnumsSkill.TeamTargeting.Support;
            return EnumsSkill.TeamTargeting.Offensive;
        }

        public static bool CanBeTargeted(in CombatEntity target)
        {
            return target != null;
        }

        public static void HandlePossibleTargets(ICollection<CombatEntity> targetsHelper,
            CombatEntity performer,
            ICombatSkill skill)
        {
            bool ignoreSelf = skill.IgnoreSelf();
            HandlePossibleTargets(targetsHelper, performer, skill.TeamTargeting, ignoreSelf);
        }


        public static void HandlePossibleTargets(ICollection<CombatEntity> targetsHelper,
            CombatEntity performer,
            EnumsSkill.TeamTargeting type, bool ignoreSelf)
        {
            targetsHelper.Clear();

            switch (type)
            {
                case EnumsSkill.TeamTargeting.Self:
                    HandleSingleTarget(performer);
                    break;
                case EnumsSkill.TeamTargeting.Offensive:
                    HandleOffensiveMembers();
                    break;
                case EnumsSkill.TeamTargeting.Support:
                    HandleSupportMembers();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void HandleSingleTarget(CombatEntity target)
            {
                targetsHelper.Add(target);
            }

            void HandleOffensiveMembers()
            {
                var enemyTeam = performer.Team.EnemyTeam;
                var enemyGuarding = enemyTeam.GuardHandler;
                var members =
                    enemyGuarding.CanGuard()
                        ? GetGuarderLine()
                        : enemyTeam.GetAllMembers();

                foreach (var member in members)
                {
                    if (CanBeTargeted(in member))
                        targetsHelper.Add(member);
                }

                IEnumerable<CombatEntity> GetGuarderLine()
                {
                    var guarder = enemyGuarding.GetCurrentGuarder();
                    return UtilsTeam.GetMemberLine(in guarder);
                }
                }

            void HandleSupportMembers()
            {
                var team = performer.Team;
                foreach (var member in team.GetAllMembers())
                {
                    if (ignoreSelf && member == performer)
                        continue;

                    targetsHelper.Add(member);
                }

            }
        }
        public static IReadOnlyList<CombatEntity> GetPossibleTargets(CombatEntity performer, ICombatSkill skill)
        {
            HandlePossibleTargets(TargetsHelper, performer, skill);
            return TargetsHelper;
        }

        public static IEnumerable<CombatEntity> GetEffectTargets(EnumsEffect.TargetType targetType)
        {
            var targetingHandler = CombatSystemSingleton.SkillTargetingHandler;
            switch (targetType)
            {
                case EnumsEffect.TargetType.Target:
                    return targetingHandler.TargetType.SingleType;
                case EnumsEffect.TargetType.TargetLine:
                    return targetingHandler.TargetType.TargetLine;
                case EnumsEffect.TargetType.TargetTeam:
                    return targetingHandler.TargetType.TargetTeam;

                case EnumsEffect.TargetType.Performer:
                    return targetingHandler.PerformerType.SingleType;
                case EnumsEffect.TargetType.PerformerLine:
                    return targetingHandler.PerformerType.TargetLine;
                case EnumsEffect.TargetType.PerformerTeam:
                    return targetingHandler.PerformerType.TargetTeam;


                case EnumsEffect.TargetType.All:
                    return targetingHandler.AllType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
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

    public static class UtilsTargetsCollection
    {
        public static void HandleLine(in ICollection<CombatEntity> aliveLine, in CombatEntity target, in bool isAlly)
        {
            aliveLine.Clear(); //safe clear

            var targetLine = isAlly
                ? UtilsTarget.GetSupportLine(target)
                : UtilsTarget.GetOffensiveLine(target);
            foreach (var member in targetLine)
            {
                if (!UtilsTarget.CanBeTargeted(in member)) continue;

                aliveLine.Add(member);
            }
        }

        public static void HandleTeam(in ICollection<CombatEntity> aliveTeam, in CombatEntity target)
        {
            aliveTeam.Clear(); //safe clear

            foreach (var member in target.Team.GetAllMembers())
            {
                if (UtilsTarget.CanBeTargeted(in member))
                    aliveTeam.Add(member);
            }
        }

    }
}
