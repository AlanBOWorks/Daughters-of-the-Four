using System;
using CombatSystem._Core;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Others
{
    public class UGameObjectToggleActiveOnCombatState : MonoBehaviour, ICombatStartListener, ICombatTerminationListener
    {
        private void Awake()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        public void OnCombatEnd()
        {
            gameObject.SetActive(false);
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
            gameObject.SetActive(true);
        }
    }
}
