using System;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatPlayerTeamFeedBacksInjector : MonoBehaviour
    {
        [SerializeField] private SCombatPlayerTeamFeedBacks defaultAsset;

        private void Awake()
        {
            PlayerCombatUserInterfaceSingleton.CombatTeemFeedBacks = defaultAsset;
        }
    }
}