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
        public static bool CanBeTargeted(in CombatEntity target)
        {
            return target != null && target.CanBeTarget();
        }

        public static IEnumerable<CombatEntity> GetPossibleTargets(CombatSkill skill, CombatEntity currentControl)
        {
            var type = skill.Archetype;
            switch (type)
            {
                case EnumsSkill.Archetype.Self:
                    return GetSingleTarget(currentControl);
                case EnumsSkill.Archetype.Offensive:
                    // todo if team isFrontGuard
                    return GetOffensiveMembers();
                case EnumsSkill.Archetype.Support:
                    return GetSupportMembers();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IEnumerable<CombatEntity> GetOffensiveMembers()
            {
                var members = currentControl.Team.EnemyTeam;
                foreach (var member in members)
                {
                    if (CanBeTargeted(in member))
                        yield return member;
                }
            }

            IEnumerable<CombatEntity> GetSupportMembers()
            {
                var team = currentControl.Team;
                bool ignoreSelf = skill.IgnoreSelf();
                foreach (var member in team)
                {
                    if (!ignoreSelf || member != currentControl)
                        yield return member;
                }
            }
        }


        private static IEnumerable<CombatEntity> GetSingleTarget(CombatEntity target)
        {
            yield return target;
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
}
