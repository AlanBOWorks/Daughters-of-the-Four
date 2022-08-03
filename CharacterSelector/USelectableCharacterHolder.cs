using System;
using CombatSystem.Entity;
using DG.Tweening;
using Lore.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharacterSelector
{
    public class USelectableCharacterHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image characterPortraitHolder;
        private RectTransform _rectTransform;

        [SerializeField] private USelectableRoleHolder[] roleHolders;

        private void Awake()
        {
            _rectTransform = (RectTransform) transform;
            _originalSizeDelta = _rectTransform.sizeDelta;
        }

        private void OnDestroy()
        {
            DOTween.Kill(_rectTransform);
        }

        private Vector2 _originalSizeDelta;

        public void InjectPortrait(Sprite portrait, Vector2 offset)
        {
            characterPortraitHolder.sprite = portrait;
            characterPortraitHolder.transform.localPosition = offset;
        }

        public void Injection(USelectedCharactersHolder holder,
            SCharacterLoreHolder loreHolder, SPlayerPreparationEntity[] roles)
        {
            var portrait = loreHolder.GetPortraitHolder();
            InjectPortrait(portrait.GetCharacterPortraitImage(),portrait.SelectCharacterPivotPosition);


            int loopThreshold = Mathf.Min(roles.Length, roleHolders.Length);
            for (var i = 0; i < loopThreshold; i++)
            {
                var role = roles[i];
                var element = roleHolders[i];
                element.Injection(role, loreHolder);
                element.Injection(holder);

                element.Show();
            }
        }

        public void RepositionHolder(Vector2 anchorPosition)
        {
            _rectTransform.anchoredPosition = anchorPosition;
        }



        private const float OnHoverWidthAddition = 64;
        private const float HoverAnimationDuration = .4f;
        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector2 targetDelta = new Vector2(_originalSizeDelta.x, OnHoverWidthAddition + _originalSizeDelta.y);
            _rectTransform.DOSizeDelta(targetDelta, HoverAnimationDuration);
            _rectTransform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Kill(_rectTransform);
            _rectTransform.sizeDelta = _originalSizeDelta;
        }
    }
}
