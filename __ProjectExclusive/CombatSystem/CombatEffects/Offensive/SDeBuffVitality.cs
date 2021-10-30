using System;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "De-VITALITY - N [DeBuff]",
        menuName = "Combat/Effect/DE-Buff Vitality", order = 100)]
    public class SDeBuffVitality : SDeBuff
    {
        [Title("Params")]
        [SerializeField] private EnumStats.VitalityStatType buffStat;
        public EnumStats.VitalityStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(
            CombatingEntity performer, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(performer, buffValue, isCritical);

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
            return new SkillComponentResolution(this, finalBuffValue);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "De-VITALITY - " + buffStat.ToString() + " [DeBuff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
