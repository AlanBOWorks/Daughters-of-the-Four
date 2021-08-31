using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        [SerializeField]
        protected EnumStats.StatsType statsType = EnumStats.StatsType.Buff;

        protected IFullStats<float> GetBuff(CombatingEntity target)
        {
            return UtilsEnumStats.GetCombatStatsHolder(target, statsType);
        }


        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            string burstType = statsType.ToString();
            name = $"{burstType.ToUpper()}" +
                   $"{GetPrefix()} [ Buff Effect ]";
            RenameAsset();
        }
        protected abstract string GetPrefix();
    }
}
