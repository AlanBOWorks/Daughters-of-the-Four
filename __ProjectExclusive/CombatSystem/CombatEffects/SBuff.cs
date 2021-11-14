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
            CombatEntityPairAction entities,
            EnumStats.BuffType buffType, 
            EnumEffects.TargetType effectTargetType,
            float effectValue, bool isCritical)
        {
            var user = entities.User;
            var skillTarget = entities.Target;

            var effectTargets = UtilsTarget.GetPossibleTargets(effectTargetType, user, skillTarget);

            var systemEvents = CombatSystemSingleton.EventsHolder;
            foreach (var effectTarget in effectTargets)
            {
                var resolution = DoBuffOn(user, effectTarget, buffType, effectValue, isCritical);
                DoEventCalls(systemEvents,entities,ref resolution);
            }

        }

        protected virtual void DoEventCalls(SystemEventsHolder systemEvents,CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(entities,ref resolution);
        }

        protected float CalculateBuffStats(CombatingEntity performer, float buffValue, bool isCritical)
        {
            return UtilStats.CalculateBuffPower(performer.CombatStats,buffValue, isCritical);
        }

        protected SkillComponentResolution DoBuffOn(
            CombatingEntity performer, CombatingEntity buffTarget, 
            EnumStats.BuffType buffType, float buffValue, bool isCritical)
        {
            var statsHolder = buffTarget.CombatStats;
            bool isSelfBuff = performer == buffTarget;
            var targetStats = statsHolder.GetBuffableStats(buffType,isSelfBuff);
            return DoBuffOn(performer, targetStats, buffValue, isCritical);
        }

        protected abstract SkillComponentResolution DoBuffOn(CombatingEntity performer, IBaseStats<float> targetStats, float buffValue,
            bool isCritical);

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Buff;
    }
}
