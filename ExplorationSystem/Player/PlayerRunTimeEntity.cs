using System;
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
        ICombatEntityProvider, IDamageableStats<float>,
        IVitalityStatsRead<float>
    {
        public PlayerRunTimeEntity(ICombatEntityProvider preset)
        {
            Preset = preset;
            _statsBase = new StatsBase<float>(preset.GetBaseStats());

            var presetSkills = preset.GetPresetSkills();
            _basicSkillsHolder = new SkillsHolder(presetSkills);
            _craftedSkillsHolder = new SkillsHolder();

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

        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => _basicSkillsHolder;
        public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetCraftedSkills() => _craftedSkillsHolder;

        internal IStanceStructureRead<IList<IFullSkill>> GetBasicsSkillsHolder() => _basicSkillsHolder;
        internal IStanceStructureRead<IList<IFullSkill>> GetCraftsSkillsHolder() => _basicSkillsHolder;

        [ShowInInspector] 
        private readonly SkillsHolder _basicSkillsHolder;
        private readonly SkillsHolder _craftedSkillsHolder;




        public float CurrentHealth { get; set; }
        public float CurrentMortality { get; set; }
        public float CurrentShields { get; set; }
        public float HealthType => _statsBase.HealthType;
        public float MortalityType => _statsBase.MortalityType;
        public float DamageReductionType => _statsBase.DamageReductionType;
        public float DeBuffResistanceType => _statsBase.DeBuffResistanceType;

        private sealed class SkillsHolder : IStanceStructureRead<List<IFullSkill>>
        {
            [ShowInInspector] public readonly List<IFullSkill> AttackingSkills;
            [ShowInInspector] public readonly List<IFullSkill> SupportSkills;
            [ShowInInspector] public readonly List<IFullSkill> DefendingSkills;

            public List<IFullSkill> AttackingStance => AttackingSkills;
            public List<IFullSkill> SupportingStance => SupportSkills;
            public List<IFullSkill> DefendingStance => DefendingSkills;

            private const int ListSize = 4;
            public SkillsHolder()
            {
                AttackingSkills = new List<IFullSkill>(ListSize);
                SupportSkills = new List<IFullSkill>(ListSize);
                DefendingSkills = new List<IFullSkill>(ListSize);
            }

            public SkillsHolder(IStanceStructureRead<IEnumerable<IFullSkill>> skillsHolder)
            {
                AttackingSkills = new List<IFullSkill>(skillsHolder.AttackingStance);
                SupportSkills = new List<IFullSkill>(skillsHolder.SupportingStance);
                DefendingSkills = new List<IFullSkill>(skillsHolder.DefendingStance);
            }
        }
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
