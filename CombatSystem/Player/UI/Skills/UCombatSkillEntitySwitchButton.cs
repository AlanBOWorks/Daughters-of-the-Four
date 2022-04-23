using System;
using CombatSystem.Entity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillEntitySwitchButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UCombatSkillEntitySwitcher switcherHandler;
        private CombatEntity _user;


        public void Injection(in CombatEntity entity) => _user = entity; 

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
