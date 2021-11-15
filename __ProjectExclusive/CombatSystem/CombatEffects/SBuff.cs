using System;
using __ProjectExclusive.Player;
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
        public void DoBuff(ISkillParameters parameters,
            EnumStats.BuffType buffType,
            float effectValue, bool isCritical)
        {
            var user = parameters.Performer;

            var effectTargets = parameters.EffectTargets;

            var systemEvents = CombatSystemSingleton.EventsHolder;
            foreach (var effectTarget in effectTargets)
            {
                var resolution = DoBuffOn(user, effectTarget, buffType, effectValue, isCritical);
                DoEventCalls(systemEvents,parameters,ref resolution);
            }

        }

        protected virtual void DoEventCalls(SystemEventsHolder systemEvents,ISkillParameters parameters,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(parameters,ref resolution);
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

        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Buff;
        }
        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("F1") + "% " + GetBuffTooltip();
        }

        protected abstract string GetBuffTooltip();
    }
}
