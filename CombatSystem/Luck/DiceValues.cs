using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Luck
{
    public sealed class DiceValuesHolder
    {
        public DiceValuesHolder(CombatStats calculationsReference)
        {
            _calculationsReference = calculationsReference;
        }

        private readonly CombatStats _calculationsReference;

        [ShowInInspector]
        public DiceValues Values { get; private set; }
        [ShowInInspector, SuffixLabel("%")]
        public float PercentageRoll { get; private set; }

        public void RollDice()
        {
            Values = UtilsLuck.RolDice();
            float rolInUnit = Values.UnitValue;
            PercentageRoll = UtilsLuck.CalculateLuckInUnit(in _calculationsReference, in rolInUnit);
        }

       
    }

    public struct DiceValues
    {
        public DiceValues(int rolValue, float unitValue)
        {
            RolValue = rolValue;
            UnitValue = unitValue;
        }

        /// <summary>
        /// Pre made values for Max or Min rolls; Useful por skills that forces those values.<br></br>
        /// See also:<br></br>
        /// Max: [<seealso cref="UtilsLuck.LuckDiceHigh"/>] <br></br>
        /// Min: [<seealso cref="UtilsLuck.LuckDiceLow"/>]
        /// </summary>
        public DiceValues(bool isMaxRoll) 
        {
            if (isMaxRoll)
            {
                RolValue = UtilsLuck.LuckDiceHigh;
                UnitValue = 1;
            }
            else
            {
                RolValue = UtilsLuck.LuckDiceLow;
                UnitValue = 0;
            }
        }



        [PropertyRange(0, 20)]
        public readonly int RolValue;
        [SuffixLabel("%"), PropertyRange(0, 1)]
        public readonly float UnitValue;

    }
}
