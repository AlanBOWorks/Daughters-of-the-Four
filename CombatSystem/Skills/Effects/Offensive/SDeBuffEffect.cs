using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    public abstract class SDeBuffEffect : SEffect, IOffensiveEffect
    {

        private const string DeBuffPrefix = EffectTags.DeBuffEffectName;
        private const string DeBurstPrefix = EffectTags.DeBurstEffectName;
        
        private const string DeBuffSmallPrefix = EffectTags.DeBuffEffectPrefix;
        private const string DeBurstSmallPrefix = EffectTags.DeBurstEffectPrefix;

        [SerializeField] protected bool isBurst;

        public override EnumStats.StatType EffectType => EnumStats.StatType.DeBuff;
        protected string GetBuffPrefix() => (isBurst) ? DeBurstPrefix : DeBuffPrefix;
        public override string EffectSmallPrefix => (isBurst) ? DeBurstSmallPrefix : DeBuffSmallPrefix;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float debuffPower = UtilsStatsEffects.CalculateDeBuffPower(in performerStats);
            float debuffResistance = UtilsStatsEffects.CalculateDeBuffResistance(in targetStats);

            IBasicStats<float> debuffStats = isBurst 
                ? UtilsStats.GetBurstStats(in targetStats, in performerStats) 
                : targetStats.BuffStats;

            DoDeBuff(in debuffPower, in debuffResistance, in effectValue, in debuffStats);
        }

        protected abstract void DoDeBuff(in float performerDeBuffPower, in float targetDeBuffResistance,
            in float effectValue,
            in IBasicStats<float> buffingStats);



        protected string GenerateAssetName(in string statTypeName)
        {
            string buffType =  GetBuffPrefix() + " Stat - ";
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
