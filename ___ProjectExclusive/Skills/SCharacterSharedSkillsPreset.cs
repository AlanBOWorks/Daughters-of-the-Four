using System;
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

        public Skill UltimateSkill => ultimateSkill;
        public Skill CommonSkillFirst => commonSkillFirst;
        public Skill CommonSkillSecondary => commonSkillSecondary;
    }

    public class SharedCombatSkills : ISkillShared<CombatSkill>
    {
        [ShowInInspector]
        public CombatSkill UltimateSkill { get; }
        [ShowInInspector]
        public CombatSkill CommonSkillFirst { get; }
        [ShowInInspector]
        public CombatSkill CommonSkillSecondary { get; }

        public SharedCombatSkills(ISkillShared<Skill> skills)
        {
            UltimateSkill = GenerateSkill(skills.UltimateSkill, true);
            CommonSkillFirst = GenerateSkill(skills.CommonSkillFirst,false);
            CommonSkillSecondary = GenerateSkill(skills.CommonSkillSecondary, false);
        }

        public SharedCombatSkills(SCharacterSharedSkillsPreset variableSkills)
        {
            UltimateSkill = GenerateSkill(variableSkills.UltimateSkill, true);
            CommonSkillFirst = GenerateSkill(variableSkills.CommonSkillFirst, false);
            CommonSkillSecondary = GenerateSkill(variableSkills.CommonSkillSecondary, false);
        }

        private CombatSkill GenerateSkill(Skill skill, bool isInCooldown)
        {
            return skill is null ? null : new CombatSkill(skill, isInCooldown);
        }


        public static void DoParse<TParse>(ISkillShared<CombatSkill> skills, ISkillShared<TParse> parsing,
            Action<CombatSkill,TParse> action)
        {
            action(skills.UltimateSkill, parsing.UltimateSkill);
            action(skills.CommonSkillFirst, parsing.CommonSkillFirst);
            action(skills.CommonSkillSecondary, parsing.CommonSkillSecondary);
        }
    }
}
