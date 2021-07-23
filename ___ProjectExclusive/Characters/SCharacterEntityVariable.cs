using System;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Characters
{
    public abstract class SCharacterEntityVariable : ScriptableObject, ICharacterCombatProvider
    {
        [Title("Narrative")]
        [GUIColor(.5f, .8f, 1f)]
        public string characterName = "NULL";
        public string CharacterName => characterName;

        [Title("Instantiation")]
        [SerializeField,AssetsOnly]
        private GameObject characterPrefab = null;
        public GameObject CharacterPrefab => characterPrefab;

        public SCharacterSkillsPreset skillsPreset = null;
        public SCharacterSharedSkillsPreset sharedSkillsPreset = null;
        public SCharacterSkillsPreset UniqueSkillsPreset => skillsPreset;
        public SCharacterSharedSkillsPreset SharedSkillsPreset => sharedSkillsPreset;

        [TitleGroup("Passives")]
        [SerializeField] private PassivesHolder passivesHolder = new PassivesHolder();
        public PassivesHolder GetPassivesHolder() => passivesHolder;

        [TitleGroup("Stats")] 
        public CharacterArchetypes.RangeType rangeType = CharacterArchetypes.RangeType.Melee;
        public CharacterArchetypes.RangeType RangeType => rangeType;


        public Transform CharacterHolderReference
        {
            get;
            private set;
        }

        public Transform SceneInstantiateCharacter(Vector3 position, Quaternion rotation)
        {
            return CharacterHolderReference 
                ? CharacterHolderReference 
                : (CharacterHolderReference = Instantiate(characterPrefab,position,rotation).transform);
        }

        public abstract CharacterCombatData GenerateCombatData();
        public CombatSkills GenerateCombatSkills(CombatingEntity injection)
        {
            return new CombatSkills(injection,sharedSkillsPreset,UniqueSkillsPreset);
        }
    }

    public interface ICharacterCombatProvider : ICharacterLore
    {
        GameObject CharacterPrefab { get; }
        CharacterArchetypes.RangeType RangeType { get; }
        CharacterCombatData GenerateCombatData();
        CombatSkills GenerateCombatSkills(CombatingEntity injection);
        PassivesHolder GetPassivesHolder( );
    }
}

