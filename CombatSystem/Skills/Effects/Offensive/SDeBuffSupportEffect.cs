using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
    menuName = "Combat/Effect/DeBuff/Support")]
    public class SDeBuffSupportEffect : SDeBuffEffect
    {
        [SerializeField] private EnumStats.SupportStatType type;
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
                case EnumStats.SupportStatType.Heal:
                    deBuffingStats.HealType -= deBuffingValue;
                    break;
                case EnumStats.SupportStatType.Shielding:
                    deBuffingStats.ShieldingType -= deBuffingValue;
                    break;
                case EnumStats.SupportStatType.Buff:
                    deBuffingStats.BuffType -= deBuffingValue;
                    break;
                case EnumStats.SupportStatType.ReceiveBuff:
                    deBuffingStats.ReceiveBuffType -= deBuffingValue;
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
