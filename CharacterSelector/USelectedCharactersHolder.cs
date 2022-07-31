using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterSelector
{
    public class USelectedCharactersHolder : UTeamStructure<USelectedCharacterHolder>
    {
        [Title("Self Behaviour")]
        [SerializeField] 
        private UStartRunHandler startRunHandler;

        [ShowInInspector,HideInEditorMode]
        private Dictionary<USelectedCharacterHolder, USelectableRoleHolder> _tracker;

        public bool AllowRepetition { get; private set; }
        [ShowInInspector,HideInEditorMode]
        private Dictionary<SCharacterLoreHolder,int> _characterRepetitionTracker;


        public void SetAllowRepetition(bool allow)
        {
            AllowRepetition = allow;
            HandleForTeamReady();
        }

        private void Awake()
        {
            var theme = CombatThemeSingleton.RolesThemeHolder;
            Injections(theme);

            _tracker = new Dictionary<USelectedCharacterHolder, USelectableRoleHolder>();
            _characterRepetitionTracker = new Dictionary<SCharacterLoreHolder, int>();
        }

        private void Start()
        {
            HandleForTeamReady();
        }

        [ShowInInspector,DisableInEditorMode]
        private int _selectedCharactersCount;
        private const int SelectedCharacterCheckAmount = 4;
        private bool IsTeamReady()
        {
            if (_selectedCharactersCount == SelectedCharacterCheckAmount && AllowRepetition) return true;

            return _characterRepetitionTracker.Count == SelectedCharacterCheckAmount;
        }


        private void Injections(ITeamFlexStructureRead<CombatThemeHolder> theme)
        {
            var enumerable = UtilsTeam.GetEnumerable(theme, this);
            foreach ((CombatThemeHolder iconHolder, USelectedCharacterHolder element) in enumerable)
            {
                element.InjectIcon(iconHolder.GetThemeIcon());
                element.Injection(this);
            }
        }


        public void SelectCharacter(SelectedCharacterValues values)
        {
            var characterLore = values.Key;
            var characterPreset = values.CombatPreset;
            var roleButton = values.Button;

            var selectionHolder = GetElement(characterPreset.GetAreaData().RoleType);

            if (_tracker.ContainsKey(selectionHolder))
            {
                USelectableRoleHolder holderValue = _tracker[selectionHolder];
                var previousKey = holderValue.GetCurrentKey();
                TryRemoveKey(previousKey);
                _selectedCharactersCount--;

                holderValue.HideSelected();
                _tracker.Remove(selectionHolder);

                if (holderValue == roleButton)
                {
                    selectionHolder.HidePortrait();
                    return;
                }
            }

            _tracker.Add(selectionHolder, roleButton);
            roleButton.ShowSelected();
            SelectCharacter(selectionHolder, characterLore, characterPreset);
        }

        public void SelectCharacter(SCharacterLoreHolder key, SPlayerPreparationEntity combatPreset)
        {
            var selectionHolder = GetElement(combatPreset.GetAreaData().RoleType);
            SelectCharacter(selectionHolder,key,combatPreset);
        }

        private void SelectCharacter(
            USelectedCharacterHolder selectionHolder, 
            SCharacterLoreHolder key,
            SPlayerPreparationEntity combatPreset)
        {
            selectionHolder.InjectPortrait(key.GetPortraitHolder());
            selectionHolder.InjectionEntity(combatPreset);
            selectionHolder.ShowPortrait();

            AddKey(key);
            _selectedCharactersCount++;
            HandleForTeamReady();
        }

        private void AddKey(SCharacterLoreHolder key)
        {
            if (_characterRepetitionTracker.ContainsKey(key))
            {
                _characterRepetitionTracker[key]++;
                return;
            }
            _characterRepetitionTracker.Add(key,1);
        }

        private void TryRemoveKey(SCharacterLoreHolder key)
        {
            if (!_characterRepetitionTracker.ContainsKey(key)) return;


            int selectedAmount = _characterRepetitionTracker[key];
            var shouldRemove = selectedAmount <= 1;

            if (shouldRemove)
                _characterRepetitionTracker.Remove(key);
            else
                _characterRepetitionTracker[key]--;

            startRunHandler.DisableControl();
        }


        public void DisableEntity(USelectedCharacterHolder button)
        {
            if(!_tracker.ContainsKey(button)) return;

            var key = _tracker[button];
            key.HideSelected();
            _tracker.Remove(button);
            button.HidePortrait();
        }

        private void HandleForTeamReady()
        {
            if (IsTeamReady())
            {
                startRunHandler.EnableControl();
            }
            else
            {
                startRunHandler.DisableControl();
            }
        }


        public readonly struct SelectedCharacterValues
        {
            public readonly SCharacterLoreHolder Key;
            public readonly SPlayerPreparationEntity CombatPreset;
            public readonly USelectableRoleHolder Button;

            public SelectedCharacterValues(SCharacterLoreHolder key, SPlayerPreparationEntity combatPreset, USelectableRoleHolder button)
            {
                Key = key;
                CombatPreset = combatPreset;
                Button = button;
            }
        }
    }
}
