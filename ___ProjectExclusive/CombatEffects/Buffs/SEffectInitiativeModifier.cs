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
<<<<<<< Updated upstream
        protected override ICharacterFullStats GetBuff(CombatingEntity target)
            => GetBurstOrBase(target);

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
=======
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
>>>>>>> Stashed changes
        {
            var buffPower = arguments.UserStats.BuffPower;
            float initiativeAddition = effectModifier * ( buffPower);
            DoEffect(target,initiativeAddition);
        }


        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.AddInitiative(target.CombatStats, effectModifier);
            TempoHandler.CallUpdateOnInitiativeBar(target);
            target.Events.InvokeTemporalStatChange();
        }

        private const string TemporalStatsPrefix = " Temporal Stat - INITIATIVE Modifier";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix;
        }
    }
}
