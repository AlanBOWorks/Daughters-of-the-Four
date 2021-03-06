using System;
using CombatSystem.Localization;
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
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + EffectPrefix;
        }
        public override string EffectTag => _effectTag;



        protected override void DoBuff(IBasicStats<float> buffingStats,
            ref float buffingValue)
        {
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

        public override string GetStatVariationEffectText() => LocalizeStats.LocalizeStatPrefix(type);


        [Button]
        private void UpdateAssetName()
        {
            base.UpdateAssetName(type.ToString());
        }
    }
}
