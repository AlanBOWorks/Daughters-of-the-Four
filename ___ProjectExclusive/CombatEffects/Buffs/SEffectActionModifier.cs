using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "ActionModifier Effect - N [Preset]",
        menuName = "Combat/Effects/Buff/Action Modifier")]
    public class SEffectActionModifier : SEffectBuffBase
    {
        private const float BuffStatLowerCap = 1f; 
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float statAddition = user.CombatStats.BuffPower - BuffStatLowerCap;
            if (statAddition < 0) statAddition = 0;
            DoEffect(target,statAddition);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.AddActionAmount(target.CombatStats,Mathf.RoundToInt(effectModifier));
            target.Events.InvokeTemporalStatChange();

        }

        private const string TemporalStatsPrefix = " Temporal Stat - ACTION Modifier";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix;
        }
    }
}
