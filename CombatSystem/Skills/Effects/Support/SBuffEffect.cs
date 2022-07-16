using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    public abstract class SBuffEffect : SEffect, IBuffEffect
    {
        private const string BuffPrefix = EffectTags.BuffEffectName;
        private const string BurstPrefix = EffectTags.BurstEffectName;

        private const string BuffSmallPrefix = EffectTags.BuffEffectPrefix;
        private const string BurstSmallPrefix = EffectTags.BurstEffectPrefix;

        private const string BuffValuePrefix = "%";

        protected string GetBuffPrefix() => (isBurst) ? BurstPrefix : BuffPrefix;
        public override string EffectSmallPrefix => (isBurst) ? BurstSmallPrefix : BuffSmallPrefix;
        

        [SerializeField] protected bool isBurst;

        public override EnumsEffect.ConcreteType EffectType => (isBurst)
            ? EnumsEffect.ConcreteType.Burst
            : EnumsEffect.ConcreteType.Buff;

        public override bool IsPercentSuffix() => true;
        public override float CalculateEffectTooltipValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateBuffPower(performerStats);
        }


        public override void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
        {
            entities.Extract(out var performer, out var target);
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsFormula.CalculateBuffPower(performerStats);
            float receivePower = UtilsStatsFormula.CalculateReceiveBuffPower(targetStats);

            IBasicStats<float> buffingStats = isBurst 
                ? UtilsStats.GetBurstStats(targetStats, performerStats) 
                : targetStats.BuffStats;


            effectValue = UtilsStatsEffects.CalculateStatsBuffValue(effectValue, bufferPower, receivePower);
            effectValue *= luckModifier;

            DoBuff(buffingStats, ref effectValue);
            CombatSystemSingleton.EventsHolder.OnBuffDone(entities, this,  effectValue);
        }

        public bool IsBurstEffect() => isBurst;

        protected abstract void DoBuff(IBasicStats<float> buffingStats,
            ref float buffingValue);

        protected string GenerateAssetName(in string statTypeName)
        {
            string buffType = GetBuffPrefix() + " Stat - ";
            const string suffix = " [Effect]";

            return buffType + statTypeName + suffix;
        }

        protected void UpdateAssetName(in string statTypeName)
        {
            string generatedName = GenerateAssetName(in statTypeName);
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }

}
