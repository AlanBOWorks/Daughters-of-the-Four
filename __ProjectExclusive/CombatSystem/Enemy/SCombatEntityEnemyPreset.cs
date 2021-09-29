using CombatEntity;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace __ProjectExclusive.Enemy
{
    [CreateAssetMenu(fileName = "Role - N [Enemy Preset]", 
        menuName = "Combat/Entity/Enemy Preset")]
    public class SCombatEntityEnemyPreset : SCombatEntityPreset, ICombatEntityInfo, ICombatEntityProvider
    {
        [Title("Info"), PropertyOrder(-10)] 
        [SerializeField]
        private string entityName = "NULL";

        public string GetEntityName() => entityName;

        [Button, GUIColor(.7f,.8f,1)]
        private void UpdateAssetName()
        {
            UpdateAssetName(entityName);
        }

        public CombatStatsHolder GenerateStatsHolder() => new CombatStatsHolder(baseStats);

        public CombatingAreaData GenerateAreaData() => new CombatingAreaData(areaData);
    }
}
