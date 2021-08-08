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
        menuName = "Combat/Skill/[Global] Skills Shared Preset", order = 100)]
    public class SCharacterSharedSkillsPreset : ScriptableObject, ISkillShared<SkillPreset>
    {
        [SerializeField] private SkillPreset ultimateSkill;
        [SerializeField] private SkillPreset commonSkillFirst;
        [SerializeField] private SkillPreset commonSkillSecondary;
        [SerializeField] private SkillPreset waitSkill;

        public SkillPreset UltimateSkill => ultimateSkill;
        public SkillPreset CommonSkillFirst => commonSkillFirst;
        public SkillPreset CommonSkillSecondary => commonSkillSecondary;
        public SkillPreset WaitSkill => waitSkill;

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

        private int _amount;
        public int GetSharedAmount() => _amount;

        public SharedCombatSkills(ISkillShared<SkillPreset> skills)
        {
            UltimateSkill = GenerateSkill(skills.UltimateSkill, true);
            CommonSkillFirst = GenerateSkill(skills.CommonSkillFirst,false);
            CommonSkillSecondary = GenerateSkill(skills.CommonSkillSecondary, false);
            WaitSkill = GenerateSkill(skills.WaitSkill, false);
        }

        public SharedCombatSkills(SCharacterSharedSkillsPreset variableSkills)
        : this(variableSkills as ISkillShared<SkillPreset>)
        { }

        private CombatSkill GenerateSkill(SkillPreset skill, bool isInCooldown)
        {
            if (skill is null)
                return null;

            _amount++;
            var generatedSkill = new CombatSkill(skill, isInCooldown);
            Add(generatedSkill);
            return generatedSkill;
        }

        public List<CombatSkill> AllSkills => this;
    }
}
