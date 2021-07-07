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
        [SerializeField] private SCharacterSkillsPreset commonSkills;
        [SerializeField] private Skills archetypesSkills = new Skills();
        public ICharacterArchetypesData<SCharacterSkillsPreset> ArchetypesSkills => archetypesSkills;

        [Serializable]
        internal class Skills : ICharacterArchetypesData<SCharacterSkillsPreset>
        {
            [SerializeField] private SCharacterSkillsPreset vanguard;
            [SerializeField] private SCharacterSkillsPreset attacker;
            [SerializeField] private SCharacterSkillsPreset support;

            [TitleGroup("Characters")]
            public SCharacterSkillsPreset FrontLiner => vanguard;
            public SCharacterSkillsPreset MidLiner => attacker;
            public SCharacterSkillsPreset BackLiner => support;
        }

        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = this;
        }
    }
}
