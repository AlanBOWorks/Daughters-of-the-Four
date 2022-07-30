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
        [ShowInInspector,HideInEditorMode]
        private Dictionary<USelectedCharacterHolder, USelectableRoleHolder> _tracker;

        private void Awake()
        {
            var theme = CombatThemeSingleton.RolesThemeHolder;
            Injections(theme);

            _tracker = new Dictionary<USelectedCharacterHolder, USelectableRoleHolder>();
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
                holderValue.HideSelected();

                if (holderValue == roleButton)
                {
                    _tracker.Remove(selectionHolder);
                    selectionHolder.HidePortrait();
                    return;
                }
            }

            _tracker.Add(selectionHolder, roleButton);
            roleButton.ShowSelected();
            selectionHolder.HandleSelectedCharacterPortrait(characterLore.GetPortraitHolder());
            selectionHolder.InjectionEntity(characterPreset);
            selectionHolder.ShowPortrait();
        }

        public void DisableEntityUnsafe(USelectedCharacterHolder button)
        {
            var key = _tracker[button];
            key.HideSelected();
            _tracker.Remove(button);
            button.HidePortrait();
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
