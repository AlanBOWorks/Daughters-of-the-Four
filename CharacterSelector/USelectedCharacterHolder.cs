using CombatSystem.Entity;
using Lore.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharacterSelector
{
    public class USelectedCharacterHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image characterPortraitHolder;
        [SerializeField] private Image roleIconHolder;

        public void InjectIcon(Sprite icon) => roleIconHolder.sprite = icon;

        public void HandleSelectedCharacterPortrait(ICharacterPortraitHolder holder)
        {
            var portrait = holder.GetCharacterPortraitImage();
            var point = holder.FacePivotPosition;
            InjectPortrait(portrait, point);

        }

        public void InjectPortrait(Sprite portrait, Vector2 offset)
        {
            characterPortraitHolder.sprite = portrait;
            characterPortraitHolder.transform.localPosition = offset;
        }

        private USelectedCharactersHolder _charactersHolder;
        public void Injection(USelectedCharactersHolder holder) => _charactersHolder = holder;


        private SPlayerPreparationEntity _entity;
        public void InjectionEntity(SPlayerPreparationEntity entity)
        {
            _entity = entity;
        }

        public void ShowPortrait()
        {
            characterPortraitHolder.enabled = true;
        }

        public void HidePortrait()
        {
            characterPortraitHolder.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_entity == null) return;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_entity == null) return;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_entity == null) return;
            _charactersHolder.DisableEntityUnsafe(this);
            _entity = null;
        }
    }
}
