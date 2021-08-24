using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public class PositionalStats<T> : IStanceData<T>, IStanceElement<T>
    {
        public static IStanceProvider NeutralProvisionalProvider = new ProvisionalProvider();

        protected PositionalStats(IStanceProvider stanceProvider)
        {
            _stanceProvider = stanceProvider;
        }

        protected PositionalStats(IStanceProvider stanceProvider, T attackingStance, T neutralStance, T defendingStance)
        {
            AttackingStance = attackingStance;
            NeutralStance = neutralStance;
            DefendingStance = defendingStance;
            _stanceProvider = stanceProvider;
        }

        public void Injection(IStanceProvider stanceProvider)
            => _stanceProvider = stanceProvider;

        private IStanceProvider _stanceProvider;
        [ShowInInspector]
        public T AttackingStance { get; }
        [ShowInInspector]
        public T NeutralStance { get; }
        [ShowInInspector]
        public T DefendingStance { get; }

        public T GetCurrentStanceValue() 
            => UtilsTeam.GetElement(this, _stanceProvider.CurrentStance);

        private class ProvisionalProvider : IStanceProvider
        {
            public EnumTeam.Stances CurrentStance => EnumTeam.Stances.Neutral;
        }

        /// <summary>
        /// Creates a [<see cref="PositionalStats"/>] with a provisional [<see cref="IStanceProvider"/>]
        /// that only returns(<see cref="EnumTeam.Stances.Neutral"/>).
        /// <br></br><br></br>
        /// Should be updated with an [<see cref="IStanceProvider"/>] by [<see cref="Injection"/>]
        /// </summary>
        public static PositionalStats<T> GenerateProvisionalGeneric() 
            => new PositionalStats<T>(NeutralProvisionalProvider);

    }

    /// <summary>
    /// [<see cref="ICombatStatsBasic"/>] based by the [<seealso cref="EnumTeam.Stances"/>]<br></br>
    /// Get mainly by [<see cref="PositionalStats{T}.GetCurrentStanceValue"/>]
    /// </summary>
    public class PositionalStats : PositionalStats<IBasicStats>
    {
       
        /// <summary>
        /// Creates and initialize this with <seealso cref="CombatStatsBasic"/>
        /// </summary>
        public PositionalStats(IStanceProvider stanceProvider) : 
            base(stanceProvider, new CombatStatsBasic(),
                new CombatStatsBasic(), new CombatStatsBasic())
        { }

        public PositionalStats(IStanceProvider stanceProvider,
            IBasicStats attackingStance, 
            IBasicStats neutralStance, 
            IBasicStats defendingStance) 
            : base(stanceProvider, attackingStance, neutralStance, defendingStance)
        {}

        /// <summary>
        /// <inheritdoc cref="PositionalStats{T}.GenerateProvisionalGeneric"/>
        /// <br></br>_____<br></br>
        /// Additionally initialize three [<seealso cref="CombatStatsBasic"/>] in each
        /// respective position
        /// </summary>
        public static PositionalStats GenerateProvisionalBasics()
            => new PositionalStats(NeutralProvisionalProvider);
    }
}
