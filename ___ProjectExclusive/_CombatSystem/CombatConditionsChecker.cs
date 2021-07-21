using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatConditionsChecker : ICombatAfterPreparationListener
    {
        [ShowInInspector]
        public IWinCombatCondition WinCombatCondition;
        [ShowInInspector]
        public IWinCombatCondition SecondaryWinCondition;
        [ShowInInspector]
        public ILoseCombatCondition LoseCombatCondition;
        [ShowInInspector]
        public ILoseCombatCondition SecondaryLoseCondition;

        public void OnAfterPreparation(
            CombatingTeam playerEntities, 
            CombatingTeam enemyEntities, 
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            var genericConditions = new GenericCombatConditions(playerEntities, enemyEntities);
            WinCombatCondition = genericConditions;
            LoseCombatCondition = genericConditions;
        }

        public enum FinishState
        {
            StillInCombat,
            PlayerWin,
            EnemyWin
        }

        /// <returns>If the Combat was finish</returns>
        public FinishState HandleFinish()
        {
            if (WinCondition())
                return FinishState.PlayerWin;
            if (LoseCondition())
                return FinishState.EnemyWin;

            return FinishState.StillInCombat;
        }


        private bool WinCondition()
        {
            return WinCombatCondition.GetWinCondition() 
                   || 
                   (SecondaryWinCondition != null && SecondaryWinCondition.GetWinCondition());
        }

        private bool LoseCondition()
        {
            return LoseCombatCondition.GetLoseCondition()
                   ||
                   (SecondaryLoseCondition != null && SecondaryLoseCondition.GetLoseCondition());
        }

#if UNITY_EDITOR
        [Button, GUIColor(.3f,.5f,.1f)]
        private void AddDebugConditions()
        {
            var debugCondition = new DebugCombatCondition();
            if (SecondaryWinCondition == null)
                SecondaryWinCondition = debugCondition;
            if (SecondaryLoseCondition == null)
                SecondaryLoseCondition = debugCondition;
        }

        internal class DebugCombatCondition : IWinCombatCondition, ILoseCombatCondition
        {
            public bool isWin;
            public bool isLose;

            public bool GetWinCondition()
            {
                if (!isWin) return false;

                isWin = false; // Do Reset
                return true;

            }

            public bool GetLoseCondition()
            {
                if (!isLose) return false;

                isLose = false; // Do Reset
                return true;
            }
        } 
#endif

    }
}
