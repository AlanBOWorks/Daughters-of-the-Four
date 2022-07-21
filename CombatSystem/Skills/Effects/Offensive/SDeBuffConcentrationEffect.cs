using System;
using CombatSystem.Localization;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/DeBuff/Concentration")]
    public class SDeBuffConcentrationEffect : SDeBuffEffect
    {
        [SerializeField] private EnumStats.ConcentrationStatType type;
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;

        public override bool IsPercentSuffix()
        {
            return type switch
            {
                EnumStats.ConcentrationStatType.Actions => false,
                EnumStats.ConcentrationStatType.Speed => false,
                _ => true
            };
        }

        protected override void DoDeBuff(IBasicStats<float> deBuffingStats, ref float deBuffingValue)
        {
            switch (type)
            {
                case EnumStats.ConcentrationStatType.Actions:
                    deBuffingStats.ActionsType -= deBuffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Speed:
                    deBuffingStats.SpeedType -= deBuffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Control:
                    deBuffingStats.ControlType -= deBuffingValue;
                    break;
                case EnumStats.ConcentrationStatType.Critical:
                    deBuffingStats.CriticalType -= deBuffingValue;
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
