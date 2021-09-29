using System.Collections.Generic;
using CombatTeam;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public class CombatEntitySkillsHolder : ITeamStanceStructure<CombatingSkillsGroup>
    {
        public CombatEntitySkillsHolder(EnumTeam.Role role, EnumStats.RangeType rangeType)
        {
            // Make static injection by role and range
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
        public CombatingSkillsGroup(ICollection<ISkill> mainSkillPresets)
        {
            SharedSkillTypes = new CombatingSkillsList();
            MainSkillTypes = new CombatingSkillsList(mainSkillPresets);
        }
        public CombatingSkillsGroup(ISkillGroupTypesRead<ICollection<ISkill>> presets)
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

        public CombatingSkillsList(ICollection<ISkill> presets)
        : base(presets.Count)
        {
            foreach (ISkill preset in presets)
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
