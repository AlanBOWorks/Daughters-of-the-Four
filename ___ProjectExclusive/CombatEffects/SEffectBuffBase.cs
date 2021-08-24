﻿using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffectBuffBase : SEffectBase
    {
        [InfoBox("$FalseBurstTooltip")]
        [SerializeField] protected bool isBurstType = false;

        protected virtual IFullStats GetBuff(CombatingEntity target)
        {
            var stats =
                isBurstType
                    ? target.CombatStats.BurstStats
                    : target.CombatStats.BuffStats;
            return stats;
        }

        protected IFullStats GetBurstOrBase(CombatingEntity target)
        {
            var stats =
                isBurstType
                    ? target.CombatStats.BurstStats
                    : (IFullStats) target.CombatStats.BaseStats;
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

        protected const string IsBuffPrefix = "False is [Buff] Type";
        protected const string IsBasePrefix = "False is [Base] Type";
        protected virtual string FalseBurstTooltip()
        {
            return IsBuffPrefix;
        }
    }
}
