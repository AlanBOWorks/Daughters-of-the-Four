using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Harmony Modifier- N [Preset]",
        menuName = "Combat/Effects/Buff/Harmony Modifier")]
    public class SHarmonyBuffEffect : SEffectBuffBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float harmonyAddition = 1)
        {
            float enlightenmentAmount = arguments.UserStats.DisruptionResistance;
            float harmonyModifier = enlightenmentAmount * harmonyAddition;
            DoEffect(target,harmonyModifier);
        }
        
        public override void DoEffect(CombatingEntity target, float harmonyAddition)
        {
            UtilsCombatStats.VariateHarmony(target,GetBuff(target), harmonyAddition);
        }

        private const string TemporalStatsPrefix = " Temporal Stat - HARMONY Modifier";
        protected override string GetPrefix() => TemporalStatsPrefix;
    }
}
