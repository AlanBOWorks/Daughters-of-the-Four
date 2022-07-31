using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharacterSelector
{
    public class UAllowRepetitionCharacterHandler : MonoBehaviour, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] 
        private USelectedCharactersHolder charactersHolder;

        [SerializeField] private Image checkImageHolder;

        private Color _checkIconColor;
        private void Start()
        {
            _checkIconColor = checkImageHolder.color;
            UpdateCheckImage();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            bool allowRepetition = !charactersHolder.AllowRepetition;
            charactersHolder.SetAllowRepetition(allowRepetition);
            UpdateCheckImage(allowRepetition);
        }

        private const float OnHoverIconAlpha = .4f;
        private void UpdateCheckImage()
        {
            UpdateCheckImage(charactersHolder.AllowRepetition);
        }
        private void UpdateCheckImage(bool active)
        {
            Color targetColor = active 
                ? _checkIconColor 
                : GenerateInactiveColor();

            checkImageHolder.color = targetColor;
        }

        private bool _isHover;
        private Color GenerateInactiveColor()
        {
            float alpha = _isHover ? OnHoverIconAlpha : 0;
            return new Color(_checkIconColor.r,_checkIconColor.g,_checkIconColor.b, alpha);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHover = true;
            UpdateCheckImage();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHover = false;
            UpdateCheckImage();
        }
    }
}
