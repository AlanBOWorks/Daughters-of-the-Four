using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Skills
{

    public static class UtilsTarget
    {
        private static readonly List<CombatEntity> TargetsHelper = new List<CombatEntity>();

        public static bool CanBeTargeted(in CombatEntity target)
        {
            return target != null && target.CanBeTarget();
        }

        public static void HandlePossibleTargets(ICollection<CombatEntity> targetsHelper,
            CombatEntity performer,
            CombatSkill skill)
        {
            bool ignoreSelf = skill.IgnoreSelf();
            HandlePossibleTargets(targetsHelper, performer, skill.Archetype, ignoreSelf);
        }


        public static void HandlePossibleTargets(ICollection<CombatEntity> targetsHelper,
            CombatEntity performer,
            EnumsSkill.Archetype type, bool ignoreSelf)
        {
            targetsHelper.Clear();

            switch (type)
            {
                case EnumsSkill.Archetype.Self:
                    HandleSingleTarget(performer);
                    break;
                case EnumsSkill.Archetype.Offensive:
                    HandleOffensiveMembers();
                    break;
                case EnumsSkill.Archetype.Support:
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
                        : enemyTeam;

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
                foreach (var member in team)
                {
                    if (ignoreSelf && member == performer)
                        continue;

                    targetsHelper.Add(member);
                }

            }
        }
        public static IReadOnlyList<CombatEntity> GetPossibleTargets(CombatEntity performer, CombatSkill skill)
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
            ExtractTargetEffectsValues(in targetEntity,out var targetTeam, out var positioning);
            switch (positioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    return targetTeam.FrontLineType;
                case EnumTeam.Positioning.MidLine:
                    return targetTeam.MidLineType.Concat(targetTeam.FlexLineType);
                case EnumTeam.Positioning.BackLine:
                    return targetTeam.BackLineType;
                case EnumTeam.Positioning.FlexLine:
                    return targetTeam.FlexLineType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }
        }

        internal static IEnumerable<CombatEntity> GetSupportLine(CombatEntity targetEntity)
        {
            ExtractTargetEffectsValues(in targetEntity,out var targetTeam, out var positioning);
            switch (positioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    return ConcatLineToFlex(targetTeam.FrontLineType);
                case EnumTeam.Positioning.MidLine:
                    return ConcatLineToFlex(targetTeam.MidLineType);
                case EnumTeam.Positioning.BackLine:
                    return ConcatLineToFlex(targetTeam.BackLineType);
                case EnumTeam.Positioning.FlexLine:
                    return targetTeam.FlexLineType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }

            IEnumerable<CombatEntity> ConcatLineToFlex(IEnumerable<CombatEntity> line) =>
                line.Concat(targetTeam.FlexLineType);
        }

        private static void ExtractTargetEffectsValues(in CombatEntity targetEntity,out CombatTeam targetTeam, out EnumTeam.Positioning positioning)
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

            foreach (var member in target.Team)
            {
                if (UtilsTarget.CanBeTargeted(in member))
                    aliveTeam.Add(member);
            }
        }

    }
}
