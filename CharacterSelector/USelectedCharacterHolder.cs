using System;
using CombatSystem.Entity;
using DG.Tweening;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharacterSelector
{
    public class USelectedCharacterHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        ICombatEntityProviderHolder
    {
        [SerializeField] private Image characterPortraitHolder;
        [SerializeField] private Image roleIconHolder;

        public void InjectIcon(Sprite icon) => roleIconHolder.sprite = icon;

        private void Awake()
        {
            characterPortraitHolder.enabled = false;
        }

        public void InjectPortrait(ICharacterPortraitHolder holder)
        {
            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.FacePivotPosition;
            InjectPortrait(portrait, point);

        }

        private const float AnimationHeightMovement = 16;
        private const float AnimationDuration = .2f;
        private Vector2 _portraitOffset;
        public void InjectPortrait(Sprite portrait, Vector2 offset)
        {
            characterPortraitHolder.sprite = portrait;
            _portraitOffset = offset;

            AnimatePortrait(AnimationHeightMovement);
        }

        private void AnimatePortrait(float punchHeight)
        {
            var portraitTransform = characterPortraitHolder.transform;

            DOTween.Kill(portraitTransform);
            Vector2 punch = new Vector2(0, punchHeight);
            portraitTransform.localPosition = _portraitOffset;
            portraitTransform.DOPunchPosition(punch, AnimationDuration, 1);
        }


        private USelectedCharactersHolder _charactersHolder;
        public void Injection(USelectedCharactersHolder holder) => _charactersHolder = holder;

        [ShowInInspector,InlineEditor(), HideInEditorMode]
        private SPlayerPreparationEntity _entity;
        public void InjectionEntity(SPlayerPreparationEntity entity)
        {
            _entity = entity;
        }
        public ICombatEntityProvider GetEntityProvider() => _entity;

        public void ShowPortrait()
        {
            characterPortraitHolder.enabled = true;
        }

        public void HidePortrait()
        {
            characterPortraitHolder.enabled = false;
        }

        private const float PointerAnimationHeightMovement = -8;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_entity == null) return;
            AnimatePortrait(PointerAnimationHeightMovement);

            // todo characters details
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_entity == null) return;
            // todo characters details
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_entity == null) return;
            _charactersHolder.DisableEntity(this);
            _entity = null;
        }

    }
}
