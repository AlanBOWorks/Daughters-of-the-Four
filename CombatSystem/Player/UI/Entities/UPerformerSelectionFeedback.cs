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

        public void OnPerformerSwitch(in CombatEntity performer)
        {
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

        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            Show();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            Hide();
        }

        public void OnTempoForceFinish(in CombatTeamControllerBase controller, in IReadOnlyList<CombatEntity> remainingMembers)
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
