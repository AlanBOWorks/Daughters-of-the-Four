using Characters;
using UnityEngine;

namespace Skills
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        [SerializeField] private bool isBurstType = false;
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            if (isBurstType)
                DoBurst(user, target.CombatStats.BurstStats, effectModifier);
            else
                DoBuff(user, target.CombatStats.BuffStats, effectModifier);
        }

        protected abstract void DoBurst(CombatingEntity user, 
            ICharacterFullStats burstStats, float effectModifier);

        protected abstract void DoBuff(CombatingEntity user,
            ICharacterBasicStats buffStats, float effectModifier);
    }
}
