using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using _Team;
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
        private EnumCharacter.RangeType rangeType = EnumCharacter.RangeType.HybridRange;

        [SerializeField] private EnumTeam.GroupPositioning targetPosition 
            = EnumTeam.GroupPositioning.FrontLine; 

        [Title("Common")]
        [SerializeField] private SkillPreset ultimateSkill;
        [SerializeField] private SkillPreset waitSkill;
        [Title("Positioned")]
        [SerializeField] private SharedSkills attackingSkills = new SharedSkills();
        [SerializeField] private SharedSkills neutralSkills = new SharedSkills();
        [SerializeField] private SharedSkills defendingSkills = new SharedSkills();

        public EnumCharacter.RangeType GetRangeType() => rangeType;
        public EnumTeam.GroupPositioning GetTargetPosition() => targetPosition;

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
    public class SharedSkills : SharedSkillsBase<SkillPreset>
    {}

}
