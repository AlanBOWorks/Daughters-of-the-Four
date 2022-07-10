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
        [ShowInInspector,HideInEditorMode]
        private string _effectTag;

        private void OnEnable()
        {
            _effectTag = GetBuffPrefix() + "_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;


        protected override void DoBuff(IBasicStats<float> buffingStats,
            ref float buffingValue)
        {
            
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
