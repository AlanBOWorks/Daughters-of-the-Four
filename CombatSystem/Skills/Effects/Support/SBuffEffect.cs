using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    public abstract class SBuffEffect : SEffect, ISupportEffect
    {
        private const string BuffPrefix = EffectTags.BuffEffectName;
        private const string BurstPrefix = EffectTags.BurstEffectName;

        private const string BuffSmallPrefix = EffectTags.BuffEffectPrefix;
        private const string BurstSmallPrefix = EffectTags.BurstEffectPrefix;
        protected string GetBuffPrefix() => (isBurst) ? BurstPrefix : BuffPrefix;
        public override string EffectSmallPrefix => (isBurst) ? BurstSmallPrefix : BuffSmallPrefix;
        

        [SerializeField] protected bool isBurst;

        public override EnumsEffect.ConcreteType EffectType => (isBurst)
            ? EnumsEffect.ConcreteType.Burst
            : EnumsEffect.ConcreteType.Buff;


        public override void DoEffect(CombatEntity performer, CombatEntity target, float effectValue)
        {
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsEffects.CalculateBuffPower(in performerStats);
            float receivePower = UtilsStatsEffects.CalculateBuffReceivePower(in targetStats);

            IBasicStats<float> buffingStats = isBurst 
                ? UtilsStats.GetBurstStats(in targetStats, in performerStats) 
                : targetStats.BuffStats;

            DoBuff(bufferPower, receivePower, buffingStats, ref effectValue);
            CombatSystemSingleton.EventsHolder.OnBuffDone(new CombatPerformedEntities(performer,target),this,  effectValue);
        }

        protected abstract void DoBuff(float performerBuffPower, float targetBuffReceivePower,
            IBasicStats<float> buffingStats,
            ref float effectValue);

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
