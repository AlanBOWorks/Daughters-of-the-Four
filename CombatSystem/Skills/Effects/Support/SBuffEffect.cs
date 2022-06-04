using CombatSystem.Entity;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    public abstract class SBuffEffect : SEffect, ISupportEffect
    {
        private const string BuffPrefix = "BUFF";
        private const string BurstPrefix = "BURST";
        protected string GetBuffPrefix() => (isBurst) ? BurstPrefix : BuffPrefix;

        [SerializeField] protected bool isBurst;

        public override EnumStats.StatType EffectType => EnumStats.StatType.Buff;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsEffects.CalculateBuffPower(in performerStats);
            float receivePower = UtilsStatsEffects.CalculateBuffReceivePower(in targetStats);

            IBasicStats<float> buffingStats = isBurst 
                ? UtilsStats.GetBurstStats(in targetStats, in performerStats) 
                : targetStats.BuffStats;

            DoBuff(in bufferPower, in receivePower, in effectValue, in buffingStats);
        }

        protected abstract void DoBuff(in float performerBuffPower, in float targetBuffReceivePower,
            in float effectValue,
            in IBasicStats<float> buffingStats);

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
