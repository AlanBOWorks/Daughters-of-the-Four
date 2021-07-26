using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        [SerializeField] protected bool isBurstType = false;


        protected ICharacterBasicStats GetBuff(CombatingEntity target)
        {
            ICharacterBasicStats stats =
                isBurstType
                    ? (ICharacterBasicStats)target.CombatStats.BurstStats
                    : target.CombatStats.BuffStats;
            return stats;
        }


        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            string burstType = UtilityBuffStats.GetBuffTooltip(isBurstType);
            name = $"{burstType.ToUpper()}" +
                   $"{GetPrefix()} [ Buff Effect ]";
            RenameAsset();
        }
        protected abstract string GetPrefix();
    }
}
