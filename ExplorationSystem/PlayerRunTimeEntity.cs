using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class PlayerRunTimeEntity : ICombatEntityProvider, IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
    {
        public PlayerRunTimeEntity(ICombatEntityProvider preset)
        {
            Preset = preset;
            _statsBase = new StatsBase<float>(preset.GetBaseStats());

            var presetSkills = preset.GetPresetSkills();
            AttackingSkills = new List<IFullSkill>(presetSkills.AttackingStance);
            SupportSkills = new List<IFullSkill>(presetSkills.SupportingStance);
            DefendingSkills = new List<IFullSkill>(presetSkills.DefendingStance);
        }

        [ShowInInspector]
        public readonly ICombatEntityProvider Preset;

        [TabGroup("Stats"), ShowInInspector]
        private readonly StatsBase<float> _statsBase;

        public IBasicStatsRead<float> GetBaseStats() => _statsBase;
        public TeamAreaData GetAreaData() => Preset.GetAreaData();

        public string GetProviderEntityName() => Preset.GetProviderEntityName();
        public string GetProviderEntityFullName() => Preset.GetProviderEntityFullName();
        public string GetProviderShorterName() => Preset.GetProviderShorterName();

        public GameObject GetVisualPrefab() => Preset.GetVisualPrefab();

        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => this;

        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> AttackingSkills;
        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> SupportSkills;
        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> DefendingSkills;

        public IReadOnlyCollection<IFullSkill> AttackingStance => AttackingSkills;
        public IReadOnlyCollection<IFullSkill> SupportingStance => SupportSkills;
        public IReadOnlyCollection<IFullSkill> DefendingStance => DefendingSkills;

    }
}
