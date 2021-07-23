using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        [SerializeField] protected bool isBurstType = false;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            ICharacterBasicStats stats =
                isBurstType
                    ? (ICharacterBasicStats)target.CombatStats.BurstStats
                    : target.CombatStats.BuffStats;

            DoBuff(user, stats, effectModifier);
        }

        protected abstract void DoBuff(CombatingEntity user,
            ICharacterBasicStats buffStats, float effectModifier);



        private void OnValidate()
        {
            string burstType = UtilityBuffStats.GetBuffTooltip(isBurstType);
            name = $"Offensive {burstType.ToUpper()} " +
                   $"- {GetToolTip().ToUpper()} - [Preset]";
            RenameAsset();
        }
        protected abstract string GetToolTip();
    }
}
