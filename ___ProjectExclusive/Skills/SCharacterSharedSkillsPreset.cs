using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Skills that can be used regardless the [<seealso cref="ISkillPositions{T}"/>]
    /// of the Character
    /// </summary>
    [CreateAssetMenu(fileName = "Skills Shared - N [Preset]",
        menuName = "Combat/Skills Shared Preset")]
    public class SCharacterSharedSkillsPreset : ScriptableObject, ISkillShared<Skill>
    {
        [SerializeField] private Skill ultimateSkill;
        [SerializeField] private Skill commonSkillFirst;
        [SerializeField] private Skill commonSkillSecondary;
        [SerializeField] private Skill waitSkill;

        public Skill UltimateSkill => ultimateSkill;
        public Skill CommonSkillFirst => commonSkillFirst;
        public Skill CommonSkillSecondary => commonSkillSecondary;
        public Skill WaitSkill => waitSkill;
    }

    public class SharedCombatSkills : List<CombatSkill>, IEquipSkill<CombatSkill>
    {
        [ShowInInspector]
        public CombatSkill UltimateSkill { get; }
        [ShowInInspector]
        public CombatSkill CommonSkillFirst { get; }
        [ShowInInspector]
        public CombatSkill CommonSkillSecondary { get; }

        public CombatSkill WaitSkill { get; }

        public SharedCombatSkills(ISkillShared<Skill> skills)
        {
            UltimateSkill = GenerateSkill(skills.UltimateSkill, true);
            CommonSkillFirst = GenerateSkill(skills.CommonSkillFirst,false);
            CommonSkillSecondary = GenerateSkill(skills.CommonSkillSecondary, false);
            WaitSkill = GenerateSkill(skills.WaitSkill, false);
        }

        public SharedCombatSkills(SCharacterSharedSkillsPreset variableSkills)
        : this(variableSkills as ISkillShared<Skill>)
        { }

        private CombatSkill GenerateSkill(Skill skill, bool isInCooldown)
        {
            if (skill is null)
                return null;

            var generatedSkill = new CombatSkill(skill, isInCooldown);
            Add(generatedSkill);
            return generatedSkill;
        }

        public List<CombatSkill> UniqueSkills => this;
    }
}
