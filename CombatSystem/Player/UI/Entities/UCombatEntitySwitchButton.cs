using System;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UCombatEntitySwitchButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [SerializeField] private Image iconHolder;
        [ShowInInspector]
        private CombatEntity _user;


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
            enabled = enableButton;
        }

        public void OnNullEntity()
        {
            enabled = false;
            canvasGroup.alpha = OnNullAlpha;
            _user = null;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            switcherHandler.DoSwitchEntity(in _user);
        }
    }
}
