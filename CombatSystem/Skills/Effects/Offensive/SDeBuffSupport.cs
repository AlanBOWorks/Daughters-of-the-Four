using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
    menuName = "Combat/Effect/DeBuff/Support")]
    public class SDeBuffSupport : SDeBuffEffect
    {
        [SerializeField] private EnumStats.SupportStatType type;
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;



        protected override void DoDeBuff(IBasicStats<float> buffingStats, float performerDeBuffPower,
            float targetDeBuffResistance, ref float effectValue)
        {
            float buffingValue = UtilsStatsEffects.CalculateStatsDeBuffValue(
                in performerDeBuffPower,
                in targetDeBuffResistance,
                in effectValue);


            switch (type)
            {
                case EnumStats.SupportStatType.Heal:
                    buffingStats.HealType -= buffingValue;
                    break;
                case EnumStats.SupportStatType.Shielding:
                    buffingStats.ShieldingType -= buffingValue;
                    break;
                case EnumStats.SupportStatType.Buff:
                    buffingStats.BuffType -= buffingValue;
                    break;
                case EnumStats.SupportStatType.ReceiveBuff:
                    buffingStats.ReceiveBuffType -= buffingValue;
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
