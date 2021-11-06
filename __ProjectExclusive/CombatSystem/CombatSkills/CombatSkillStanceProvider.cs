using System;
using System.Collections.Generic;
using CombatTeam;
using UnityEngine;

namespace CombatSkills
{
    [Serializable]
    public class CombatSkillStanceProvider : ITeamStanceStructureRead<ICollection<SkillProviderParams>>
    {
        [SerializeField] private SkillProviderParams[] attackingSkills = new SkillProviderParams[0];
        [SerializeField] private SkillProviderParams[] neutralSkills = new SkillProviderParams[0];
        [SerializeField] private SkillProviderParams[] defendingSkills = new SkillProviderParams[0];

        public ICollection<SkillProviderParams> OnAttackStance => attackingSkills;
        public ICollection<SkillProviderParams> OnNeutralStance => neutralSkills;
        public ICollection<SkillProviderParams> OnDefenseStance => defendingSkills;
    }

    [Serializable]
    public struct SkillProviderParams
    {
        public SSkill preset;
        public bool isControlSkill;
    }
}
