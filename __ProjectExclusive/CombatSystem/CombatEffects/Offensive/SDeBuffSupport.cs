using System;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "De-SUPPORT - N [DeBuff]",
        menuName = "Combat/Effect/DE-Buff Support", order = 100)]
    public class SDeBuffSupport : SDeBuff
    {
        [Title("Params")]
        [SerializeField] private EnumStats.SupportStatType buffStat;
        public EnumStats.SupportStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(
            SkillValuesHolders values, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(values, buffValue, isCritical);
            switch (buffStat)
            {
                case EnumStats.SupportStatType.Heal:
                    targetStats.Heal += finalBuffValue;
                    break;
                case EnumStats.SupportStatType.Buff:
                    targetStats.Buff += finalBuffValue;
                    break;
                case EnumStats.SupportStatType.ReceiveBuff:
                    targetStats.ReceiveBuff += finalBuffValue;
                    break;
                case EnumStats.SupportStatType.Shielding:
                    targetStats.Shielding += finalBuffValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SkillComponentResolution(this, finalBuffValue);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "De-SUPPORT - " + buffStat.ToString() + " [DeBuff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}