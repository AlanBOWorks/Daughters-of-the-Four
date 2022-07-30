using System;
using CombatSystem.Entity;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterSelector
{
    public class USelectableCharactersHolder : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] private USelectedCharactersHolder selectedCharactersHolder;

        [TitleGroup("Instantiations")]
        [SerializeField, HideInPlayMode] private USelectableCharacterHolder copySelectablePrefab;

        [TitleGroup("Characters")] 
        [SerializeField]
        private SelectableCharacter[] selectableCharacters = new SelectableCharacter[0];


        private void Awake()
        {
            InstantiateCharacters();
        }

        private const float LateralSeparation = 2f;
        private void InstantiateCharacters()
        {
            var instantiationParent = transform;

            var rectTransform = (RectTransform) copySelectablePrefab.transform;
            var anchorPosition = rectTransform.anchoredPosition;
            var rect = rectTransform.rect;

            var lateralSeparation = LateralSeparation + rect.width;
            var height = anchorPosition.y;
            var initialLateralOffset = anchorPosition.x;

            var count = selectableCharacters.Length;

            InstantiateCharacter(copySelectablePrefab, selectableCharacters[0]);
            for(int i = 1; i < count; i++)
            {
                var holder = InstantiateHolder(instantiationParent);
                InstantiateCharacter(holder, selectableCharacters[i]);

                Vector2 targetAnchorPosition = new Vector2(lateralSeparation * i + initialLateralOffset, height);
                holder.RepositionHolder(targetAnchorPosition);
            } 
        }


        private USelectableCharacterHolder InstantiateHolder(Transform parent)
        {
            return Instantiate(copySelectablePrefab, parent);
        }
        private void InstantiateCharacter(USelectableCharacterHolder holder, SelectableCharacter selectableCharacter)
        {
            holder.Injection(selectedCharactersHolder,
                selectableCharacter.GetLoreHolder(),selectableCharacter.GetCharacterRoles());
        }
    }

    [Serializable]
    public struct SelectableCharacter
    {
        [SerializeField,InlineEditor()]
        private SCharacterLoreHolder loreHolder;
        [SerializeField]
        private SPlayerPreparationEntity[] charactersRoles;

        public SCharacterLoreHolder GetLoreHolder() => loreHolder;
        public SPlayerPreparationEntity[] GetCharacterRoles() => charactersRoles;
    }
}
