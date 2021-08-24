using System;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace Characters
{
    public abstract class SCharacterEntityVariable : ScriptableObject, ICharacterCombatProvider
    {
        [Title("Narrative")]
        [GUIColor(.5f, .8f, 1f)]
        public string characterName = "NULL";

        [Title("Instantiation")]
        [SerializeField,AssetsOnly]
        private GameObject characterPrefab = null;

        public SCharacterSkillsPreset skillsPreset = null;
        public SCharacterSharedSkillsPreset sharedSkillsPreset = null;

        [TitleGroup("Passives")]
        [SerializeField] 
        private SCriticalBuffPreset criticalBuff = null;

        [TitleGroup("Stats")] 
        public CharacterArchetypes.RangeType rangeType = CharacterArchetypes.RangeType.Melee;

        public Transform SceneInstantiateCharacter(Vector3 position, Quaternion rotation)
        {
            return CharacterHolderReference 
                ? CharacterHolderReference 
                : (CharacterHolderReference = Instantiate(characterPrefab,position,rotation).transform);
        }

        public abstract CombatStatsHolder GenerateCombatData();
        public CombatSkills GenerateCombatSkills(CombatingEntity injection)
        {
            return new CombatSkills(injection,sharedSkillsPreset, skillsPreset);
        }

        public string CharacterName => characterName;
        public Transform CharacterHolderReference { get; private set; }
        public GameObject CharacterPrefab => characterPrefab;
        public SCharacterSkillsPreset UniqueSkillsPreset => skillsPreset;
        public SCharacterSharedSkillsPreset SharedSkillsPreset => sharedSkillsPreset;
     
        public SCriticalBuffPreset GetCriticalBuff() => criticalBuff;

        public CharacterArchetypes.RangeType RangeType => rangeType;
    }

    public interface ICharacterCombatProvider : ICharacterLore
    {
        GameObject CharacterPrefab { get; }
        CharacterArchetypes.RangeType RangeType { get; }
        CombatStatsHolder GenerateCombatData();
        CombatSkills GenerateCombatSkills(CombatingEntity injection);
        SCriticalBuffPreset GetCriticalBuff();
    }
}

