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
#if UNITY_EDITOR
        [SerializeField] private USelectableCharacterHolder selectableHolder;
        [SerializeField] private USelectedCharacterHolder facePortraitHolder;

        [Title("Asset")]
        [SerializeField, InlineEditor()]
        private SCharacterLoreHolder preset;

        [SerializeReference]
        private ICharacterPortraitHolder _portraitReference = null;


        private void OnValidate()
        {
            if (preset == null) return;

            var presetReference = preset.GetPortraitHolder();
            if (presetReference != null && _portraitReference != presetReference) 
                _portraitReference = presetReference;

            if (!_portraitReference.GetCharacterPortraitImage()) return;

            HandleSelectedCharacterPortrait(_portraitReference);
            HandleSelectableCharacterPortrait(_portraitReference);
        }

        private void HandleSelectedCharacterPortrait(ICharacterPortraitHolder holder)
        {
            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.FacePivotPosition;
            facePortraitHolder.InjectPortrait(portrait, point);
        }

        private void HandleSelectableCharacterPortrait(ICharacterPortraitHolder holder)
        {
            if (selectableHolder == null) return;

            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.SelectCharacterPivotPosition;
                selectableHolder.InjectPortrait(portrait, point);
        }

        [Button, ShowIf("_portraitReference")]
        private void ReferenceToNull()
        {
            _portraitReference = null;
        }

#endif    
    }
}
