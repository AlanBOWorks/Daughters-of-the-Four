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
        public const string AssetPathFolderRoot = "Assets/Prefabs/Characters";
        [SerializeField]
        private PreparationEntity preparationData = new PreparationEntity();
        internal PreparationEntity GetPreparationEntity() => preparationData;
        
        public string GetLocalizableCharactersName() => GetProviderEntityName();


        public override IBasicStatsRead<float> GetBaseStats() => preparationData.GetBaseStats();
        internal BaseStats GetStats() => preparationData.GetStats();
        public override TeamAreaData GetAreaData() => preparationData.GetAreaData();
        public override IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => preparationData.GetPresetSkills();


        
    }

    public abstract class SPreparationEntityBase : ScriptableObject, ICombatEntityProvider
    {
#if UNITY_EDITOR
        [InfoBox("Offensive: %; %; %; %; \n" +
                 "Support: %; %; %; %;\n" +
                 "Vitality: u; u; %; %;\n " +
                 "Concentration: u; u; %; %;", 
            "_showStatsTooltips")]
        [ShowInInspector, NonSerialized]
        private bool _showStatsTooltips; 
#endif


        public abstract IBasicStatsRead<float> GetBaseStats();
        public abstract TeamAreaData GetAreaData();
        public abstract string GetProviderEntityName();
        public abstract GameObject GetVisualPrefab();
        public abstract IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills();

        protected virtual string AssetPrefix() => AssetPrefixName;
        private const string AssetPrefixName = " [BASIC Preparation Entity]";
        [Button]
        private void UpdateAssetWithRoleAndID()
        {
            string finalName = GetEntityFrontAssetName();

            finalName += GetProviderEntityName();
            finalName += AssetPrefix();

            UtilsAssets.UpdateAssetNameWithID(this, finalName);
        }

        protected virtual string GetEntityFrontAssetName()
        {
            var areaData = GetAreaData();
            return $"{areaData.RoleType.ToString().ToUpper()} - ";
        }


        protected interface IEntitySkills : IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
        { }

        [Serializable]
        protected sealed class SkillsHolder : IEntitySkills
        {
            [SerializeField] private SSkillPresetBase[] attackingSkills = new SSkillPresetBase[0];
            [SerializeField] private SSkillPresetBase[] supportingSkills = new SSkillPresetBase[0];
            [SerializeField] private SSkillPresetBase[] defendingSkills = new SSkillPresetBase[0];


            public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills;
            public IReadOnlyCollection<IFullSkill> SupportingStance => supportingSkills;
            public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills;
        }

        [Serializable]
        protected sealed class ReferencedSkillsHolder : IEntitySkills
        {
            [SerializeReference] private IEntitySkills attackingSkills;
            [SerializeReference] private IEntitySkills supportingSkills;
            [SerializeReference] private IEntitySkills defendingSkills;

            public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills.AttackingStance;
            public IReadOnlyCollection<IFullSkill> SupportingStance => supportingSkills.SupportingStance;
            public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills.DefendingStance;
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
