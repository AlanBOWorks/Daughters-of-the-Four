using System;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    // It's equivalent to SBuffOffensive but exits because type check (also could have a special behaviour in the future)
    [CreateAssetMenu(fileName = "De-OFFENSIVE - N [DeBuff]",
        menuName = "Combat/Effect/DE-Buff Offensive", order = 100)]
    public class SDeBuffOffensive : SDeBuff
    {
        [Title("Params")]
        [SerializeField] private EnumStats.OffensiveStatType buffStat;
        public EnumStats.OffensiveStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(
            CombatingEntity performer, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(performer, buffValue, isCritical);

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
            return new SkillComponentResolution(this, finalBuffValue);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "De-OFFENSIVE - " + buffStat.ToString() + " [DeBuff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
