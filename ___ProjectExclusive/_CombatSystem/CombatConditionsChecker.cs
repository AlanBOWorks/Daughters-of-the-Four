using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatConditionsChecker : ITemporalStatsChangeListener
    {
        [ShowInInspector]
        public IWinCombatCondition WinCombatCondition;
        [ShowInInspector]
        public ILoseCombatCondition LoseCombatCondition;

        public void OnTemporalStatsChange(ICombatTemporalStats currentStats)
        {
            if (WinCombatCondition.GetWinCondition())
            {
                //TODO invoke WIN

#if UNITY_EDITOR
                Debug.Log("XXXX - WINNER - XXXX");
#endif

                return;
            }

            if (LoseCombatCondition.GetLoseCondition())
            {
#if UNITY_EDITOR
                Debug.Log("XXXX - LOSER - XXXX");
#endif
                //TODO invoke LOSE
                return;
            }
        }

    }
}
