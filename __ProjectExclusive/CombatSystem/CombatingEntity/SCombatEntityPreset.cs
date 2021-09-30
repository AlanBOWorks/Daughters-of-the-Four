using System.Collections.Generic;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEntity
{
    public abstract class SCombatEntityPreset : ScriptableObject, ITeamStanceStructureRead<ICollection<SkillProviderParams>>
    {
        [Title("Area")] 
        [SerializeField]
        protected AreaData areaData = new AreaData();
        [SerializeField, HorizontalGroup("Stats"), Tooltip("Stats which the character starts with")]
        protected BaseStats baseStats = new BaseStats();

        [SerializeField, Title("Skills")] 
        private CombatSkillStanceProvider skillProvider = new CombatSkillStanceProvider();



        public AreaData GetAreaData() => areaData;
        public IBaseStatsRead<float> BaseStats => baseStats;

        public void UpdateAssetName(string prefixName)
        {
            name 
                = $"{prefixName} - {areaData.GetRole()} ({areaData.GetRangeType().ToString().ToUpper()}) [Preset] - ID_" + GetInstanceID();
            UtilsAssets.UpdateAssetName(this);
        }

        public ICollection<SkillProviderParams> OnAttackStance => skillProvider.OnAttackStance;
        public ICollection<SkillProviderParams> OnNeutralStance => skillProvider.OnNeutralStance;
        public ICollection<SkillProviderParams> OnDefenseStance => skillProvider.OnDefenseStance;

        public ITeamStanceStructureRead<ICollection<SkillProviderParams>> SkillProvider => skillProvider;
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
