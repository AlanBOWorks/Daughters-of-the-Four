using System;
using CombatSystem._Core;
using CombatSystem.Team;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI.Skills
{
    public sealed class UCombatControlStanceHandler : MonoBehaviour, 
        ICombatStartListener,
        ITeamEventListener
    {
        [SerializeField] private TextMeshProUGUI currentControlText;
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeForCombatStart(this);
            playerEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(this);
            playerEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
        }

        public void OnControlChange(CombatTeam team, float phasedControl)
        {
            var teamControl = team.DataValues.CurrentControl;
            currentControlText.text = teamControl.ToString("P0");
            
            if (teamControl < 1) return;
            EnableStanceSwitching();
        }

        private const float OnDisableAlphaAmount = .4f;
        public void DisableStanceSwitching()
        {
            canvasGroup.alpha = OnDisableAlphaAmount;

        }

        public void EnableStanceSwitching()
        {
            canvasGroup.alpha = 1;

        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            OnControlChange(playerTeam, 0);
            DisableStanceSwitching();
        }

        public void OnCombatStart()
        {
        }
    }
}
