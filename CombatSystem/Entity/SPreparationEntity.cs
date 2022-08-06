using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Entity
{
    public abstract class SPreparationEntity : SPreparationEntityBase
    {
        protected const string AssetPathFolderRoot = "Assets/Prefabs/Characters";
        [SerializeField]
        private PreparationEntity preparationData = new PreparationEntity();
        internal PreparationEntity GetPreparationEntity() => preparationData;
        
        public string GetLocalizableCharactersName() => GetProviderEntityName();


        public override IBasicStatsRead<float> GetBaseStats() => preparationData.GetBaseStats();
        internal BaseStats GetStats() => preparationData.GetStats();
        public override TeamAreaData GetAreaData() => preparationData.GetAreaData();
        public override IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => preparationData.GetPresetSkills();

        protected virtual string AssetPrefix() => AssetPrefixName;
        private const string AssetPrefixName = "[BASIC Preparation Entity]";

        [Button]
        private void UpdateAssetWithRoleAndID()
        {
            string finalName = GetProviderEntityName();
            var areaData = GetAreaData();
            finalName += $" - {areaData.RoleType.ToString().ToUpper()} ";
            finalName += AssetPrefix();

            UtilsAssets.UpdateAssetNameWithID(this,finalName);
        }
    }

    public abstract class SPreparationEntityBase : ScriptableObject, ICombatEntityProvider
    {
        public abstract IBasicStatsRead<float> GetBaseStats();
        public abstract TeamAreaData GetAreaData();
        public abstract string GetProviderEntityName();
        public abstract string GetProviderEntityFullName();
        public abstract string GetProviderShorterName();
        public abstract GameObject GetVisualPrefab();
        public abstract IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills();
    }

    [Serializable]
    internal sealed class PreparationEntity : ICombatEntityPreparation
    {
        [Title("Values")]
        [SerializeField]
        private BaseStats baseStats = new BaseStats();

        [Title("Skills")] 
        [SerializeField] private EntitySkills skills = new EntitySkills();


        [TitleGroup("Team related"), PropertyOrder(-4)]
        [SerializeField]
        private EnumTeam.Role role;


        public IBasicStatsRead<float> GetBaseStats() => baseStats;
        internal BaseStats GetStats() => baseStats;
        public TeamAreaData GetAreaData() => Team.UtilsTeam.GenerateAreaData(role);
        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => skills;


        
        [Serializable]
        private sealed class EntitySkills : IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
        {
            [SerializeField] private SSkillPresetBase[] attackingSkills;
            [SerializeField] private SSkillPresetBase[] neutralSkills;
            [SerializeField] private SSkillPresetBase[] defendingSkills;


            public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills;
            public IReadOnlyCollection<IFullSkill> SupportingStance => neutralSkills;
            public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills;
        }
    }
}
