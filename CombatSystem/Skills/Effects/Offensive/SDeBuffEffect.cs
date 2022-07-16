using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    public abstract class SDeBuffEffect : SEffect, IDeBuffEffect
    {

        private const string DeBuffPrefix = EffectTags.DeBuffEffectName;
        private const string DeBurstPrefix = EffectTags.DeBurstEffectName;
        
        private const string DeBuffSmallPrefix = EffectTags.DeBuffEffectPrefix;
        private const string DeBurstSmallPrefix = EffectTags.DeBurstEffectPrefix;

        private const string DeBuffValuePrefix = "%";

        [SerializeField] protected bool isBurst;

        public override bool IsPercentSuffix() => true;
        public override float CalculateEffectTooltipValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateDeBuffPower(performerStats);
        }


        public override EnumsEffect.ConcreteType EffectType => (isBurst) 
            ? EnumsEffect.ConcreteType.DeBurst : EnumsEffect.ConcreteType.DeBuff;
        protected string GetBuffPrefix() => (isBurst) ? DeBurstPrefix : DeBuffPrefix;
        public override string EffectSmallPrefix => (isBurst) ? DeBurstSmallPrefix : DeBuffSmallPrefix;

        public override void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
        {
            var performer = entities.Performer;
            var target = entities.Target;

            var performerStats = performer.Stats;
            var targetStats = target.Stats;

            float debuffPower = UtilsStatsFormula.CalculateDeBuffPower(performerStats);
            float debuffResistance = UtilsStatsFormula.CalculateDeBuffResistance(targetStats);

            IBasicStats<float> debuffStats = isBurst 
                ? UtilsStats.GetBurstStats(targetStats, performerStats) 
                : targetStats.BuffStats;

            effectValue = UtilsStatsEffects.CalculateStatsDeBuffValue(effectValue, debuffPower, debuffResistance);
            effectValue *= luckModifier;

            DoDeBuff(debuffStats, ref effectValue);
            CombatSystemSingleton.EventsHolder.OnDeBuffDone(entities,this, effectValue);
        }

        public bool IsBurstEffect() => isBurst;

        protected abstract void DoDeBuff(IBasicStats<float> deBuffingStats, ref float debuffValue);

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
