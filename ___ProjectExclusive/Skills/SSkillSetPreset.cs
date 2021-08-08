using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N (T) - SKILL set [Preset]",
        menuName = "Combat/Skill/[Set] Skills Set Preset")]
    public class SSkillSetPreset : ScriptableObject
    {
        [Title("Params")]
        [SerializeField,Delayed]
        private string skillGroupName = "NULL";
        public string GetSkillGroupName() => skillGroupName;

        [SerializeField] private CharacterArchetypes.RoleArchetype archetype 
            = CharacterArchetypes.RoleArchetype.Attacker;
        public CharacterArchetypes.RoleArchetype GetTargetArchetype() => archetype;

        [SerializeField] private CharacterArchetypes.RangeType rangeType
            = CharacterArchetypes.RangeType.Hybrid;
        public CharacterArchetypes.RangeType GetRangeType() 
            => rangeType;

        [Title("Skills")]
        [SerializeField] private List<SkillPreset> skills = new List<SkillPreset>();
        public List<SkillPreset> GetSkills() => skills;


        private const string SkillGroupPrefix = " - Skill SET [Preset]";
        [Button(ButtonSizes.Large)]
        private void UpdateFileName()
        {
            name = skillGroupName.ToUpper() + " (" + archetype + ") - R_" 
                   +rangeType.ToString().ToUpper() + SkillGroupPrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
