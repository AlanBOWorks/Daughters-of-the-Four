using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using ExplorationSystem;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils_Project;

namespace CharacterSelector
{
    public class USelectedCharactersHolder : UTeamStructure<USelectedCharacterHolder>
    {
        [Title("Self Behaviour")]
        [SerializeField] 
        private UStartRunHandler startRunHandler;

        [ShowInInspector,HideInEditorMode]

        private Dictionary<SPlayerPreparationEntity, CharacterKeys> _characterKeys;

        public bool AllowRepetition { get; private set; }
        [ShowInInspector,HideInEditorMode]
        private Dictionary<SCharacterLoreHolder,int> _characterRepetitionTracker;


        public void SetAllowRepetition(bool allow)
        {
            AllowRepetition = allow;
            HandleForTeamReady();
        }


        private const int CollectionSize = EnumTeam.RoleTypesCount;
        private void Awake()
        {
            var theme = CombatThemeSingleton.RolesThemeHolder;
            Injections(theme);

            _characterKeys = new Dictionary<SPlayerPreparationEntity, CharacterKeys>(CollectionSize);
            _characterRepetitionTracker = new Dictionary<SCharacterLoreHolder, int>(CollectionSize);

            startRunHandler.Injection(this);
        }

        private void Start()
        {
            HandleForTeamReady();
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
            SPlayerPreparationEntity characterPreset = values.CombatPreset;
            SCharacterLoreHolder characterLore = values.LoreKey;
            USelectableRoleHolder selectableButton = values.Button;

            USelectedCharacterHolder selectedButton = GetElement(characterPreset.GetAreaData().RoleType);
            var selectedButtonEntity = selectedButton.GetPreset();

            bool isRoleAlreadySelected = selectedButtonEntity != null;
            if (isRoleAlreadySelected)
            {
                RemoveCharacter(selectedButtonEntity);

                bool isTheSameCharacterSelected = selectedButtonEntity == characterPreset;
                if (isTheSameCharacterSelected)
                    return;
            }

            if(selectableButton) selectableButton.ShowSelected();
            AddKeys(characterPreset, selectedButton, selectableButton, characterLore);
            InjectCharacterInHolders(selectedButton, characterLore, characterPreset);

            HandleForTeamReady();
        }
        private void AddKeys(
            SPlayerPreparationEntity combatPreset,
            USelectedCharacterHolder selectedButton,
            USelectableRoleHolder selectableButton,
            SCharacterLoreHolder loreKey
        )
        {
            var keys = new CharacterKeys(loreKey, selectableButton, selectedButton);
            _characterKeys.Add(combatPreset, keys);

            if (_characterRepetitionTracker.ContainsKey(loreKey))
                _characterRepetitionTracker[loreKey]++;
            else
                _characterRepetitionTracker.Add(loreKey, 1);
        }


        private static void InjectCharacterInHolders(
            USelectedCharacterHolder selectionHolder,
            SCharacterLoreHolder key,
            SPlayerPreparationEntity combatPreset)
        {
            selectionHolder.InjectPortrait(key.GetPortraitHolder());
            selectionHolder.InjectionEntity(combatPreset);
            selectionHolder.ShowPortrait();
        }


        public void RemoveCharacter(SPlayerPreparationEntity key)
        {
            if(!_characterKeys.ContainsKey(key)) return;
            var keys = _characterKeys[key];
            if(!keys.IsValid()) return;


            TryRemoveLoreKey(key, keys);
            RemoveCharacterInHolders(keys, key);

            HandleForTeamReady();
        }
        private void TryRemoveLoreKey(SPlayerPreparationEntity key, CharacterKeys keys)
        {
            _characterKeys.Remove(key);

            var loreKey = keys.LoreKey;
            if (!_characterRepetitionTracker.ContainsKey(loreKey)) return;


            int selectedAmount = _characterRepetitionTracker[loreKey];
            var shouldRemove = selectedAmount <= 1;

            if (shouldRemove)
                _characterRepetitionTracker.Remove(loreKey);
            else
                _characterRepetitionTracker[loreKey]--;

            startRunHandler.DisableControl();
        }
        private static void RemoveCharacterInHolders(CharacterKeys keys, SPlayerPreparationEntity key)
        {
            keys.SelectableButtonKey.HideSelected();
            keys.SelectedButtonKey.RemoveEntity(key);
        }


        private int CalculateTrueSelectedCharactersCount()
        {
            var selectedCharactersCount = 0;
            foreach (var pair in _characterRepetitionTracker)
            {
                selectedCharactersCount += pair.Value;
            }

            return selectedCharactersCount;
        }


        private const int SelectedCharacterCheckAmount = 4;
        private bool IsTeamReady()
        {
            if (!AllowRepetition)
            {
                var countWithoutRepetition = _characterRepetitionTracker.Count;
                return countWithoutRepetition == SelectedCharacterCheckAmount;
            }

            var countWithRepetition = CalculateTrueSelectedCharactersCount();
            return (countWithRepetition == SelectedCharacterCheckAmount);

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


        public void ConfirmTeamAndSendToSingleton()
        {
            if (!IsTeamReady())
                throw new AccessViolationException($"Team wasn't ready - Count: [{CalculateTrueSelectedCharactersCount()}]");

            PlayerExplorationSingleton.Instance.InjectTeam(this);
            UtilsScene.LoadWorldMapScene(true, false);
        }


        public readonly struct SelectedCharacterValues
        {
            public readonly SPlayerPreparationEntity CombatPreset;
            public readonly SCharacterLoreHolder LoreKey;
            public readonly USelectableRoleHolder Button;

            public SelectedCharacterValues(
                SPlayerPreparationEntity combatPreset, 
                SCharacterLoreHolder loreKey,
                USelectableRoleHolder selectableButton)
            {
                CombatPreset = combatPreset;
                LoreKey = loreKey;
                Button = selectableButton;
            }
        }
        private readonly struct CharacterKeys
        {
            public static CharacterKeys NullKeys = new CharacterKeys(null,null, null);

            public readonly SCharacterLoreHolder LoreKey;
            public readonly USelectableRoleHolder SelectableButtonKey;
            public readonly USelectedCharacterHolder SelectedButtonKey;

            public CharacterKeys(SelectedCharacterValues values, USelectedCharacterHolder selectedButtonKey)
            {
                LoreKey = values.LoreKey;
                SelectableButtonKey = values.Button;
                SelectedButtonKey = selectedButtonKey;
            }
            public CharacterKeys(
                SCharacterLoreHolder loreKey, 
                USelectableRoleHolder selectableButtonKey, 
                USelectedCharacterHolder selectedButtonKey)
            {
                LoreKey = loreKey;
                SelectableButtonKey = selectableButtonKey;
                SelectedButtonKey = selectedButtonKey;
            }

            public bool IsValid() => SelectableButtonKey && SelectedButtonKey && LoreKey;
        }
    }
}
