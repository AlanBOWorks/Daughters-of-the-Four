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
    public sealed class UPerformerSelectionFeedback : MonoBehaviour, IPlayerEntityListener,
        ITempoTeamStatesListener
    {
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [SerializeField] private RectTransform focusIcon;

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.ManualSubscribe(this);
            playerEvents.DiscriminationEventsHolder.ManualSubscribe(this);
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

        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            Show();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            Hide();
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
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
    }
}
