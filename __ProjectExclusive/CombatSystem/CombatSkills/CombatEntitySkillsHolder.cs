using System.Collections.Generic;
using CombatSystem;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public class CombatEntitySkillsHolder : ITeamStanceStructure<CombatingSkillsGroup>
    {
        public CombatEntitySkillsHolder()
        {
            var waitSkill = GlobalCombatParametersSingleton.Parameters.WaitSkill;
            WaitSkill = new CombatingSkill(waitSkill.GetSkillPreset());
        }

        public CombatEntitySkillsHolder(CombatingTeam team,
            ITeamStanceStructureRead<ICollection<SkillProviderParams>> mainSkills, EnumTeam.Role role)
        : this()
        {
            _team = team;
            UtilsTeam.InjectElements(this,mainSkills, SkillsGroupDeclaration);

            SSharedSkillSet globalSharedSkills =
                UtilsTeam.GetElement(GlobalCombatParametersSingleton.SharedSkills, role);
            if(globalSharedSkills != null) 
                UtilsTeam.DoActionOnTeam(this, globalSharedSkills, GenerateAndInjectSharedSkill);
            else Debug.LogWarning("NULL: Global Skills");



            CombatingSkillsGroup SkillsGroupDeclaration(ICollection<SkillProviderParams> skills)
            {
                return new CombatingSkillsGroup(skills);
            }
            void GenerateAndInjectSharedSkill(CombatingSkillsGroup skills, ICollection<SSkill> sharedSkills)
            {
                if(sharedSkills != null)
                    skills.SharedSkillTypes = new CombatingSkillsList(sharedSkills);
            }
        }

        public CombatingSkillsGroup GetCurrentSkills() 
            => UtilsTeam.GetElement(this, _team.CurrentStance);

        private readonly CombatingTeam _team;
        [ShowInInspector] 
        public readonly CombatingSkill WaitSkill;
        [ShowInInspector]
        public CombatingSkillsGroup OnAttackStance { get; set; }
        [ShowInInspector]
        public CombatingSkillsGroup OnNeutralStance { get; set; }
        [ShowInInspector]
        public CombatingSkillsGroup OnDefenseStance { get; set; }

        public void TickCoolDowns()
        {
            OnAttackStance.TickCoolDowns();
            OnNeutralStance.TickCoolDowns();
            OnDefenseStance.TickCoolDowns();
        }

    }

    public class CombatingSkillsGroup : ISkillGroupTypesRead<CombatingSkillsList>
    {
        public CombatingSkillsGroup(ICollection<SkillProviderParams> mainSkillPresets)
        {
            if(mainSkillPresets != null)
                MainSkillTypes = new CombatingSkillsList(mainSkillPresets);
        }
        public CombatingSkillsGroup(ICollection<SkillProviderParams> mainSkillPresets, ICollection<SSkill> sharedSkills)
        {
            if(sharedSkills != null)
                SharedSkillTypes = new CombatingSkillsList(sharedSkills);
            if(mainSkillPresets != null)
                MainSkillTypes = new CombatingSkillsList(mainSkillPresets);
        }



        [ShowInInspector,HorizontalGroup()]
        public CombatingSkillsList SharedSkillTypes { get; set; }
        [ShowInInspector,HorizontalGroup()]
        public CombatingSkillsList MainSkillTypes { get; }

        public void TickCoolDowns()
        {
            SharedSkillTypes?.TickCooldowns();
            MainSkillTypes?.TickCooldowns();
        }
    }

    public class CombatingSkillsList : List<CombatingSkill>
    {
        public CombatingSkillsList() : base() { }

        public CombatingSkillsList(ICollection<ISkill> presets) : base()
        {
            foreach (var preset in presets)
            {
                Add(new CombatingSkill(preset));
            }
        }
        public CombatingSkillsList(ICollection<SSkill> presets) : base()
        {
            foreach (var preset in presets)
            {
                Add(new CombatingSkill(preset.GetSkillPreset()));
            }
        }

        public CombatingSkillsList(ICollection<SkillProviderParams> presets)
        : base(presets.Count)
        {
            foreach (SkillProviderParams preset in presets)
            {
                Add(new CombatingSkill(preset));
            }
        }

        public void TickCooldowns()
        {
            foreach (var skill in this)
            {
                skill.TickCooldown();
            }
        }

        public void SilenceSkills()
        {
            foreach (var skill in this)
            {
                skill.Silence();
            }
        }
    }
}
