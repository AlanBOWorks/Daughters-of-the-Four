using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Buff/Support")]
    public class SBuffSupportEffect : SBuffEffect
    {
        [SerializeField] private EnumStats.SupportStatType type;

        protected override void DoBuff(in float performerBuffPower, in float targetBuffReceivePower,
            in float effectValue,
            in IBasicStats<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateSupportStatBuffValue(
                in performerBuffPower,
                in targetBuffReceivePower,
                in effectValue);

            switch (type)
            {
                case EnumStats.SupportStatType.Heal:
                    buffingStats.HealType += buffingValue;
                    break;
                case EnumStats.SupportStatType.Shielding:
                    buffingStats.ShieldingType += buffingValue;
                    break;
                case EnumStats.SupportStatType.Buff:
                    buffingStats.BuffType += buffingValue;
                    break;
                case EnumStats.SupportStatType.ReceiveBuff:
                    buffingStats.ReceiveBuffType += buffingValue;
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
