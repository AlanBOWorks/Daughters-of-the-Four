using System;
using CombatSystem.Entity;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelector.Editor
{
    public class UCharacterPortraitEditor : MonoBehaviour
    {
        [SerializeField] private USelectableCharacterHolder selectableHolder;
        [SerializeField] private USelectedCharacterHolder facePortraitHolder;

        [Title("Asset")]
        [SerializeField, InlineEditor()] 
        private SCharacterLoreHolder preset;

        [ShowInInspector, ShowIf("_presetHolder")]
        private ICharacterPortraitHolder _presetHolder;

        private void OnValidate()
        {
            if(preset == null) return;
            _presetHolder = preset.GetPortraitHolder();

            if(!_presetHolder.GetCharacterPortraitImage()) return;

            HandleSelectedCharacterPortrait(_presetHolder);
            HandleSelectableCharacterPortrait(_presetHolder);
        }

        private void HandleSelectedCharacterPortrait(ICharacterPortraitHolder holder)
        {
            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.FacePivotPosition;
            facePortraitHolder.InjectPortrait(portrait,point);
        }

        private void HandleSelectableCharacterPortrait(ICharacterPortraitHolder holder)
        {
            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.SelectCharacterPivotPosition;
            selectableHolder.InjectPortrait(portrait,point);
        }
    }
}
