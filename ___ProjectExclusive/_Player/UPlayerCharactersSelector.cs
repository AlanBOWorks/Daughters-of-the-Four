using System;
using System.Collections.Generic;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UPlayerCharactersSelector : MonoBehaviour, 
        IPlayerArchetypes<SPlayerCharacterEntityVariable>
    {
        [Title("Selections")]
        [SerializeField] private SPlayerCharacterEntityVariable _frontLiner;
        [SerializeField] private SPlayerCharacterEntityVariable _midLiner;
        [SerializeField] private SPlayerCharacterEntityVariable _backLiner;
        [SerializeField] private STeamControlStatsPreset teamStats;

        public bool autoSelectionOnStart = true;

        public static void DoSelect(IPlayerArchetypesData<ICharacterCombatProvider> selected)
        {
            PlayerEntitySingleton.SelectedCharacters = selected;
        }
        [Button,DisableInEditorMode]
        public void DoSelectOfCurrent()
        {
            if(!UtilsCharacterArchetypes.IsValid(this))
                throw new ArgumentException("Invalid selected characters; Some roles are not selected");

            var playableCharacters = new PlayableCharactersSelected(this);
            DoSelect(playableCharacters);
        }

        private void Start()
        {
            if (autoSelectionOnStart)
            {
                DoSelectOfCurrent();
                PlayerEntitySingleton.TeamControlStats = teamStats;
            }
        }

        public SPlayerCharacterEntityVariable Vanguard
        {
            get => _frontLiner;
            set => _frontLiner = value;
        }

        public SPlayerCharacterEntityVariable Attacker
        {
            get => _midLiner;
            set => _midLiner = value;
        }

        public SPlayerCharacterEntityVariable Support
        {
            get => _backLiner;
            set => _backLiner = value;
        }
    }

    /// <summary>
    /// A wrapper for creating and holding data that can be altered through GamePlay without fear.<br></br>
    /// On before/after play should be saved in a persistent data holder (JSON?). This
    /// should be managed by the <see cref="PlayableCharacter"/> class itself
    /// </summary>
    internal class PlayableCharactersSelected : IPlayerArchetypes<PlayableCharacter>
    {
        public PlayableCharactersSelected(ICharacterArchetypesData<SPlayerCharacterEntityVariable> variable)
        {
            Vanguard = new PlayableCharacter(variable.Vanguard);
            Attacker = new PlayableCharacter(variable.Attacker);
            Support = new PlayableCharacter(variable.Support);
        }
        [ShowInInspector]
        public PlayableCharacter Vanguard { get; set; }
        [ShowInInspector]
        public PlayableCharacter Attacker { get; set; }
        [ShowInInspector]
        public PlayableCharacter Support { get; set; }
    }
}
