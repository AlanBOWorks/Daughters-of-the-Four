using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public class CombatFinishHandler : ICombatFinishHandler
    {
        private CombatTeam _playerTeam;
        private CombatTeam _enemyTeam;
      

        public bool IsCombatFinish()
        {
            return IsEnemyTeamDefeat() || IsPlayerTeamDefeat();
        }

        public bool CheckIfPlayerWon()
        {
            return IsEnemyTeamDefeat();
        }

        private bool IsPlayerTeamDefeat()
        {
            foreach (var entity in _playerTeam.GetAllMembers())
            {
                if (UtilsCombatStats.IsAlive(entity)) return false;
            }

            return true;
        }

        private bool IsEnemyTeamDefeat()
        {
            foreach (var entity in _enemyTeam.GetMainRoles())
            {
                if (UtilsCombatStats.IsAlive(entity)) return false;
            }

            return true;
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }
    }

    public interface ICombatFinishHandler : ICombatPreparationListener
    {
        bool IsCombatFinish();
        bool CheckIfPlayerWon();
    }

    public interface ICombatTerminationListener : ICombatEventListener
    {
        /// <summary>
        /// The very first call finishing the combat (nothing is hidden). <br></br>
        /// Use it for animations, fade out things and play the Win/Lose popUp
        /// </summary>
        /// <param name="finishType"></param>
        void OnCombatFinish(UtilsCombatFinish.FinishType finishType);

        /// <summary>
        /// Invoked once the combat is hidden behind of the LoadScreen
        /// </summary>
        /// <param name="finishType"></param>
        void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType);
    }

    public static class UtilsCombatFinish 
    {
        public enum FinishType
        {
            QuitRun,
            PlayerWon,
            EnemyWon
        }

        public static FinishType GetEnumByBoolean(bool isPlayerWin)
        {
            return isPlayerWin ? FinishType.PlayerWon : FinishType.EnemyWon;
        }

        public static void FinishCombat(FinishType finishType)
        {
            CombatCoroutinesTracker.KillCombatCoroutines();
        }
    }
}
