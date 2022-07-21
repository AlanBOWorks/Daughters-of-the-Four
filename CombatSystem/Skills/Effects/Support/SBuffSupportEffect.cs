using System;
using CombatSystem.Localization;
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
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + "_" + EffectPrefix;
        } public override string EffectTag => _effectTag;


        protected override void DoBuff(IBasicStats<float> buffingStats,
            ref float buffingValue)
        {
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

        public override string GetStatVariationEffectText() => LocalizeStats.LocalizeStatPrefix(type);

        [Button]
        private void UpdateAssetName()
        {
            base.UpdateAssetName(type.ToString());
        }

    }
}
