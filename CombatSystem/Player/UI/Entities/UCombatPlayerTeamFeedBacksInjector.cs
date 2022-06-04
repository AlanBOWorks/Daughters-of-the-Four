using System;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatPlayerTeamFeedBacksInjector : MonoBehaviour
    {
        [SerializeField] private SCombatPlayerTeamFeedBacksTheme defaultAsset;

        private void Awake()
        {
            PlayerCombatVisualsSingleton.CombatTeamFeedBacks = defaultAsset;
        }
    }
}
