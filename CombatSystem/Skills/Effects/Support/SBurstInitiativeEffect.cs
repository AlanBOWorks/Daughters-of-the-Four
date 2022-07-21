using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{

    [CreateAssetMenu(menuName = "Combat/Effect/Buff/Burst Initiative",
        fileName = "Burst Initiative [Effect]", order = 100)]
    public class SBurstInitiativeEffect : SEffect, IBuffEffect
    {
        public override void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
        {
            entities.Extract(out var performer, out var target);
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsFormula.CalculateBuffPower(performerStats);
            float receivePower = UtilsStatsFormula.CalculateReceiveBuffPower(targetStats);

            effectValue = UtilsStatsEffects.CalculateStatsBuffValue(effectValue, bufferPower, receivePower);

            effectValue *= luckModifier;
            effectValue = Mathf.Round(effectValue);

            UtilsCombatStats.TickInitiative(targetStats, effectValue);

            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnBuffDone(entities,this, effectValue);
        }

        public override float CalculateEffectByStatValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateBuffPower(performerStats);
        }

        public override bool IsPercentSuffix() => false;
        public bool IsBurstEffect() => true;

        public override string EffectTag => EffectTags.InitiativeEffectTag;
        public override string EffectSmallPrefix => EffectTags.InitiativeEffectPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Initiative;
        public string GetStatVariationEffectText() => LocalizeStats.LocalizeStatPrefix(StatsTags.InitiativeStatPrefix);
    }
}
