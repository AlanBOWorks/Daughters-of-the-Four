using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "ActionModifier Effect - N [Preset]",
        menuName = "Combat/Effects/Buff/Action Modifier")]
    public class SEffectActionModifier : SEffectBuffBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.AddActionAmount(target.CombatStats,Mathf.RoundToInt(effectModifier +0.1f));
            target.Events.InvokeTemporalStatChange();

        }

        private const string TemporalStatsPrefix = " Temporal Stat - ACTION Modifier";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix;
        }
    }
}
