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



        protected override void DoDeBuff(IBasicStats<float> deBuffingStats, ref float deBuffingValue)
        {
            switch (type)
            {
                case EnumStats.VitalityStatType.Health:
                    deBuffingStats.HealthType -= deBuffingValue;
                    break;
                case EnumStats.VitalityStatType.Mortality:
                    deBuffingStats.MortalityType -= deBuffingValue;
                    break;
                case EnumStats.VitalityStatType.DamageReduction:
                    deBuffingStats.DamageReductionType -= deBuffingValue;
                    break;
                case EnumStats.VitalityStatType.DebuffResistance:
                    deBuffingStats.DeBuffResistanceType -= deBuffingValue;
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
