using _CombatSystem;
using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Initiative Modifier- N [Preset]",
        menuName = "Combat/Effects/Buff/Initiative Modifier")]
    public class SEffectInitiativeModifier : SEffectBuffBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float initiativeAddition = effectModifier * ( user.CombatStats.BuffPower);
            DoEffect(target,initiativeAddition);
        }


        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.AddInitiative(target, effectModifier, isBurstType);
            
        }

        private const string TemporalStatsPrefix = " Temporal Stat - INITIATIVE Modifier";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix;
        }
    }
}
