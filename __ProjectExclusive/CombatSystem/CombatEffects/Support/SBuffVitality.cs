using System;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "VITALITY - N [Buff]",
        menuName = "Combat/Effect/Buff Vitality", order = 100)]
    public class SBuffVitality : SBuff
    {
        [Title("Params")] 
        [SerializeField] private EnumStats.VitalityStatType buffStat;
        public EnumStats.VitalityStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(
            SkillValuesHolders values, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(values, buffValue, isCritical);

            switch (buffStat)
            {
                case EnumStats.VitalityStatType.MaxHealth:
                    targetStats.MaxHealth += finalBuffValue;
                    break;
                case EnumStats.VitalityStatType.MaxMortality:
                    targetStats.MaxMortality += finalBuffValue;
                    break;
                case EnumStats.VitalityStatType.DebuffResistance:
                    targetStats.DebuffResistance += finalBuffValue;
                    break;
                case EnumStats.VitalityStatType.DamageResistance:
                    targetStats.DamageResistance += finalBuffValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SkillComponentResolution(this,finalBuffValue);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "VITALITY - " + buffStat.ToString() + " [Buff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
