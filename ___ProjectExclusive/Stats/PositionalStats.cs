using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    /// <summary>
    /// [<see cref="ICombatStatsBasic"/>] based by the [<seealso cref="EnumTeam.Stances"/>]<br></br>
    /// Get mainly by [<see cref="PositionalStats{T}.GetCurrentStanceValue"/>]
    /// </summary>
    public class PositionalStats : PositionalStats<float>, IBasicStatsData<float>
    {
       
        /// <summary>
        /// Creates and initialize this with <seealso cref="CombatStatsBasic"/>
        /// </summary>
        public PositionalStats(IStanceProvider stanceProvider) : 
            base(stanceProvider, new CombatStatsBasic(),
                new CombatStatsBasic(), new CombatStatsBasic())
        { }
        
        public float AttackPower => GetCurrentStanceValue().AttackPower;
        public float DeBuffPower => GetCurrentStanceValue().DeBuffPower;
        public float StaticDamagePower => GetCurrentStanceValue().StaticDamagePower;
        public float HealPower => GetCurrentStanceValue().HealPower;
        public float BuffPower => GetCurrentStanceValue().BuffPower;
        public float BuffReceivePower => GetCurrentStanceValue().BuffReceivePower;
        public float MaxHealth => GetCurrentStanceValue().MaxHealth;
        public float MaxMortalityPoints => GetCurrentStanceValue().MaxMortalityPoints;
        public float DamageReduction => GetCurrentStanceValue().DamageReduction;
        public float DeBuffReduction => GetCurrentStanceValue().DeBuffReduction;
        public float DisruptionResistance => GetCurrentStanceValue().DisruptionResistance;
        public float CriticalChance => GetCurrentStanceValue().CriticalChance;
        public float SpeedAmount => GetCurrentStanceValue().SpeedAmount;
        public float InitiativePercentage => GetCurrentStanceValue().InitiativePercentage;
        public float ActionsPerInitiative => GetCurrentStanceValue().ActionsPerInitiative;
        public float HarmonyAmount => GetCurrentStanceValue().HarmonyAmount;
    }

    public class PositionalStats<T> : PositionalStructureBase<IBasicStats<T>>
    {
        public PositionalStats(IStanceProvider stanceProvider,
            IBasicStats<T> attackingStance,
            IBasicStats<T> neutralStance,
            IBasicStats<T> defendingStance)
            : base(stanceProvider, attackingStance, neutralStance, defendingStance)
        { }


        /// <summary>
        /// <inheritdoc cref="PositionalStructureBase{T}.GenerateProvisionalGeneric"/>
        /// <br></br>_____<br></br>
        /// Additionally initialize three [<seealso cref="CombatStatsBasic"/>] in each
        /// respective position
        /// </summary>
        public static PositionalStats GenerateProvisionalBasics()
            => new PositionalStats(NeutralProvisionalProvider);
    }
}
