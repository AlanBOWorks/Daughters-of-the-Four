using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Buff/Concentration")]
    public class SBuffConcentrationEffect : SBuffEffect
    {
        [SerializeField] private EnumStats.ConcentrationStatType type;
        protected override void DoBuff(in float performerBuffPower, in float targetBuffReceivePower,
            in float effectValue,
            in IBasicStats<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateConcentrationStatBuffValue(
                in performerBuffPower,
                in targetBuffReceivePower,
                in effectValue);
            switch (type)
            {
                case EnumStats.ConcentrationStatType.Actions:
                    buffingStats.ActionsType += buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Speed:
                    buffingStats.SpeedType += buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Control:
                    buffingStats.ControlType += buffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Critical:
                    buffingStats.CriticalType += buffingValue;
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
