using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public abstract class SCombatConditions : ScriptableObject
    {
        public abstract bool GetTriggerCondition();
    }

    public class GenericCombatConditions : IWinCombatCondition, ILoseCombatCondition
    {
        private readonly CombatingTeam _playerTeam;
        private readonly CombatingTeam _enemyTeam;

        public GenericCombatConditions(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }

        public bool GetWinCondition()
        {
            foreach (CombatingEntity entity in _enemyTeam)
            {
                if (entity.IsAlive())
                    return false;
            }

            return true;
        }

        public bool GetLoseCondition()
        {
            foreach (CombatingEntity entity in _playerTeam)
            {
                if (entity.IsAlive())
                    return false;
            }

            return true;
        }
    }


    public interface ICombatConditionHolder : IWinCombatCondition, ILoseCombatCondition
    {
        bool IsWinConditionValid();
        bool IsLoseConditionValid();
    }


    public interface IWinCombatCondition
    {
        bool GetWinCondition();
    }

    public interface ILoseCombatCondition
    {
        bool GetLoseCondition();
    }


}
