using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using DG.Tweening;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class UPerformerSelectionFeedback : MonoBehaviour, 
        IPlayerEntityListener,
        ICombatStatesListener
    {
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [SerializeField] private RectTransform focusIcon;

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.Subscribe(this);
            Hide();
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            if(performer == null) return;

            var buttons = switcherHandler.GetDictionary();
            ;
            var targetButton = buttons[performer];
            SwitchFocus(in targetButton);
        }
        
        private void SwitchFocus(in UCombatEntitySwitchButton targetButton)
        {
            focusIcon.position = targetButton.transform.position;
            DoAnimation();
        }

        private const float AnimationDuration = .2f;
        private void DoAnimation()
        {
            DOTween.Kill(this);


            focusIcon.rotation = Quaternion.AngleAxis(-10, Vector3.forward); 
            focusIcon.DORotateQuaternion(Quaternion.identity, AnimationDuration);
        }
        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
        }

        private void Show()
        {
            focusIcon.gameObject.SetActive(true);
        }
        private void Hide()
        {
            focusIcon.gameObject.SetActive(false);
        }

        public void OnCombatEnd()
        {
            Hide();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
            Show();
        }
    }
}
