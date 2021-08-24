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
        private SharedSkillsSet sharedSkillsSets = new SharedSkillsSet();
        [SerializeField] 
        private NullSkills onNullSkills = new NullSkills();
        [SerializeField] 
        private SBackUpSkills backUpSkills;


        [Title("Critical buffs")]
        [SerializeField]
        private CriticalBuff onNullCriticalBuff = new CriticalBuff();


        [Title("Animator")]
        public SkipAnimationsType skipAnimationsType = SkipAnimationsType.None;
        [SerializeField,Range(.2f,5f)] 
        private float tempoVelocityModifier = 1;


        [Title("Params")] 
        [Range(-1,1)] public float criticalHarmonyAddition = 0.05f;

        public enum SkipAnimationsType
        {
            None,
            Enemy,
            All
        }


        public SSharedSkillsSet GetSkillPreset(
            EnumCharacter.RoleArchetype role,
            EnumCharacter.RangeType range)
        {
            var roleSkills = UtilsCharacter.GetElement(sharedSkillsSets, role);

            return UtilsCharacter.GetElement(roleSkills, range);

        }
        /// <summary>
        /// Used when a character doesn't have skills
        /// </summary>
        public ICharacterArchetypesData<SCharacterSharedSkillsPreset> ArchetypesOnNullSkills 
            => onNullSkills;
        public ICharacterArchetypesData<SCriticalBuffPreset> ArchetypesBackupOnNullCriticalBuffs
            => onNullCriticalBuff;
        public ISkillBackUp<SkillPreset[]> BackUpSkills => backUpSkills;

        public float TempoVelocityModifier
        {
            get => tempoVelocityModifier;
            set
            {
                tempoVelocityModifier = value;

                var currentTempoHandler = CombatSystemSingleton.TempoHandler;
                if (currentTempoHandler != null) currentTempoHandler.TempoStepModifier = value;
            }
        }

        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = this;
        }

        [Serializable]
        private class NullSkills : ICharacterArchetypesData<SCharacterSharedSkillsPreset>
        {
            [SerializeField] private SCharacterSharedSkillsPreset vanguard;
            [SerializeField] private SCharacterSharedSkillsPreset attacker;
            [SerializeField] private SCharacterSharedSkillsPreset support;

            [TitleGroup("Characters")]
            public SCharacterSharedSkillsPreset Vanguard => vanguard;
            public SCharacterSharedSkillsPreset Attacker => attacker;
            public SCharacterSharedSkillsPreset Support => support;
        }

        [Serializable]
        private class SharedSkillsSet : ICharacterArchetypesData<ICharacterRangesData<SSharedSkillsSet>>
        {
            [SerializeField] private RangesSkills frontLiner;
            [SerializeField] private RangesSkills midLiner;
            [SerializeField] private RangesSkills backLiner;

            public ICharacterRangesData<SSharedSkillsSet> Vanguard => frontLiner;

            public ICharacterRangesData<SSharedSkillsSet> Attacker => midLiner;

            public ICharacterRangesData<SSharedSkillsSet> Support => backLiner;

            [Serializable]
            private class RangesSkills : RangesSkills<SSharedSkillsSet>
            {}
        }

        [Serializable]
        private class CriticalBuff : ICharacterArchetypesData<SCriticalBuffPreset>
        {
            [SerializeField] private SCriticalBuffPreset frontLiner;
            [SerializeField] private SCriticalBuffPreset midLiner;
            [SerializeField] private SCriticalBuffPreset backLiner;

            public SCriticalBuffPreset Vanguard => frontLiner;

            public SCriticalBuffPreset Attacker => midLiner;

            public SCriticalBuffPreset Support => backLiner;
        }
    }
}
