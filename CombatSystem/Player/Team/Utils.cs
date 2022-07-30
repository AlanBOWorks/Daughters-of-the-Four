using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public static class UtilsPlayerTeam
    {
        public static SelectedCharacterPair GenerateSelectionPair(ICombatEntityProvider preset)
        {
            var runTimeEntity = new RunTimeEntity(preset);
            return new SelectedCharacterPair(preset,runTimeEntity);
        }

        public static ICombatEntityProvider GenerateRunTimeEntity(ICombatEntityProvider preset)
        {
            return new RunTimeEntity(preset);
        }



        private sealed class RunTimeEntity : ICombatEntityProvider, IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
        {
            public RunTimeEntity(ICombatEntityProvider entity)
            {
                _preset = entity;
                var entitySkills = entity.GetPresetSkills();

                _runtimeStats = new StatsBase<float>(entity.GetBaseStats());

                _attackingSkills = GenerateSkillCollection(entitySkills.AttackingStance);
                _supportSkills = GenerateSkillCollection(entitySkills.SupportingStance);
                _defendSkills = GenerateSkillCollection(entitySkills.DefendingStance);
            }
            private static List<IFullSkill> GenerateSkillCollection(IEnumerable<IFullSkill> skills)
            {
                return new List<IFullSkill>(skills);
            }



            [ShowInInspector]
            private readonly ICombatEntityProvider _preset;
            [ShowInInspector]
            private readonly IBasicStats<float> _runtimeStats;

            public IBasicStatsRead<float> GetBaseStats() => _runtimeStats;
            public TeamAreaData GetAreaData() => _preset.GetAreaData();
            public string GetProviderEntityName() => _preset.GetProviderEntityName();
            public string GetProviderEntityFullName() => _preset.GetProviderEntityFullName();
            public string GetProviderShorterName() => _preset.GetProviderShorterName();
            public GameObject GetVisualPrefab() => _preset.GetVisualPrefab();
            public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => this;


            [Title("Skills")]
            [ShowInInspector]
            private readonly List<IFullSkill> _attackingSkills;
            [ShowInInspector]
            private readonly List<IFullSkill> _supportSkills;
            [ShowInInspector]
            private readonly List<IFullSkill> _defendSkills;

            public IReadOnlyCollection<IFullSkill> AttackingStance => _attackingSkills;
            public IReadOnlyCollection<IFullSkill> SupportingStance => _supportSkills;
            public IReadOnlyCollection<IFullSkill> DefendingStance => _defendSkills;
        }
    }


}