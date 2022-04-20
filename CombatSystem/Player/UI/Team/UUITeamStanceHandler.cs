using System;
using CombatSystem.Team;
using Localization.Combat;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUITeamStanceHandler : UTeamDiscriminatorListener<UUITeamStanceHandler.StanceHandler>
    {
        protected override void OnCombatPrepares(in StanceHandler element, in CombatTeam team)
        {
            var stance = team.DataValues.CurrentStance;
            element.UpdateStanceText(in stance);
        }

        protected override void OnStanceChange(in StanceHandler element, in EnumTeam.StanceFull switchStance)
        {
            element.UpdateStanceText(in switchStance);
        }

        protected override void OnControlChange(in StanceHandler element, in float phasedControl, in bool isBurst)
        {
        }


        [Serializable]
        public sealed class StanceHandler
        {
            [SerializeField] private TextMeshProUGUI stanceText;

            public void UpdateStanceText(in EnumTeam.StanceFull stance)
            {
                var stanceString = stance.ToString();
                stanceString = CombatLocalizations.LocalizeStance(in stanceString);

                stanceText.text = stanceString;
            }
        }
    }
}
