using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI.Skills
{
    public sealed class UCombatControlStanceHandler : MonoBehaviour, 
        ICombatStartListener,
        ITeamEventListener, IPlayerCombatEventListener,

        IStanceStructureRead<UCombatStanceButton>
    {
        [SerializeField] private TextMeshProUGUI currentControlText;
        [SerializeField] private CanvasGroup canvasGroup;

        [Title("Stance References")] 
        [SerializeField] private UCombatStanceButton attackerButton;
        [SerializeField] private UCombatStanceButton supportButton;
        [SerializeField] private UCombatStanceButton vanguardButton;

        public UCombatStanceButton AttackingStance => attackerButton;
        public UCombatStanceButton SupportingStance => supportButton;
        public UCombatStanceButton DefendingStance => vanguardButton;


        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeForCombatStart(this);
            playerEvents.SubscribeAsPlayerEvent(this);
            playerEvents.DiscriminationEventsHolder.Subscribe(this);

            HandleStanceInitializations();
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(this);
            playerEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        private void HandleStanceInitializations()
        {
            var stanceTheme = CombatThemeSingleton.RolesThemeHolder;
            var enumerable = UtilsTeam.GetEnumerable(stanceTheme, this);
            foreach ((CombatThemeHolder theme, UCombatStanceButton button) in enumerable)
            {
                button.Injection(this);
                button.Injection(theme.GetThemeColor());
            }
        }

        public void DoSwitchStance(EnumTeam.StanceFull targetStance)
        {
            if(!enabled) return;

            PlayerCombatSingleton.StanceSwitcher.DoSaveStance(targetStance);
        }

        private UCombatStanceButton _currentButton;

        public void OnPerformerSwitch(CombatEntity performer)
        {
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            var targetButton = UtilsTeam.GetElement(targetStance, this);
            if (_currentButton == targetButton) return;

            if (_currentButton != null) _currentButton.DeActivate();

            _currentButton = targetButton;
            _currentButton.DoActiveButton();
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
            _currentButton = null;

            OnControlChange(playerTeam, 0);
            DisableStanceSwitching();
            var targetStance = playerTeam.DataValues.CurrentStance;
            DoSwitchStance(targetStance);
        }

        public void OnCombatStart()
        {
        }

    }
}
