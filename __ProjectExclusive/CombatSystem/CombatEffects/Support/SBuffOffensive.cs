using System;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "OFFENSIVE - N [Buff]",
        menuName= "Combat/Effect/Buff Offensive")]
    public class SBuffOffensive : SBuff
    {
        [Title("Params")]
        [SerializeField] private EnumStats.OffensiveStatType buffStat;
        public EnumStats.OffensiveStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(SkillValuesHolders values, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(values, buffValue, isCritical);

            switch (buffStat)
            {
                case EnumStats.OffensiveStatType.Attack:
                    targetStats.Attack += finalBuffValue;
                    break;
                case EnumStats.OffensiveStatType.Persistent:
                    targetStats.Persistent += finalBuffValue;
                    break;
                case EnumStats.OffensiveStatType.DeBuff:
                    targetStats.Debuff += finalBuffValue;
                    break;
                case EnumStats.OffensiveStatType.FollowUp:
                    targetStats.FollowUp += finalBuffValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SkillComponentResolution(this,finalBuffValue);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "OFFENSIVE - " + buffStat.ToString() + " [Buff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
