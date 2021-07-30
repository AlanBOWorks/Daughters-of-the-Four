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
            if(!CharacterArchetypes.IsValid(this))
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

        public SPlayerCharacterEntityVariable FrontLiner
        {
            get => _frontLiner;
            set => _frontLiner = value;
        }

        public SPlayerCharacterEntityVariable MidLiner
        {
            get => _midLiner;
            set => _midLiner = value;
        }

        public SPlayerCharacterEntityVariable BackLiner
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
            FrontLiner = new PlayableCharacter(variable.FrontLiner);
            MidLiner = new PlayableCharacter(variable.MidLiner);
            BackLiner = new PlayableCharacter(variable.BackLiner);
        }
        [ShowInInspector]
        public PlayableCharacter FrontLiner { get; set; }
        [ShowInInspector]
        public PlayableCharacter MidLiner { get; set; }
        [ShowInInspector]
        public PlayableCharacter BackLiner { get; set; }
    }
}
