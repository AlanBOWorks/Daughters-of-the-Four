using System;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using DG.Tweening;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharacterSelector
{
    public class USelectableRoleHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
         IPointerClickHandler
    {
        [ShowInInspector, HideInEditorMode]
        private SPlayerPreparationEntity _entity;
        private SCharacterLoreHolder _entityKey;

        private USelectedCharactersHolder _selectedCharactersHolder;

        [SerializeField] private Image roleIconHolder;
        [SerializeField] private Image selectedImage;


        private void Awake()
        {
            if(_entity == null)
                Hide();
            selectedImage.gameObject.SetActive(false);
        }

        public SCharacterLoreHolder GetCurrentKey() => _entityKey;
        public SPlayerPreparationEntity GetCurrentPreset() => _entity;

        public void Injection(USelectedCharactersHolder holder) => _selectedCharactersHolder = holder;

        public void Injection(SPlayerPreparationEntity roleEntity, SCharacterLoreHolder entityKey)
        {
            _entity = roleEntity;
            _entityKey = entityKey;

            var combatTheme = CombatThemeSingleton.RolesThemeHolder;

            var entityRoleType = roleEntity.GetAreaData().RoleType;
            var roleTheme = UtilsTeam.GetElement(entityRoleType, combatTheme);
            roleIconHolder.sprite = roleTheme.GetThemeIcon();
        }


        private const float AnimationDuration = .4f;
        private void Animate()
        {
            Vector3 punch = new Vector3(-.1f,-.1f,0);
            transform.DOPunchScale(punch, AnimationDuration);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            Animate();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            Animate();
            HandleSelection();
        }

        private void HandleSelection()
        {
            var selectionValues = new USelectedCharactersHolder.SelectedCharacterValues(_entityKey, _entity, this);
            _selectedCharactersHolder.SelectCharacter(selectionValues);


        }

        public void ShowSelected()
        {
            var selectedGameObject = selectedImage.gameObject;
            selectedGameObject.SetActive(true);
        }

        public void HideSelected()
        {
            var selectedGameObject = selectedImage.gameObject;
            selectedGameObject.SetActive(false);

        }


        public void Hide()
        {
            var parent = transform.parent;
            parent.gameObject.SetActive(false);
        }
        public void Show()
        {
            var parent = transform.parent;
            parent.gameObject.SetActive(true);
        }
    }
}
