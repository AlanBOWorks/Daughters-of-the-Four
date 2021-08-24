using System;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "SHARED - N [Skill Set]", 
        menuName = "Combat/Skill/[Set] Shared Skills")]
    public class SSharedSkillsSet : ScriptableObject, ISharedSkillsInPosition<SSkillPreset>
    {
        [SerializeField] private SharedSkillPresets attackingSkills;
        [SerializeField] private SharedSkillPresets neutralSkills;
        [SerializeField] private SharedSkillPresets defendingSkills;

        public ISharedSkills<SSkillPreset> AttackingSkills => attackingSkills;
        public ISharedSkills<SSkillPreset> NeutralSkills => neutralSkills;
        public ISharedSkills<SSkillPreset> DefendingSkills => defendingSkills;

        [Serializable]
        private class SharedSkillPresets : SharedSkillsBase<SSkillPreset>
        { }
    }

    
}
