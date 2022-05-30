using System;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{

    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/DeBuff/Offensive")]
    public class SDeBuffOffensive : SDeBuffEffect
    {
        [SerializeField] private EnumStats.OffensiveStatType type;
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
                case EnumStats.OffensiveStatType.Attack:
                    buffingStats.AttackType -= buffingValue;
                    break;
                case EnumStats.OffensiveStatType.OverTime:
                    buffingStats.OverTimeType -= buffingValue;
                    break;
                case EnumStats.OffensiveStatType.DeBuff:
                    buffingStats.DeBuffType -= buffingValue;
                    break;
                case EnumStats.OffensiveStatType.FollowUp:
                    buffingStats.FollowUpType -= buffingValue;
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
