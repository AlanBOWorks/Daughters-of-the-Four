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
    public abstract class SPreparationEntity : ScriptableObject, ICombatEntityProvider
    {
        protected const string AssetPathFolderRoot = "Assets/Prefabs/Characters";
        [SerializeField]
        private PreparationEntity preparationData = new PreparationEntity();
        internal PreparationEntity GetPreparationEntity() => preparationData;


        public abstract string GetProviderEntityName();
        public abstract string GetProviderEntityFullName();
        public abstract string GetProviderShorterName();

        public string GetLocalizableCharactersName() => GetProviderEntityName();

        public abstract GameObject GetVisualPrefab();

        public IStatsRead<float> GetBaseStats() => preparationData.GetBaseStats();
        public TeamAreaData GetAreaData() => preparationData.GetAreaData();
        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => preparationData.GetPresetSkills();

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


        public IStatsRead<float> GetBaseStats() => baseStats;
        public TeamAreaData GetAreaData() => Team.UtilsTeam.GenerateAreaData(role);
        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => skills;


        [Serializable]
        private class BaseStats : Stats<float>
        {
            public BaseStats() : base(1,0)
            {
                
            }
        }
        [Serializable]
        private sealed class EntitySkills : IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
        {
            [SerializeField] private SSkillPreset[] attackingSkills;
            [SerializeField] private SSkillPreset[] neutralSkills;
            [SerializeField] private SSkillPreset[] defendingSkills;


            public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills;
            public IReadOnlyCollection<IFullSkill> NeutralStance => neutralSkills;
            public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills;
        }
    }
}
