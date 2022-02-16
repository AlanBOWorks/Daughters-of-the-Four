using System.Collections;
using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class CombatEntity : ITeamAreaDataRead, ICombatEntityInfoHolder, IStanceDataRead
    {
        public CombatEntity(ICombatEntityProvider preparationData)
        {

            Provider = preparationData;

            var baseStats = preparationData.GetBaseStats();
            var areaData = preparationData.GetAreaData();
            var skills = preparationData.GetPresetSkills();

            Stats = new CombatStats(baseStats);
            _skillsHolder = new SkillsHolder(skills);

            RoleType = areaData.RoleType;
            PositioningType = areaData.PositioningType;

        }
        public CombatEntity(ICombatEntityProvider preparationData, CombatTeam team) :
            this(preparationData)
        {
            InjectTeam(team);
        }

        [ShowInInspector, InlineEditor()] 
        public readonly ICombatEntityProvider Provider;
        public GameObject InstantiationReference;

        [ShowInInspector]
        public readonly CombatStats Stats;

        [ShowInInspector]
        private readonly SkillsHolder _skillsHolder;
       


        [ShowInInspector]
        public EnumTeam.Role RoleType { get; }
        [ShowInInspector]
        public EnumTeam.Positioning PositioningType { get; }

        [ShowInInspector]
        public CombatTeam Team { get; private set; }

        private IStanceDataRead _teamStanceRead;



        public string GetEntityName() => Provider.GetEntityName();
        public IFullStanceStructureRead<IReadOnlyCollection<CombatSkill>> StanceSkills => _skillsHolder;
        public IReadOnlyCollection<CombatSkill> AllSkills => _skillsHolder;
        public EnumTeam.StanceFull CurrentStance => _teamStanceRead.CurrentStance;

        private void InjectTeam(CombatTeam team)
        {
            Team = team;
            _teamStanceRead = team;
            _skillsHolder.StanceData = team;
        }

        public void SwitchTeam(CombatTeam team)
        {
            Team.Remove(this);
            InjectTeam(team);
        }

        /// <summary>
        /// ReadOnly StanceSkills in the current [<seealso cref="EnumTeam.StanceFull"/>];<br></br>
        /// To modify the collection check [<seealso cref="SkillsHolder.GetCurrentSkills"/>]
        /// </summary>
        public IReadOnlyList<CombatSkill> GetCurrentSkills() 
            => _skillsHolder.GetCurrentSkills();

        // ----- CLASSES
        private sealed class SkillsHolder : IFullStanceStructureRead<List<CombatSkill>>, IReadOnlyCollection<CombatSkill>
        {
            public SkillsHolder(IStanceStructureRead<IReadOnlyCollection<IFullSkill>> skills)
            {
                DisruptionStance = new List<CombatSkill>();
                AttackingStance = GenerateSkills(skills.AttackingStance);
                NeutralStance = GenerateSkills(skills.NeutralStance);
                DefendingStance = GenerateSkills(skills.DefendingStance);

                AllSkills = new HashSet<CombatSkill>(AttackingStance);
                AddSkills(NeutralStance);
                AddSkills(DefendingStance);

                void AddSkills(IEnumerable<CombatSkill> stanceSkills)
                {
                    foreach (CombatSkill skill in stanceSkills)
                    {
                        AllSkills.Add(skill);
                    }
                }

                List<CombatSkill> GenerateSkills(IReadOnlyCollection<IFullSkill> presetSkills)
                {
                    List<CombatSkill> generatedSkills;
                    if (presetSkills == null)
                    {
                        generatedSkills = new List<CombatSkill>();
                    }
                    else
                    {
                        generatedSkills = new List<CombatSkill>(presetSkills.Count);
                        foreach (var preset in presetSkills)
                        {
                            CombatSkill skill = new CombatSkill(preset);
                            generatedSkills.Add(skill);
                        }
                    }
                    

                    return generatedSkills;
                }
            }

            public IStanceDataRead StanceData { set; private get; }

            private readonly HashSet<CombatSkill> AllSkills;

            [ShowInInspector]
            public List<CombatSkill> AttackingStance { get; }
            [ShowInInspector]
            public List<CombatSkill> NeutralStance { get; }
            [ShowInInspector]
            public List<CombatSkill> DefendingStance { get; }
            [ShowInInspector]
            public List<CombatSkill> DisruptionStance { get; }

            public List<CombatSkill> GetCurrentSkills()
            {
                return UtilsTeam.GetElement(StanceData, this);
            }


            public IEnumerator<CombatSkill> GetEnumerator() => AllSkills.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => AllSkills.Count;
        }

    }

    public interface ICombatEntityProvider : ICombatEntityPreparation, ICombatEntityInfoHolder
    {
        GameObject GetVisualPrefab();
        IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills();
    }

    public interface ICombatEntityPreparation
    {
        IStatsRead<float> GetBaseStats();
        TeamAreaData GetAreaData();
    }


    public interface ICombatEntityInfoHolder
    {
        string GetEntityName();
    }
}
