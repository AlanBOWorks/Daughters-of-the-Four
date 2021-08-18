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
            UtilsCombatStats.AddInitiative(GetBuff(target), effectModifier);
            target.Events.InvokeTemporalStatChange();
            var tempoHandler = CombatSystemSingleton.TempoHandler;

            tempoHandler.CallUpdateOnInitiativeBar(target);
            tempoHandler.CheckAndInjectEntityInitiative(target);
        }

        private const string TemporalStatsPrefix = " Temporal Stat - INITIATIVE Modifier";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix;
        }
    }
}
