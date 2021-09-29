using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEntity
{
    public abstract class SCombatEntityPreset : ScriptableObject
    {
        [Title("Area")] 
        [SerializeField]
        protected AreaData areaData = new AreaData();
        [SerializeField, HorizontalGroup("Stats"), Tooltip("Stats which the character starts with")]
        protected BaseStats baseStats = new BaseStats();

        public AreaData GetAreaData() => areaData;
        public IBaseStatsRead<float> BaseStats => baseStats;

        public void UpdateAssetName(string prefixName)
        {
            name 
                = $"{prefixName} - {areaData.GetRole()} ({areaData.GetRangeType().ToString().ToUpper()}) [Preset] - ID_" + GetInstanceID();
            UtilsAssets.UpdateAssetName(this);
        }
    }

    public interface ICombatEntityInfo
    {
        string GetEntityName();
    }

    public interface ICombatEntityReflection
    {
        AreaData AreaDataHolder { set; }
        BaseStats BaseStats { set; }
    }
}
