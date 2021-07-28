using System;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Skills
{

    public class CriticalActionBuff : AfterBuff, IStackableBuff
    {
        [ShowInInspector]
        private float _incrementCount;

        public static TempoHandler.TickType GetKeyType() => TempoHandler.TickType.OnBeforeSequence;
        public override TempoHandler.TickType GetTickType() => GetKeyType();


        public const int ActionIncrement = 1;
        public const float InitiativeIncrement = .05f;
        public override void DoBuff(CombatingEntity onEntity)
        {
            var stats = onEntity.CombatStats;
            UtilsCombatStats.AddActionAmount(stats,ActionIncrement);
            var initiativeAdd = InitiativeIncrement * (1 + _incrementCount);
            UtilsCombatStats.AddInitiative(stats,initiativeAdd);
        }

        public void Increment(float amount)
        {
            _incrementCount++;
        }

        public static void AddCriticalBuff(CombatingEntity entity)
        {
            TickerBuffHolders buffHolders = entity.TickerBuffHolders;
            Type type = typeof(CriticalActionBuff);

            if (buffHolders.HasStackableBuff(type))
            {
                buffHolders.IncrementBuff(type);
            }
            else
            {
                var buff = new CriticalActionBuff();
                buffHolders.EnqueueBuff(buff, TempoHandler.TickType.OnBeforeSequence);
            }
        }
    }

}
