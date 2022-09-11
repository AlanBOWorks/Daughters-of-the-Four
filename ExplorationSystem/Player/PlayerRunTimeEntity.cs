using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class PlayerRunTimeEntity : 
        ICombatEntityProvider, 
        IStanceStructureRead<IReadOnlyCollection<IFullSkill>>,
        IDamageableStats<float>,
        IVitalityStatsRead<float>
    {
        public PlayerRunTimeEntity(ICombatEntityProvider preset)
        {
            Preset = preset;
            _statsBase = new StatsBase<float>(preset.GetBaseStats());

            var presetSkills = preset.GetPresetSkills();
            AttackingSkills = new List<IFullSkill>(presetSkills.AttackingStance);
            SupportSkills = new List<IFullSkill>(presetSkills.SupportingStance);
            DefendingSkills = new List<IFullSkill>(presetSkills.DefendingStance);

            CurrentHealth = HealthType;
            CurrentMortality = MortalityType;
            CurrentShields = 0;
        }

        [ShowInInspector]
        public readonly ICombatEntityProvider Preset;

        [TabGroup("Stats"), ShowInInspector]
        private readonly StatsBase<float> _statsBase;

        public IBasicStatsRead<float> GetBaseStats() => _statsBase;
        public TeamAreaData GetAreaData() => Preset.GetAreaData();

        public string GetProviderEntityName() => Preset.GetProviderEntityName();

        public GameObject GetVisualPrefab() => Preset.GetVisualPrefab();

        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => this;

        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> AttackingSkills;
        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> SupportSkills;
        [TabGroup("Skills"), ShowInInspector] public readonly List<IFullSkill> DefendingSkills;

        public IReadOnlyCollection<IFullSkill> AttackingStance => AttackingSkills;
        public IReadOnlyCollection<IFullSkill> SupportingStance => SupportSkills;
        public IReadOnlyCollection<IFullSkill> DefendingStance => DefendingSkills;



        public float CurrentHealth { get; set; }
        public float CurrentMortality { get; set; }
        public float CurrentShields { get; set; }
        public float HealthType => _statsBase.HealthType;
        public float MortalityType => _statsBase.MortalityType;
        public float DamageReductionType => _statsBase.DamageReductionType;
        public float DeBuffResistanceType => _statsBase.DeBuffResistanceType;
    }

    public sealed class PlayerRunTimeTeam : ITeamFlexStructureRead<PlayerRunTimeEntity>,
        ICombatTeamProvider
    {
        public PlayerRunTimeTeam(ITeamFlexStructureRead<ICombatEntityProviderHolder> team)
        {
            VanguardType = HandleInstantiation(team.VanguardType.GetEntityProvider());
            AttackerType = HandleInstantiation(team.AttackerType.GetEntityProvider());
            SupportType = HandleInstantiation(team.SupportType.GetEntityProvider());
            FlexType = HandleInstantiation(team.FlexType.GetEntityProvider());
        }


        [ShowInInspector, EnableIf("VanguardType"), HorizontalGroup("FrontLine")]
        public PlayerRunTimeEntity VanguardType { get; }
        [ShowInInspector, EnableIf("AttackerType"), HorizontalGroup("FrontLine")]
        public PlayerRunTimeEntity AttackerType { get; }
        [ShowInInspector, EnableIf("SupportType"), HorizontalGroup("BackLine")]
        public PlayerRunTimeEntity SupportType { get; }
        [ShowInInspector, EnableIf("FlexType"), HorizontalGroup("BackLine")]
        public PlayerRunTimeEntity FlexType { get; }
        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters()
        {
            yield return VanguardType;
            yield return AttackerType;
            yield return SupportType;
            yield return FlexType;
        }

        public int MembersCount { get; private set; }


        private PlayerRunTimeEntity HandleInstantiation(ICombatEntityProvider preset)
        {
            if (preset == null)
                return null;

            MembersCount++;
            return new PlayerRunTimeEntity(preset);
        }
    }
}
