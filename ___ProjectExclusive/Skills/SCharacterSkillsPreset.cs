using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Skills - N [Preset]",
        menuName = "Combat/Skills Preset")]
    public class SCharacterSkillsPreset : ScriptableObject, ISkillPositions<List<Skill>>
    {
        [SerializeField] private List<Skill> attackingSkills;
        [SerializeField] private List<Skill> neutralSkills;
        [SerializeField] private List<Skill> defendingSkills;
        [SerializeField] private SCharacterSharedSkillsPreset sharedSkills;

        public ISkillShared<Skill> SkillShared => sharedSkills;

        public List<Skill> AttackingSkills => attackingSkills;
        public List<Skill> NeutralSkills => neutralSkills;
        public List<Skill> DefendingSkills => defendingSkills;
    }

    public class PositionCombatSkills : ISkillPositions<List<CombatSkill>>
    {
        [ShowInInspector]
        public List<CombatSkill> AttackingSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> NeutralSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> DefendingSkills { get; }


        public PositionCombatSkills(ISkillPositions<List<Skill>> presets)
        {
            AttackingSkills = GenerateList(presets.AttackingSkills);
            NeutralSkills = GenerateList(presets.NeutralSkills);
            DefendingSkills = GenerateList(presets.DefendingSkills);

            List<CombatSkill> GenerateList(List<Skill> skills)
            {
                List<CombatSkill> generated = new List<CombatSkill>(skills.Count);
                foreach (Skill skill in skills)
                {
                    generated.Add(new CombatSkill(skill));
                }

                return generated;
            }
        }

    }

}
