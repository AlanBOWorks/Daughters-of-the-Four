using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Skills.Presets;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Enemy] Preparation",
        fileName = "N " + AssetPrefixName)]
    public class SEnemyPreparationEntity : SPreparationEntityBase
    {
        public const string AssetPrefixName = "[ENEMY Preparation Entity]";
        private const string NullName = "NULL";
        public const string EnemiesAssetPathFolder = SPreparationEntity.AssetPathFolderRoot + "/Enemies/";

        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
         AssetSelector(Paths = EnemiesAssetPathFolder)]
        private GameObject instantiationObject;

        [Title("Names")]
        [SerializeField] private string entityName = NullName;



        [Title("Positioning")]
        [SerializeField]
        private TeamAreaData areaData;

        [Title("Stats")]
        [SerializeReference]
        private IBasicStats stats = new EnemyEntityBasicStats();
        public override IBasicStatsRead<float> GetBaseStats() => stats;
        public IBasicStats GetStats() => stats;

        private bool IsDefaultStats()
        {
            return stats is EnemyEntityBasicStats;
        }

        [Title("Skills")]
        [SerializeReference] 
        private IEntitySkills skillsHolder = new SkillsHolder();

        public override TeamAreaData GetAreaData() => areaData;

        public override string GetProviderEntityName() => entityName;
        public override GameObject GetVisualPrefab() => instantiationObject;
        public override IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => skillsHolder;

        [Serializable]
        private sealed class EnemyEntityBasicStats : ReferencedMainStatsBase
        {
            public EnemyEntityBasicStats() : base(Stats.OffensiveStats.One, Stats.SupportStats.One, 
                new VitalityStats(10,500,0), new ConcentrationStats(1))
            { }

#if UNITY_EDITOR
            protected override bool DisableOffensive() => true;
            protected override bool DisableSupport() => true;
#endif

        }
    }
}
