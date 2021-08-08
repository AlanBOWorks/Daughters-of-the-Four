using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N - Character Skills [Preset]",
        menuName = "Combat/Skill/[Set] Character Skills Preset")]
    public class SCharacterSkillsPreset : ScriptableObject, ISkillPositions<List<SkillPreset>>
    {
        [SerializeField] private SSkillSetPreset attackingSkills;
        [SerializeField] private SSkillSetPreset neutralSkills;
        [SerializeField] private SSkillSetPreset defendingSkills;


        public List<SkillPreset> AttackingSkills => attackingSkills?.GetSkills();
        public List<SkillPreset> NeutralSkills => neutralSkills?.GetSkills();
        public List<SkillPreset> DefendingSkills => defendingSkills?.GetSkills();

    }


}
