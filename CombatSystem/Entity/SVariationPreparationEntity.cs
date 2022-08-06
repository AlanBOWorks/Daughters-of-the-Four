using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Enemy] Preparation - Variant",
        fileName = "N " + AssetPrefixName)]
    public class SVariationPreparationEntity : SPreparationEntityBase, IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
    {
        private const string AssetPrefixName = SEnemyPreparationEntity.AssetPrefixName + "- VARIANT";

        [Title("Main Reference")]
        [SerializeField, InfoBox("NULL reference", InfoMessageType.Error, "IsReferenceInValid")] 
        private SEnemyPreparationEntity presetReference;

        private bool IsReferenceInValid() => !presetReference;

        [Title("Prefabs")]
        [SerializeField, AssetsOnly, PreviewField(ObjectFieldAlignment.Left),
         AssetSelector(Paths = SEnemyPreparationEntity.EnemiesAssetPathFolder)]
        private GameObject instantiationObject;

        private const string NullName = "Tp. B";
        [Title("Names")]
        [SerializeField] private string variationName = NullName;

        private string GenerateEntityName (string referencedName) => referencedName + " - " + variationName;
        public override string GetProviderEntityName() => GenerateEntityName(presetReference.GetProviderEntityName());
        public override string GetProviderEntityFullName() => GenerateEntityName(presetReference.GetProviderEntityFullName());
        public override string GetProviderShorterName() => GenerateEntityName(presetReference.GetProviderShorterName());
        public override GameObject GetVisualPrefab() => instantiationObject ? instantiationObject : presetReference.GetVisualPrefab();

        [Title("Stats")]
        [SerializeReference] private IBasicStats stats = null;

        public override IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => this;
        public override IBasicStatsRead<float> GetBaseStats() => stats ?? presetReference.GetBaseStats();
        public override TeamAreaData GetAreaData() => presetReference.GetAreaData();


        [Title("Skills")]
        [SerializeField] private SSkillPresetBase[] attackingSkills = new SSkillPresetBase[0];
        public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills;

        [SerializeField] private SSkillPresetBase[] supportingSkills = new SSkillPresetBase[0];
        public IReadOnlyCollection<IFullSkill> SupportingStance => supportingSkills;

        [SerializeField] private SSkillPresetBase[] defendingSkills = new SSkillPresetBase[0];
        public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills;


        [Button, ShowIf("IsReferenceInValid")]
        private void InjectReference(SEnemyPreparationEntity entity)
        {
            presetReference = entity;
        }

        [Button, ShowIf("presetReference")]
        private void InjectFromReference()
        {
            if (stats == null) stats = presetReference.GetStats();
            else if (stats is ReferencedMainStatsBase referencedMainStats)
            {
                HandleAsReferencedMainStats(referencedMainStats, referencedMainStats); 
            }
        }

        private void HandleAsReferencedMainStats(ReferencedMainStatsBase selfStats, IBasicStats referenced)
        {
            if (selfStats.OffensiveStats == null) selfStats.OffensiveStats = referenced;
            if (selfStats.SupportStats == null) selfStats.SupportStats = referenced;
            if (selfStats.VitalityStats == null) selfStats.VitalityStats = referenced;
            if (selfStats.ConcentrationStats == null) selfStats.ConcentrationStats = referenced;
        }
    }
}
