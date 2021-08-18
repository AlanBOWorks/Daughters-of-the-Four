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
        protected override ICharacterFullStats GetBuff(CombatingEntity target)
            => GetBurstOrBase(target);

        
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float harmonyAddition = 1)
        {
            float harmonyModifier = user.CombatStats.Enlightenment * harmonyAddition;
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
