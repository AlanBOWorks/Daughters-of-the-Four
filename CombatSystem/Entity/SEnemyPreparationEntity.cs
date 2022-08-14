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
    public class SEnemyPreparationEntity : SPreparationEntityBase,
        IEnemyEntityVariationStructureRead<ICombatEntityPreparation>
    {
        public const string AssetPrefixName = " [ENEMY Preparation Entity]";
        private const string NullName = "NULL";
        public const string EnemiesAssetPathFolder = SPreparationEntity.AssetPathFolderRoot + "/Enemies/";

        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
         AssetSelector(Paths = EnemiesAssetPathFolder)]
        private GameObject instantiationObject;

        [Title("Names")]
        [SerializeField] 
        private SEnemyPreparationEntity variateFromPreset;
        [SerializeField, InfoBox("Name will be added to the end of the preset above", 
             "variateFromPreset", 
             InfoMessageType = InfoMessageType.Warning)] 
        private string entityName = NullName;




        [Title("Positioning")]
        [SerializeField]
        private TeamAreaData areaData;

        [Title("Stats")]
        [SerializeReference, InfoBox("Will use stats from Preset", "ShowStatsInfoBox")]
        [TabGroup("Stats")]
        private IBasicStats stats = new EnemyEntityBasicStats();
        public override IBasicStatsRead<float> GetBaseStats() => 
            stats ?? (variateFromPreset != null ? variateFromPreset.stats : null);
        public IBasicStats GetStats() => stats;
        private bool ShowStatsInfoBox() => stats == null && variateFromPreset;

        private bool IsDefaultStats()
        {
            return stats is EnemyEntityBasicStats;
        }

        
        [Title("Skills")]
        [SerializeReference, InfoBox("Will use skills from Preset", "ShowSkillInfoBox")] 
        [TabGroup("Skills")]
        private IEntitySkills skillsHolder = null;

        private bool ShowSkillInfoBox() => skillsHolder == null && variateFromPreset;

        public override TeamAreaData GetAreaData() => areaData;

        protected override string AssetPrefix() => AssetPrefixName;
        public override string GetProviderEntityName()
        {
            var targetName = (variateFromPreset) ? variateFromPreset.entityName + $" ({entityName})" : entityName;
            return targetName;
        }

        public override GameObject GetVisualPrefab() => instantiationObject;
        public override IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => 
            skillsHolder 
            ?? (variateFromPreset != null ? variateFromPreset.skillsHolder : null);

        [Serializable]
        private sealed class EnemyEntityBasicStats : ReferencedMainStatsBase
        {
            public EnemyEntityBasicStats() : base(null, null, 
                new VitalityStats(10,500,0), new ConcentrationStats(1))
            { }
        }


        [Serializable]
        private sealed class VariationEntity : ICombatEntityProvider
        {
            [SerializeField, DisableIf("deriveFrom")] 
            private SEnemyPreparationEntity deriveFrom;

            public VariationEntity(SEnemyPreparationEntity preset, int typeIndex)
            {
                deriveFrom = preset;
                switch (typeIndex)
                {
                    case 0:
                        derivationName = WeakDerivationName;
                        break;
                    case 1:
                        derivationName = CorruptedDerivationName;
                        break;
                    default:
                        derivationName = NullDerivationName + (typeIndex -2);
                        break;
                }
            }

            private const string WeakDerivationName = "Type W.";
            private const string CorruptedDerivationName = "Type C.";
            private const string NullDerivationName = "Type N.";
            [Title("Variations")] 
            [SerializeField] private string derivationName;
            [SerializeField] private GameObject overrideVisualPrefab;


            [TabGroup("Stats")]
            [SerializeReference, InfoBox("Will use the preset asset", "IsStatsNull")] 
            private IBasicStats stats;
            [TabGroup("Skill")]
            [SerializeReference, InfoBox("Will use the preset asset", "IsSkillsNull")] 
            private IEntitySkills skills;

#if UNITY_EDITOR
            private bool IsStatsNull() => stats == null;
            private bool IsSKillsNull() => skills == null;
#endif

            public IBasicStatsRead<float> GetBaseStats() => stats ?? deriveFrom.stats;
            public TeamAreaData GetAreaData() => deriveFrom.areaData;
            public string GetProviderEntityName() => deriveFrom.entityName + $" {derivationName}";
            public GameObject GetVisualPrefab() => 
                overrideVisualPrefab ? overrideVisualPrefab : deriveFrom.instantiationObject;
            public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() =>
                skills ?? deriveFrom.skillsHolder;
        }


        [SerializeField, ListDrawerSettings(HideAddButton = true, NumberOfItemsPerPage = 1),BoxGroup("Variations"), GUIColor(.8f,.9f,1f)]
        private List<VariationEntity> variationEntities = new List<VariationEntity>(0);

        [Button(ButtonSizes.Large), BoxGroup("Variations"), GUIColor(.7f, .8f, 1f)]
        private void AddVariationEntity()
        {
            var addingEntity = new VariationEntity(this, variationEntities.Count);
            variationEntities.Add(addingEntity);
        }

        public bool HasVariationEntities() => variationEntities.Count > 0;
        public ICombatEntityPreparation GetFirstVariationEntity() => variationEntities[0];
        public ICombatEntityPreparation WeakType => variationEntities.Count > 0 ? variationEntities[0] : null;
        public ICombatEntityPreparation CorruptedType => variationEntities.Count > 1 ? variationEntities[1] : WeakType;
    }

    public interface IEnemyEntityVariationStructureRead<out T>
    {
        T WeakType { get; }
        T CorruptedType { get; }
    }
}
