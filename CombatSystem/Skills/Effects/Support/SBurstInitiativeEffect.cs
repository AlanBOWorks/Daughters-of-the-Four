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
            var target = entities.Target;
            var targetStats = target.Stats;
            float finalInitiativeAddition = CalculateEffectTooltipValue(targetStats, effectValue);
            UtilsCombatStats.TickInitiative(targetStats, finalInitiativeAddition);

            CombatSystemSingleton.EventsHolder.OnBuffDone(entities,this, effectValue);
        }

        public override float CalculateEffectTooltipValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateBuffPower(performerStats);
        }

        public override bool IsPercentSuffix() => false;
        public bool IsBurstEffect() => true;

        public override string EffectTag => EffectTags.InitiativeEffectTag;
        public override string EffectSmallPrefix => EffectTags.InitiativeEffectPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Initiative;
    }
}
