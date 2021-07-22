using Characters;
using UnityEngine;

namespace Skills
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            ICharacterBasicStats stats = target.CombatStats.BuffStats;
            DoBuff(user,stats,effectModifier);
        }

        public void DoEffect(CombatingEntity user, CombatingEntity target, bool isBurstType,
            float effectModifier = 1)
        {
            ICharacterBasicStats stats =
                isBurstType
                    ? (ICharacterBasicStats) target.CombatStats.BurstStats
                    : target.CombatStats.BuffStats;

            DoBuff(user, stats, effectModifier);
        }


       protected abstract void DoBuff(CombatingEntity user,
            ICharacterBasicStats buffStats, float effectModifier);
    }
}
