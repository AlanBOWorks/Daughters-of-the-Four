using System;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SBuff : SSkillComponentEffect, IBuff
    {
        public void DoBuff(
            SkillValuesHolders values,
            EnumStats.BuffType buffType, 
            EnumEffects.TargetType effectTargetType,
            float effectValue, bool isCritical)
        {
            var user = values.User;
            var skillTarget = values.Target;

            var effectTargets = UtilsTarget.GetPossibleTargets(user, skillTarget, effectTargetType);

            var systemEvents = CombatSystemSingleton.EventsHolder;
            foreach (var effectTarget in effectTargets)
            {
                var resolution = DoBuffOn(values, effectTarget, buffType, effectValue, isCritical);
                var entities = new CombatEntityPairAction(user,effectTarget);
                DoEventCalls(systemEvents,entities,ref resolution);
            }

        }

        protected virtual void DoEventCalls(SystemEventsHolder systemEvents,CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportAction(entities,ref resolution);
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
