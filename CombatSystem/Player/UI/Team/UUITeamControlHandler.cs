using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public sealed class UUITeamControlHandler : MonoBehaviour, ICombatPreparationListener,
        ITeamEventListener
    {
        [Title("Master")]
        [SerializeField] private Image controlBar;
        private float _percentageBarHalfSize;
        [Title("Slaves")]
        [SerializeField] private RectTransform movingPercentageHolder;
        [SerializeField] private TextMeshProUGUI percentageText;

        private void Awake()
        {
            _percentageBarHalfSize = controlBar.rectTransform.rect.width * .5f;
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }


        private void MovePercentageHolder(float percentage)
        {
            percentage = Mathf.Clamp(percentage, -1, 1); //Safe clamp

            Vector2 holderPosition = movingPercentageHolder.anchoredPosition;
            holderPosition.x = _percentageBarHalfSize * percentage;

            movingPercentageHolder.anchoredPosition = holderPosition;
        }

        private void UpdatePercentageText(float percentage)
        {
            percentage *= 100;
            percentageText.text = percentage.ToString("###");
        }

        private void DoPercentageAsLeaderTeam(in CombatTeam team)
        {
            float teamPercentage = team.DataValues.NaturalControl;

            MovePercentageHolder(teamPercentage);
            UpdatePercentageText(teamPercentage);
        }

        private void DoPercentageBasedOnPlayerTeam()
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            DoPercentageAsLeaderTeam(in playerTeam);
        }
        

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
        }

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            if(!isBurst)
                DoPercentageBasedOnPlayerTeam();
        }



        
        [Button]
        private void TestPercentage(float targetPercentage)
        {
            MovePercentageHolder(targetPercentage);
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            DoPercentageAsLeaderTeam(in playerTeam);
        }
    }
}
