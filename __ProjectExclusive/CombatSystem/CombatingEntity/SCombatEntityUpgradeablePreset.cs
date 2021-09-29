using System;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace CombatEntity
{
    public class SCombatEntityUpgradeablePreset : SCombatEntityPreset
    {
        [SerializeField, HorizontalGroup("Stats"), Tooltip("Stats which the character gains each upgrade")]
        private BaseStats growStats = new BaseStats();
        public IBaseStatsRead<float> GrowStats => growStats;

        public void DoReflection(ICombatEntityUpgradableReflection reflection)
        {
            reflection.BaseStats = baseStats;
            reflection.GrowStats = growStats;
            reflection.AreaDataHolder = areaData;
        }
    }

    public interface ICombatEntityUpgradableReflection : ICombatEntityReflection
    {
        BaseStats GrowStats { set; }
    }
}
