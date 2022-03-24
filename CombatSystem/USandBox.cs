using System;
using CombatSystem.Luck;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class USandBox : MonoBehaviour
    {
        [SerializeField]
        private TestStats testStats = new TestStats();

        [ShowInInspector] private CombatStats testCombatStats;


        [Button]
        private void TestMethod()
        {
            if (testCombatStats == null)
            {
                testCombatStats = new CombatStats(testStats);
            }
           
            float luckValue = UtilsLuck.CalculateLuckInUnit(testCombatStats);
            Debug.Log("Roll: " + luckValue);
        }

        [Serializable]
        private class TestStats : IStats<float>
        {
            [SerializeField]
            private float criticalType;

            private float _concentrationType = 1;


            public float AttackType { get; set; }
            public float OverTimeType { get; set; }
            public float DeBuffType { get; set; }
            public float FollowUpType { get; set; }
            public float HealType { get; set; }
            public float ShieldingType { get; set; }
            public float BuffType { get; set; }
            public float ReceiveBuffType { get; set; }
            public float HealthType { get; set; }
            public float MortalityType { get; set; }
            public float DamageReductionType { get; set; }
            public float DeBuffResistanceType { get; set; }
            public float ActionsType { get; set; }
            public float SpeedType { get; set; }
            public float ControlType { get; set; }

            public float CriticalType
            {
                get => criticalType;
                set => criticalType = value;
            }

            public float OffensiveType { get; set; }
            public float SupportType { get; set; }
            public float VitalityType { get; set; }

            public float ConcentrationType
            {
                get => _concentrationType;
                set => _concentrationType = value;
            }
        }
    }

}
