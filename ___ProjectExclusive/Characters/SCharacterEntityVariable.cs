using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Characters
{
    public abstract class SCharacterEntityVariable : ScriptableObject, ICharacterLore
    {
        [Title("Narrative")]
        [GUIColor(.5f, .8f, 1f)]
        public string characterName = "NULL";
        public string CharacterName => characterName;

        [Title("Instantiation")]
        [SerializeField]
        private GameObject characterPrefab = null;
        public GameObject CharacterPrefab => characterPrefab;

        public SCharacterSkillsPreset skillsPreset = null;
        public SCharacterSharedSkillsPreset sharedSkillsPreset = null;
        [TitleGroup("Stats")] 
        public CharacterArchetypes.RangeType rangeType = CharacterArchetypes.RangeType.Melee;

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

        public abstract CharacterCombatData GenerateData();
       
    }
}
