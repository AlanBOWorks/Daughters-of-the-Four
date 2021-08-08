using System;
using _Team;
using Characters;
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
        [SerializeField] 
        private Skills onNullSkills = new Skills();
        [Title("BackUp Skills")]
        [SerializeField] private SBackUpSkills backUpSkills;

        [Title("Critical buffs")]
        [SerializeField]
        private CriticalBuff onNullCriticalBuff = new CriticalBuff();


        [Title("Animator")]
        public SkipAnimationsType skipAnimationsType = SkipAnimationsType.None;

        [Title("Params")] 
        [Range(-1,1)] public float criticalHarmonyAddition = 0.05f;

        public enum SkipAnimationsType
        {
            None,
            Enemy,
            All
        }

        /// <summary>
        /// Used when a character doesn't have skills
        /// </summary>
        public ICharacterArchetypesData<SCharacterSharedSkillsPreset> ArchetypesOnNullSkills 
            => onNullSkills;
        public ICharacterArchetypesData<SCriticalBuffPreset> ArchetypesBackupOnNullCriticalBuffs
            => onNullCriticalBuff;
        public ISkillBackUp<SkillPreset[]> BackUpSkills => backUpSkills;
        
        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = this;
        }

        [Serializable]
        private class Skills : ICharacterArchetypesData<SCharacterSharedSkillsPreset>
        {
            [SerializeField] private SCharacterSharedSkillsPreset vanguard;
            [SerializeField] private SCharacterSharedSkillsPreset attacker;
            [SerializeField] private SCharacterSharedSkillsPreset support;

            [TitleGroup("Characters")]
            public SCharacterSharedSkillsPreset FrontLiner => vanguard;
            public SCharacterSharedSkillsPreset MidLiner => attacker;
            public SCharacterSharedSkillsPreset BackLiner => support;
        }

        [Serializable]
        private class CriticalBuff : ICharacterArchetypesData<SCriticalBuffPreset>
        {
            [SerializeField] private SCriticalBuffPreset frontLiner;
            [SerializeField] private SCriticalBuffPreset midLiner;
            [SerializeField] private SCriticalBuffPreset backLiner;

            public SCriticalBuffPreset FrontLiner => frontLiner;

            public SCriticalBuffPreset MidLiner => midLiner;

            public SCriticalBuffPreset BackLiner => backLiner;
        }
    }
}
