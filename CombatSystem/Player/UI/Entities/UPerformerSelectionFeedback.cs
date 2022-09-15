using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public sealed class UPerformerSelectionFeedback : MonoBehaviour, 
        IPlayerCombatEventListener,
        ICombatTerminationListener
    {
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [SerializeField] private RectTransform focusIcon;
        [SerializeField] private RectTransform hoverIcon;

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.Subscribe(this);
            Hide();
            hoverIcon.gameObject.SetActive(false); //hover show should active this
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            if(performer == null) return;
            Show();

            var buttons = switcherHandler.GetDictionary();
            
            UCombatEntitySwitchButton targetButton = buttons[performer];
            DoIconAnimation(focusIcon,targetButton.GetIconHolder());
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
        }

        public void OnSwitchButtonHover(Transform onIcon)
        {
            DoIconAnimation(hoverIcon,onIcon);
            hoverIcon.gameObject.SetActive(true);
        }

        public void OnSwitchButtonExit()
        {
            hoverIcon.gameObject.SetActive(false);
        }

        private static void DoIconAnimation(Transform hoverElement, Component targetButton)
        {
            hoverElement.position = targetButton.transform.position;
            DoAnimation(hoverElement);
        }

        private const float AnimationDuration = .2f;
        private static void DoAnimation(Transform hoverElement)
        {
            DOTween.Kill(hoverElement);
            
            hoverElement.localScale = new Vector3(1.1f,1.1f,1.1f);
            hoverElement.DOScale(Vector3.one, AnimationDuration);
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            Hide();
        }

    }
}
