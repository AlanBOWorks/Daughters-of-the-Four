using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    internal sealed class PlayerSelectedCharactersHolder : 
        ICombatTeamProvider
    {
        public PlayerSelectedCharactersHolder()
        {
            int rolesAmount = EnumTeam.RoleTypesCount;
            _runTimeCharacters = new List<ICombatEntityProvider>(rolesAmount);
            _characterReferences = new HashSet<ICombatEntityProvider>();
        }

        [ShowInInspector,DisableInEditorMode,DisableInPlayMode]
        private readonly List<ICombatEntityProvider> _runTimeCharacters;
        private readonly HashSet<ICombatEntityProvider> _characterReferences;


        [Button]
        private void DebugCount()
        {
            Debug.Log($"List Length: {_runTimeCharacters.Capacity} - Characters Amount :" + Count);
        }

        [Button]
        public void AddPredefinedTeam(SPlayerPresetTeam predefinedTeam) => AddTeam(predefinedTeam);

        public void AddTeam(ICombatTeamProvider predefinedTeam)
        {
            Clear();

            var teamMembers = predefinedTeam.GetSelectedCharacters();
            foreach (var preset in teamMembers)
            {
                var runTimeCharacter = UtilsPlayerTeam.GenerateRunTimeEntity(preset);
                AddUnSafeSelected(preset,runTimeCharacter);
            }
        }
        public void AddTeam(IEnumerable<SelectedCharacterPair> characters)
        {
            Clear();

            foreach (SelectedCharacterPair pair in characters)
            {
                AddUnSafeSelected(pair.Preset,pair.RunTimeEntity);
            }
        }
        private void AddUnSafeSelected(ICombatEntityProvider preset, ICombatEntityProvider runTimeCharacter)
        {
            _characterReferences.Add(preset);
            _runTimeCharacters.Add(runTimeCharacter);
        }

        public void AddSelectedCharacter(SelectedCharacterPair selectedCharacter)
        {
            var preset = selectedCharacter.Preset;
            if(Contains(preset)) return;

            AddUnSafeSelected(preset,selectedCharacter.RunTimeEntity);
        }


        /// <summary>
        /// Used when the player does a quick "Run Reset" > re-instantiates the entities
        /// to their initial state removing skills and modified stats.
        /// </summary>
        public void ResetToInitialReferences()
        {
            if(_characterReferences.Count == 0) return;

            _runTimeCharacters.Clear();
            foreach (var preset in _characterReferences)
            {
                var runTimeEntity = UtilsPlayerTeam.GenerateRunTimeEntity(preset);
                _runTimeCharacters.Add(runTimeEntity);
            }
        }

        public void Clear()
        {
            _runTimeCharacters.Clear();
            _characterReferences.Clear();
        }

        public int Count => _runTimeCharacters.Count;
        public bool IsValid() => Count > 0;
        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => _runTimeCharacters;
        public int MembersCount => Count;
        public bool Contains(ICombatEntityProvider key) => _characterReferences.Contains(key);
    }


    public readonly struct SelectedCharacterPair
    {
        public readonly ICombatEntityProvider Preset;
        public readonly ICombatEntityProvider RunTimeEntity;

        public SelectedCharacterPair(ICombatEntityProvider preset, ICombatEntityProvider runTimeEntity)
        {
            Preset = preset;
            RunTimeEntity = runTimeEntity;
        }
    }
}
