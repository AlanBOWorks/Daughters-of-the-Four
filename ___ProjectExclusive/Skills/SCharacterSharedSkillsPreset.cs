using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Skills that can be used regardless the [<seealso cref="ISkillPositions{T}"/>]
    /// of the Character
    /// </summary>
    [CreateAssetMenu(fileName = "Skills SHARED - N [Set]",
        menuName = "Combat/Skill/[Global] Skills Shared Preset", order = 100)]
    public class SCharacterSharedSkillsPreset : ScriptableObject, ISharedSkillsSet<SkillPreset>
    {
        [Title("Params")] 
        [SerializeField] 
        private string setName = "NULL";
        [SerializeField]
        private CharacterArchetypes.RangeType rangeType = CharacterArchetypes.RangeType.Hybrid;

        [SerializeField] private CharacterArchetypes.TeamPosition targetPosition 
            = CharacterArchetypes.TeamPosition.All; 

        [Title("Common")]
        [SerializeField] private SkillPreset ultimateSkill;
        [SerializeField] private SkillPreset waitSkill;
        [Title("Positioned")]
        [SerializeField] private SharedSkills attackingSkills = new SharedSkills();
        [SerializeField] private SharedSkills neutralSkills = new SharedSkills();
        [SerializeField] private SharedSkills defendingSkills = new SharedSkills();

        public CharacterArchetypes.RangeType GetRangeType() => rangeType;
        public CharacterArchetypes.TeamPosition GetTargetPosition() => targetPosition;

        public SkillPreset UltimateSkill => ultimateSkill;
        public SkillPreset WaitSkill => waitSkill;

        public ISharedSkills<SkillPreset> AttackingSkills => attackingSkills;
        public ISharedSkills<SkillPreset> NeutralSkills => neutralSkills;
        public ISharedSkills<SkillPreset> DefendingSkills => defendingSkills;

        [Button(ButtonSizes.Large), GUIColor(.3f,.6f,1f)]
        private void UpdateAssetName()
        {
            name = $"Skills (SHARED) {targetPosition} {rangeType.ToString().ToUpper()} - {setName} [Set]"; 

            UtilsGame.UpdateAssetName(this);
        }
    }

    [Serializable]
    public class SharedSkills : ISharedSkills<SkillPreset>
    {
        [SerializeField] private SkillPreset commonSkillFirst;
        [SerializeField] private SkillPreset commonSkillSecondary;

        public SkillPreset CommonSkillFirst => commonSkillFirst;
        public SkillPreset CommonSkillSecondary => commonSkillSecondary;
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

                CommonSkillFirst = new CombatSkill(presets.CommonSkillFirst);
                CommonSkillSecondary = new CombatSkill(presets.CommonSkillSecondary);
            }

            public CombatSkill CommonSkillFirst { get; }
            public CombatSkill CommonSkillSecondary { get; }
        }
    }
}
