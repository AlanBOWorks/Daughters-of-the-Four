using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    internal sealed class PlayerSelectedCharactersHolder : 
        IReadOnlyCollection<ICombatEntityProvider>,
        ICombatTeamProvider
    {
        public PlayerSelectedCharactersHolder()
        {
            int rolesAmount = EnumTeam.RoleTypesAmount;
            _characters = new List<ICombatEntityProvider>(rolesAmount);
            for (int i = 0; i < rolesAmount; i++)
            {
                _characters.Add(null);
            }
        }

        [ShowInInspector,DisableInEditorMode,DisableInPlayMode]
        private readonly List<ICombatEntityProvider> _characters;

        [Button]
        private void DebugCount()
        {
            Debug.Log($"List Length: {_characters.Capacity} - Characters Amount :" + Count);
        }

        [Button]
        public void AddTeam(SPlayerPresetTeam predefinedTeam)
        {
            foreach (var character in predefinedTeam.GetSelectedCharacters())
            {
                Add(character);
            }
        }

        [Button]
        private void AddCharacter(SPlayerPreparationEntity selectedCharacter)
        => Add(selectedCharacter);

        [Button]
        private void TryRemove(SPlayerPreparationEntity selectedCharacter)
            => TryRemove(selectedCharacter as ICombatEntityProvider);

        public void Add(ICombatEntityProvider selectedCharacter)
        {
            TryRemove(selectedCharacter);

            int roleIndex = UtilsTeam.GetRoleIndex(selectedCharacter);
            _characters[roleIndex] = selectedCharacter;
            Count++;
        }

        public void TryRemove(ICombatEntityProvider selectedCharacter)
        {
            if (!_characters.Contains(selectedCharacter)) return;

            int roleIndex = UtilsTeam.GetRoleIndex(selectedCharacter);
            _characters[roleIndex] = null;
            Count--;
        }


        public IEnumerator<ICombatEntityProvider> GetEnumerator() => _characters.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        [ShowInInspector,DisableInEditorMode,DisableInPlayMode,PropertyOrder(-10)]
        public int Count { get; private set; }
        public IReadOnlyCollection<ICombatEntityProvider> GetSelectedCharacters() => _characters;
        public bool IsValid() => Count > 0;

    }
}
