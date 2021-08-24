using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [Serializable]
    public class SharedSkillsBase<T> : ISharedSkills<T>
    {
        [SerializeField] private T commonSkillFirst;
        [SerializeField] private T commonSkillSecondary;

        public T CommonSkillFirst => commonSkillFirst;

        public T CommonSkillSecondary => commonSkillSecondary;
    }
    [Serializable]
    public class RangesSkills<T> : ICharacterRangesData<T>
    {
        [SerializeField] private T meleeRange;
        [SerializeField] private T rangedRange;

        public T MeleeRange => meleeRange;

        public T RangedRange => rangedRange;
    }


    public class SharedCombatSkills : List<CombatSkill>, ISharedSkillsSet<CombatSkill>
    {
        [ShowInInspector]
        public CombatSkill UltimateSkill { get; }
        public CombatSkill WaitSkill { get; }

        [ShowInInspector]
        public ISharedSkills<CombatSkill> AttackingSkills { get; }
        [ShowInInspector]
        public ISharedSkills<CombatSkill> NeutralSkills { get; }
        [ShowInInspector]
        public ISharedSkills<CombatSkill> DefendingSkills { get; }


        public SharedCombatSkills(ISharedSkillsSet<SkillPreset> skills)
        {
            UltimateSkill = GenerateSkill(skills.UltimateSkill, true);
            WaitSkill = GenerateSkill(skills.WaitSkill, false);

            AttackingSkills = new CombatSharedSkills(skills.AttackingSkills);
            NeutralSkills = new CombatSharedSkills(skills.NeutralSkills);
            DefendingSkills = new CombatSharedSkills(skills.DefendingSkills);
        }


        private CombatSkill GenerateSkill(SkillPreset skill, bool isInCooldown)
        {
            if (skill == null || skill.Preset == null)
                return null;

            var generatedSkill = new CombatSkill(skill, isInCooldown);
            Add(generatedSkill);
            return generatedSkill;
        }

        public List<CombatSkill> AllSkills => this;

        private class CombatSharedSkills : ISharedSkills<CombatSkill>
        {
            public CombatSharedSkills(ISharedSkills<SkillPreset> presets)
            {
                var firstSkill = presets.CommonSkillFirst;
                var secondary = presets.CommonSkillSecondary;

                if (firstSkill != null && firstSkill.Preset != null)
                    CommonSkillFirst = new CombatSkill(firstSkill);
                if (secondary != null && secondary.Preset != null)
                    CommonSkillSecondary = new CombatSkill(secondary);
            }

            public CombatSkill CommonSkillFirst { get; }
            public CombatSkill CommonSkillSecondary { get; }
        }
    }
}
