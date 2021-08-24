using System;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "SHARED - N [Skill Set]", 
        menuName = "Combat/Skill/[Set] Shared Skills")]
    public class SSharedSkillsSet : ScriptableObject, ISharedSkillsInPosition<SkillPreset>
    {
        [SerializeField] private SharedSkillPresets attackingSkills;
        [SerializeField] private SharedSkillPresets neutralSkills;
        [SerializeField] private SharedSkillPresets defendingSkills;

        public ISharedSkills<SkillPreset> AttackingSkills => attackingSkills;
        public ISharedSkills<SkillPreset> NeutralSkills => neutralSkills;
        public ISharedSkills<SkillPreset> DefendingSkills => defendingSkills;

        [Serializable]
        private class SharedSkillPresets : SharedSkillsBase<SkillPreset>
        { }
    }

    
}
