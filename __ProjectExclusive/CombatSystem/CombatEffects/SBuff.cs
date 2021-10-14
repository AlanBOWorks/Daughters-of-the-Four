using System;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SBuff : SSkillComponentEffect, IBuff
    {
        public void DoBuff(SkillValuesHolders values,EnumStats.BuffType buffType, EnumEffects.TargetType effectTargetType, float effectValue,
            bool isCritical)
        {
            var user = values.User;
            var skillTarget = values.Target;
            var effectTargets = UtilsTarget.GetPossibleTargets(user, skillTarget, effectTargetType);

            switch (values.UsedSkill.GetTargetType())
            {
                case EnumSkills.TargetType.Self:
                    LoopSelf();
                    break;
                case EnumSkills.TargetType.Support:
                    LoopSupport();
                    break;
                case EnumSkills.TargetType.Offensive:
                    LoopOffensive();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void LoopSelf()
            {
                foreach (var effectTarget in effectTargets)
                {
                    DoBuffOn(values, effectTarget,buffType, effectValue, isCritical);
                }
            }
            void LoopOffensive()
            {
                foreach (var effectTarget in effectTargets)
                {
                    var resolution = DoBuffOn(values, effectTarget, buffType, effectValue, isCritical);
                    user.EventsHolder.OnPerformSupportAction(effectTarget,resolution);
                    effectTarget.EventsHolder.OnReceiveSupportAction(user,resolution);
                }
            }
            void LoopSupport()
            {
                foreach (var effectTarget in effectTargets)
                {
                    var resolution = DoBuffOn(values, effectTarget, buffType, effectValue, isCritical);
                    user.EventsHolder.OnPerformOffensiveAction(effectTarget, resolution);
                    effectTarget.EventsHolder.OnReceiveOffensiveAction(user, resolution);
                }
            }

        }

        protected float CalculateBuffStats(SkillValuesHolders values, float buffValue, bool isCritical)
        {
            return UtilStats.CalculateBuffPower(values.User.CombatStats,buffValue, isCritical);
        }

        protected SkillComponentResolution DoBuffOn(
            SkillValuesHolders values, CombatingEntity buffTarget, 
            EnumStats.BuffType buffType, float buffValue, bool isCritical)
        {
            var statsHolder = buffTarget.CombatStats;
            bool isSelfBuff = values.User == buffTarget;
            var targetStats = statsHolder.GetBuffableStats(buffType,isSelfBuff);
            return DoBuffOn(values, targetStats, buffValue, isCritical);
        }

        protected abstract SkillComponentResolution DoBuffOn(SkillValuesHolders values, IBaseStats<float> targetStats, float buffValue,
            bool isCritical);

    }
}
