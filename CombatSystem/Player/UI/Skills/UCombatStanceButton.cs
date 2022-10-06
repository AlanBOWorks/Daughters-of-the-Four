using System;
using CombatSystem.Localization;
using CombatSystem.Team;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatSystem.Player.UI.Skills
{
    public class UCombatStanceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Title("Button")]
        [SerializeField] 
        private Image backgroundHolder;
        [SerializeField] 
        private Image backgroundIcon;
        [SerializeField]
        private TextMeshProUGUI stanceName;
        [SerializeField]
        private EnumTeam.StanceFull buttonStance;

        [Title("Shortcut's")] 
        [SerializeField] private TextMeshProUGUI shortcutText;


        [SerializeReference]
        private ColorsHolder initialColorsHolder = new ColorsHolder();
        private Color _activeColor;
        private float _initialFontSize;

        private UCombatControlStanceHandler _handler;

        private const float BackgroundIconAlpha = .6f;
       
        private void Awake()
        {
            LocalizationsCombat.LocalizeStance(stanceName.name);
            _initialFontSize = stanceName.fontSize;
        }

        private void OnDisable()
        {
            DeActivate();
        }

        public void Injection(Color roleColor)
        {
            _activeColor = roleColor;
        }

        public void Injection(Sprite roleIcon)
        {
            backgroundIcon.sprite = roleIcon;
        }

        public void Injection(UCombatControlStanceHandler handler) => _handler = handler;

        private void StopTextAnimations()
        {
            DOTween.Kill(stanceName);
            stanceName.fontSize = _initialFontSize;
        }

        private void StopBackgroundAnimations()
        {
            DOTween.Kill(backgroundHolder);
            DOTween.Kill(backgroundIcon);

            backgroundHolder.color = initialColorsHolder.GetInitialBackgroundColor();
            backgroundIcon.color = CalculateIconColor(initialColorsHolder.GetInitialMainColor());
        }
        private static Color CalculateIconColor(Color fromColor)
        {
            return new Color(fromColor.r, fromColor.g, fromColor.b, BackgroundIconAlpha);
        }



        private const float OnAnimateIncrementFontAmount = 2;
        private const float AnimationDuration = .4f;
        private void DoSizeAnimation()
        {
            StopTextAnimations();
            var targetFontSize = _initialFontSize + OnAnimateIncrementFontAmount;
            stanceName.DOFontSize(targetFontSize, AnimationDuration);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            DoSizeAnimation();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            StopTextAnimations();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            DoPointerDown();
        }
        public void OnPointerDown(InputAction.CallbackContext context)
        {
            DoPointerDown();
        }

        private void DoPointerDown()
        {
            _handler.DoSwitchStance(buttonStance);
        }

        public void InjectShortcutName(string shortcutName)
        {
            shortcutText.text = shortcutName;
        }

        public void DoActiveButton()
        {
            var mainColor = initialColorsHolder.GetInitialMainColor();
            stanceName.color = mainColor;
            shortcutText.color = mainColor;

            StopBackgroundAnimations();
            backgroundHolder.DOColor(_activeColor, AnimationDuration);
            var iconTargetColor = CalculateIconColor(mainColor);
            backgroundIcon.DOColor(iconTargetColor, AnimationDuration);

        }
        public void DeActivate()
        {
            StopTextAnimations();
            StopBackgroundAnimations();

            var initialColor = initialColorsHolder.GetInitialMainColor();
            stanceName.color = initialColor;
            shortcutText.color = initialColor;
        }

        [Serializable]
        private sealed class ColorsHolder
        {
            [SerializeField] private Color initialMainColor;
            [SerializeField] private Color initialBackgroundColor;

            public Color GetInitialMainColor() => initialMainColor;
            public Color GetInitialBackgroundColor() => initialBackgroundColor;
        }
    }
}
