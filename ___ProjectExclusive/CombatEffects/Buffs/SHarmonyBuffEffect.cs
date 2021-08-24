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
        protected override IFullStats GetBuff(CombatingEntity target)
            => GetBurstOrBase(target);

        
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float harmonyAddition = 1)
        {
            float enlightenmentAmount = arguments.UserStats.GetEnlightenment();
            float harmonyModifier = enlightenmentAmount * harmonyAddition;
            DoEffect(target,harmonyModifier);
        }
        
        public override void DoEffect(CombatingEntity target, float harmonyAddition)
        {
            UtilsCombatStats.AddHarmony(target,GetBuff(target), harmonyAddition);
            target.Events.InvokeTemporalStatChange();
        }

        private const string TemporalStatsPrefix = " Temporal Stat - HARMONY Modifier";
        protected override string GetPrefix() => TemporalStatsPrefix;
        protected override string FalseBurstTooltip() => IsBasePrefix;
    }
}
