using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Buff/Vitality")]
    public class SBuffVitalityEffect : SBuffEffect
    {
        [SerializeField] private EnumStats.VitalityStatType type;
        protected override void DoBuff(in float performerBuffPower, in float targetBuffReceivePower,
            in float effectValue,
            in IBasicStats<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateVitalityStatBuffValue(
                in performerBuffPower,
                in targetBuffReceivePower,
                in effectValue);
            switch (type)
            {
                case EnumStats.VitalityStatType.Health:
                    buffingStats.HealthType += buffingValue;
                    break;
                case EnumStats.VitalityStatType.Mortality:
                    buffingStats.MortalityType += buffingValue;
                    break;
                case EnumStats.VitalityStatType.DamageReduction:
                    buffingStats.DamageReductionType += buffingValue;
                    break;
                case EnumStats.VitalityStatType.DebuffResistance:
                    buffingStats.DeBuffResistanceType += buffingValue;
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
