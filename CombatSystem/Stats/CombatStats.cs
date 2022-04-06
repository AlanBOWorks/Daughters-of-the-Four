using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    public class CombatStats : ICombatStats<float>, IMainStats<float>, 
        IStatsTypesRead<IBasicStatsRead<float>>
    {
        public CombatStats(IStatsRead<float> baseStats)
        {
            BaseStats = baseStats;
            BuffStats = new StatsBase<float>(0);
            BurstStats = new BurstStats();

            // --------------------- 
            // Master's assignation
            // --------------------- 
            OffensiveType = 1;
            SupportType = 1;
            VitalityType = 1;
            ConcentrationType = 1;
            UtilsStats.DoCopyMaster(this, baseStats);
            

            // --------------------- 
            // Special case's assignation
            // --------------------- 
 
            float heathAmount = UtilsStatsFormula.CalculateMaxHealth(this);
            float mortalityAmount = UtilsStatsFormula.CalculateMaxMortality(this);

            CurrentHealth = heathAmount;
            CurrentMortality = mortalityAmount;
        }



        // Base stats comes from predefined data structures that can't be altered, that's why it only readable
        [ShowInInspector, InfoBox("This is a reference type; it could modify an SO values", InfoMessageType.Warning)]
        public readonly IStatsRead<float> BaseStats;
        public readonly StatsBase<float> BuffStats;
        public readonly BurstStats BurstStats;

        /// <summary>
        /// It's just the [<see cref="IStatsTypesRead{T}"/>] implementation for [<see cref="BaseStats"/>]
        /// </summary>
        public IBasicStatsRead<float> BaseType => BaseStats;
        /// <summary>
        /// It's just the [<see cref="IStatsTypesRead{T}"/>] implementation for [<see cref="BuffStats"/>]
        /// </summary>
        public IBasicStatsRead<float> BuffType => BuffStats;
        /// <summary>
        /// It's just the [<see cref="IStatsTypesRead{T}"/>] implementation for [<see cref="BurstStats"/>]
        /// </summary>
        public IBasicStatsRead<float> BurstType => BurstStats;


        public bool CanModifyInitiative()
        {
            // Negative Speed Entities are inactive entities (obstacles for example)
            // Zero Speed are entities that can Act but requires giving initiative to them (by buff, punish skills, etc)
            return BaseStats.SpeedType > 0;
        }

        public void SetToFullInitiative()
        {
            if (!CanModifyInitiative()) return;

            CurrentInitiative = TempoTicker.LoopThreshold;
        }

        [Title("Volatile")]
        [ShowInInspector]
        public float CurrentHealth { get; set; }
        [ShowInInspector]
        public float CurrentMortality { get; set; }

        [ShowInInspector]
        public float CurrentShields { get; set; }

        [ShowInInspector]
        public float UsedActions { get; set; }
        [ShowInInspector]
        public float CurrentInitiative { get; set; }

        [Title("Masters")]
        [ShowInInspector]
        public float OffensiveType { get; set; }
        [ShowInInspector]
        public float SupportType { get; set; }
        [ShowInInspector]
        public float VitalityType { get; set; }
        [ShowInInspector]
        public float ConcentrationType { get; set; }
        

        public bool IsAlive()
        {
            return CurrentHealth > 0 || CurrentMortality > 0;
        }


        public void OnSequenceStart()
        {
            BurstStats.OnEntityStartSequence();
        }
        public void OnSequenceFinish()
        {
            BurstStats.OnEntityFinishSequence();
        }
    }
}
