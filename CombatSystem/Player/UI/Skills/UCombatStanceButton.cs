using System;
using CombatSystem.Localization;
using CombatSystem.Team;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CombatSystem.Player.UI.Skills
{
    public class UCombatStanceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] 
        private Image backgroundHolder;
        [SerializeField] 
        private TextMeshProUGUI stanceName;
        [SerializeField]
        private EnumTeam.StanceFull buttonStance;
        
        private Color _initialColor;
        private Color _activeColor;
        private Color _backgroundInitialColor;
        private float _initialFontSize;

        private UCombatControlStanceHandler _handler;

        private void Awake()
        {
            CombatLocalizations.LocalizeStance(stanceName.name);
            _initialColor = stanceName.color;
            _initialFontSize = stanceName.fontSize;
            _backgroundInitialColor = backgroundHolder.color;
        }

        private void OnDisable()
        {
            DeActivate();
        }

        public void Injection(Color roleColor)
        {
            _activeColor = roleColor;
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
            backgroundHolder.color = _backgroundInitialColor;
        }


        private const float OnAnimateIncrementFontAmount = 2;
        private const float AnimationDuration = .2f;
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
            _handler.DoSwitchStance(buttonStance);
        }

        public void DoActiveButton()
        {
            stanceName.color = _backgroundInitialColor;
            StopBackgroundAnimations();
            backgroundHolder.DOColor(_activeColor, AnimationDuration);

        }
        public void DeActivate()
        {
            StopTextAnimations();
            StopBackgroundAnimations();
            stanceName.color = _initialColor;
        }
    }
}
