using System.Collections.Generic;
using CombatTeam;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public class CombatEntitySkillsHolder : ITeamStanceStructure<CombatingSkillsGroup>
    {
        public CombatEntitySkillsHolder(ITeamStanceStructureRead<ICollection<SkillProviderParams>> mainSkills, EnumTeam.Role role)
        {
            // Make static injection by role and range
            UtilsTeam.InjectElements(this,mainSkills, SkillsGroupDeclaration);


            CombatingSkillsGroup SkillsGroupDeclaration(ICollection<SkillProviderParams> skills)
            {
                return new CombatingSkillsGroup(skills);
            }
        }


        public CombatingSkillsGroup OnAttackStance { get; set; }
        public CombatingSkillsGroup OnNeutralStance { get; set; }
        public CombatingSkillsGroup OnDefenseStance { get; set; }
    }

    public class CombatingSkillsGroup : ISkillGroupTypesRead<CombatingSkillsList>
    {
        public CombatingSkillsGroup()
        {
            SharedSkillTypes = new CombatingSkillsList();
            MainSkillTypes = new CombatingSkillsList();
        }
        public CombatingSkillsGroup(ICollection<SkillProviderParams> mainSkillPresets)
        {
            SharedSkillTypes = new CombatingSkillsList();
            MainSkillTypes = new CombatingSkillsList(mainSkillPresets);
        }


        public CombatingSkillsGroup(ISkillGroupTypesRead<ICollection<SkillProviderParams>> presets)
        {
            SharedSkillTypes = new CombatingSkillsList(presets.SharedSkillTypes);
            MainSkillTypes = new CombatingSkillsList(presets.MainSkillTypes);
        }

        public CombatingSkillsList SharedSkillTypes { get; }
        public CombatingSkillsList MainSkillTypes { get; }
    }

    public class CombatingSkillsList : List<CombatingSkill>
    {
        public CombatingSkillsList() : base() { }

        public CombatingSkillsList(ICollection<SkillProviderParams> presets)
        : base(presets.Count)
        {
            foreach (SkillProviderParams preset in presets)
            {
                Add(new CombatingSkill(preset));
            }
        }

        public void TickCooldown()
        {
            foreach (var skill in this)
            {
                skill.TickCooldown();
            }
        }

        public void RefreshCooldown()
        {
            foreach (CombatingSkill skill in this)
            {
                skill.TickRefresh();
            }
        }
    }
}
