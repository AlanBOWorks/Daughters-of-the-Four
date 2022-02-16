using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]", 
        menuName = "Combat/Effect/Buff/Offensive")]
    public class SBuffOffensiveEffect : SBuffEffect
    {
        [SerializeField] private EnumStats.OffensiveStatType type;

        protected override void DoBuff(in float performerBuffPower, in float targetBuffReceivePower, in float effectValue,
            in StatsBase<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateOffensiveStatBuffed(
                in performerBuffPower,
                in targetBuffReceivePower,
                in effectValue);
            switch (type)
            {
                case EnumStats.OffensiveStatType.Attack:
                    buffingStats.AttackType += buffingValue;
                    break;
                case EnumStats.OffensiveStatType.OverTime:
                    buffingStats.OverTimeType += buffingValue;
                    break;
                case EnumStats.OffensiveStatType.DeBuff:
                    buffingStats.DeBuffType += buffingValue;
                    break;
                case EnumStats.OffensiveStatType.FollowUp:
                    buffingStats.FollowUpType += buffingValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Button]
        private void UpdateAssetName()
        {
            base.UpdateAssetName(type.ToString());
        }
    }
}
