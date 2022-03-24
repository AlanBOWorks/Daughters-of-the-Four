using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/DeBuff/Concentration")]
    public class SDeBuffConcentration : SDeBuffEffect
    {
        [SerializeField] private EnumStats.ConcentrationStatType type;
        protected override void DoDeBuff(in float performerDeBuffPower, in float targetDeBuffResistance, in float effectValue,
            in IBasicStats<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateStatsDeBuffValue(
                in performerDeBuffPower,
                in targetDeBuffResistance,
                in effectValue);
            switch (type)
            {
                case EnumStats.ConcentrationStatType.Actions:
                    buffingStats.ActionsType -= buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Speed:
                    buffingStats.SpeedType -= buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Control:
                    buffingStats.ControlType -= buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Critical:
                    buffingStats.CriticalType -= buffingValue;
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
