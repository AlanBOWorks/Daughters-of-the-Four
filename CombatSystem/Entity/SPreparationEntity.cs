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
    [CreateAssetMenu(menuName = "Combat/Entities/[Basic] Preparation",
        fileName = "N " + AssetPrefixName)]
    public class SPreparationEntity : ScriptableObject, ICombatEntityProvider
    {
        [Title("Info")]
        [SerializeField] private string entityName = "NULL";

        [SerializeField]
        private PreparationEntity preparationData = new PreparationEntity();
        internal PreparationEntity GetPreparationEntity() => preparationData;


        public string GetEntityName() => entityName;

        public IStatsRead<float> GetBaseStats() => preparationData.GetBaseStats();
        public TeamAreaData GetAreaData() => preparationData.GetAreaData();
        public IStanceStructureRead<IReadOnlyCollection<ISkill>> GetPresetSkills() => preparationData.GetPresetSkills();

        protected virtual string AssetPrefix() => AssetPrefixName;
        private const string AssetPrefixName = "[BASIC Preparation Entity]";

        [Button]
        private void UpdateAssetWithRoleAndID()
        {
            string finalName = entityName;
            var areaData = GetAreaData();
            finalName += $" - {areaData.RoleType.ToString().ToUpper()} ";
            finalName += AssetPrefix();

            UtilsAssets.UpdateAssetNameWithID(this,finalName);
        }
    }

    [Serializable]
    internal sealed class PreparationEntity : ICombatEntityPreparation
    {
        

        [Title("Stats")]
        [SerializeField]
        private BaseStats baseStats = new BaseStats();

        [Title("Skills")] 
        [SerializeField] private EntitySkills skills = new EntitySkills();


        [TitleGroup("Team related"), PropertyOrder(-4)]
        [SerializeField]
        private EnumTeam.Role role;


        public IStatsRead<float> GetBaseStats() => baseStats;
        public TeamAreaData GetAreaData() => Team.UtilsTeam.GenerateAreaData(role);
        public IStanceStructureRead<IReadOnlyCollection<ISkill>> GetPresetSkills() => skills;


        [Serializable]
        private class BaseStats : Stats<float>
        {
            public BaseStats() : base(1,0)
            {
                
            }
        }
        [Serializable]
        private sealed class EntitySkills : IStanceStructureRead<IReadOnlyCollection<ISkill>>
        {
            [SerializeField] private SSkillPreset[] attackingSkills;
            [SerializeField] private SSkillPreset[] neutralSkills;
            [SerializeField] private SSkillPreset[] defendingSkills;


            public IReadOnlyCollection<ISkill> AttackingStance => attackingSkills;
            public IReadOnlyCollection<ISkill> NeutralStance => neutralSkills;
            public IReadOnlyCollection<ISkill> DefendingStance => defendingSkills;
        }
    }
}
