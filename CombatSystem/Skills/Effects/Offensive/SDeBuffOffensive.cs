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



        protected override void DoDeBuff(IBasicStats<float> deBuffingStats, ref float deBuffingValue)
        {
            switch (type)
            {
                case EnumStats.OffensiveStatType.Attack:
                    deBuffingStats.AttackType -= deBuffingValue;
                    break;
                case EnumStats.OffensiveStatType.OverTime:
                    deBuffingStats.OverTimeType -= deBuffingValue;
                    break;
                case EnumStats.OffensiveStatType.DeBuff:
                    deBuffingStats.DeBuffType -= deBuffingValue;
                    break;
                case EnumStats.OffensiveStatType.FollowUp:
                    deBuffingStats.FollowUpType -= deBuffingValue;
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
