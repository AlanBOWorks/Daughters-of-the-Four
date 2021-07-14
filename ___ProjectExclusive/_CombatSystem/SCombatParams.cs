using System;
using System.Collections.Generic;
using Characters;
using _Player;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    [CreateAssetMenu(fileName = "Combat Parameters [Singleton]",
        menuName = "Combat/Combat Parameters")]
    public class SCombatParams : ScriptableObject
    {
        [Title("Skills")]
        [SerializeField] private Skills backupSkills = new Skills();
        /// <summary>
        /// Used when a character doesn't have skills
        /// </summary>
        public ICharacterArchetypesData<SCharacterSharedSkillsPreset> ArchetypesBackupSkills => backupSkills;

        [Serializable]
        internal class Skills : ICharacterArchetypesData<SCharacterSharedSkillsPreset>
        {
            [SerializeField] private SCharacterSharedSkillsPreset vanguard;
            [SerializeField] private SCharacterSharedSkillsPreset attacker;
            [SerializeField] private SCharacterSharedSkillsPreset support;

            [TitleGroup("Characters")]
            public SCharacterSharedSkillsPreset FrontLiner => vanguard;
            public SCharacterSharedSkillsPreset MidLiner => attacker;
            public SCharacterSharedSkillsPreset BackLiner => support;
        }

        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = this;
        }
    }
}
