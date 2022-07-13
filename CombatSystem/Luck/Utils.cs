using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Luck
{
    public static class UtilsLuck
    {
        public const float LuckDiceModifier = .1f;

        public const int LuckDiceLow = 0;
        public const int LuckDiceHigh = 10;



        public static float CalculateLuckInUnit(in CombatStats entityStats)
        {
            float diceUnitValue = RollDiceInUnit();

            return CalculateLuckInUnit(in entityStats, in diceUnitValue);
        }

        public static float CalculateLuckInUnit(in CombatStats entityStats, in float rollInUnit)
        {
            float luckModifier = UtilsStatsFormula.CalculateLuckAmount(entityStats);
            return luckModifier * rollInUnit;
        }
        


        public static DiceValues RolDice()
        {
            int roll = RollDice();
            float rolInUnits = DiceRolModifiedInUnit(roll);

            return new DiceValues(roll, rolInUnits);
        }


        public static int RollDice()
        {
            const int luckDiceHighValue = LuckDiceHigh + 1; //high roll is exclusive
            return Random.Range(LuckDiceLow, luckDiceHighValue);
        }

        public static float RollDiceInUnit()
        {
            int diceRoll = RollDice();
            return DiceRolModifiedInUnit(diceRoll);
        }

        private static float DiceRolModifiedInUnit(int diceRoll) => diceRoll * LuckDiceModifier;
    }
}
