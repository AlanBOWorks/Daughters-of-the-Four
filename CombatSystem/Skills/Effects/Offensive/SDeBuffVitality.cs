using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/DeBuff/Vitality")]
    public class SDeBuffVitality : SDeBuffEffect
    {
        [SerializeField] private EnumStats.VitalityStatType type;
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;



        protected override void DoDeBuff(in float performerDeBuffPower, in float targetDeBuffResistance, in float effectValue,
            in IBasicStats<float> buffingStats)
        {
            float buffingValue = UtilsStatsEffects.CalculateStatsDeBuffValue(
                in performerDeBuffPower,
                in targetDeBuffResistance,
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
