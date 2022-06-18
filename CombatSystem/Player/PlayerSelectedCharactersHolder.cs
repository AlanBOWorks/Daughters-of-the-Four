using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
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
            _characters = new List<ICombatEntityProvider>(rolesAmount);
            for (int i = 0; i < rolesAmount; i++)
            {
                _characters.Add(null);
            }

            _teamSkills = new List<ITeamSkillPreset>();
        }

        [ShowInInspector,DisableInEditorMode,DisableInPlayMode]
        private readonly List<ICombatEntityProvider> _characters;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly List<ITeamSkillPreset> _teamSkills;

        [Button]
        private void DebugCount()
        {
            Debug.Log($"List Length: {_characters.Capacity} - Characters Amount :" + Count);
        }

        [Button]
        public void AddTeam(SPlayerPresetTeam predefinedTeam)
        {
            _characters.Clear();
            var teamMembers = predefinedTeam.GetPresetCharacters();
            _characters.AddRange(teamMembers);
            var teamSkills = predefinedTeam.GetTeamSkills();
            _teamSkills.AddRange(teamSkills);
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
        }

        public void TryRemove(ICombatEntityProvider selectedCharacter)
        {
            if (!_characters.Contains(selectedCharacter)) return;

            int roleIndex = UtilsTeam.GetRoleIndex(selectedCharacter);
            _characters[roleIndex] = null;
        }

        public void Add(ITeamSkillPreset teamSkill)
        {
            _teamSkills.Add(teamSkill);
        }

        public void TryRemove(ITeamSkillPreset teamSkill)
        {
            if (_teamSkills.Contains(teamSkill)) return;
            _teamSkills.Remove(teamSkill);
        }

        public int Count => _characters.Count;
        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => _characters;
        public IEnumerable<ITeamSkillPreset> GetTeamSkills() => _teamSkills;

        public bool IsValid() => Count > 0;

    }
}
