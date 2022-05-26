using System;
using CombatSystem.Player.Events;
using Common;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatPauseControlHandler : MonoBehaviour, ICombatPauseListener
    {
        [SerializeField] private UPauseMenuControl menuControl;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.ManualSubscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.ManualUnSubscribe(this);
        }

        public void OnCombatPause()
        {
            menuControl.ShowMenu();
        }

        public void OnCombatResume()
        {
            menuControl.HideMenu();
        }
    }
}
