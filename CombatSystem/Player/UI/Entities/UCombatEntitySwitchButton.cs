using System;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.UI;

namespace CombatSystem.Player.UI
{
    public class UCombatEntitySwitchButton : UButtonPointerFeedback
    {
        [Title("Button")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [SerializeField] private Image iconHolder;
        [ShowInInspector,HideInEditorMode]
        private CombatEntity _user;

        public Image GetIconHolder() => iconHolder;

        public void Injection(in CombatEntity entity) => _user = entity;

        public void Injection(in Sprite icon)
        {
            iconHolder.sprite = icon;
        }


        private const float OnDisableAlpha = .3f;
        private const float OnNullAlpha = .1f;
        public void DoEnable(bool enableButton)
        {
            float targetAlpha = enableButton
                ? 1
                : OnDisableAlpha;
            canvasGroup.alpha = targetAlpha;
        }

        public void OnNullEntity()
        {
            enabled = false;
            canvasGroup.alpha = OnNullAlpha;
            _user = null;
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(_user == null) return;

            switcherHandler.DoSwitchEntity(_user);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            switcherHandler.OnHoverEnter(iconHolder);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            switcherHandler.OnHoverExit();
        }
    }
}
