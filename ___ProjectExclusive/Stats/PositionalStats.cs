using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{
    public abstract class PositionalStats<T> : IStanceData<T>, IStanceElement<T>
    {
        protected PositionalStats(IStanceProvider stanceProvider)
        {
            _stanceProvider = stanceProvider;
        }
        protected PositionalStats(IStanceProvider stanceProvider, T attackingStance, T neutralStance, T defendingStance)
        : this(stanceProvider)
        {
            AttackingStance = attackingStance;
            NeutralStance = neutralStance;
            DefendingStance = defendingStance;
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
    }

    /// <summary>
    /// [<see cref="ICharacterBasicStats"/>] based by the [<seealso cref="EnumTeam.Stances"/>]<br></br>
    /// Get mainly by [<see cref="PositionalStats{T}.GetCurrentStanceValue"/>]
    /// </summary>
    public class PositionalStats : PositionalStats<ICharacterBasicStats>
    {
        public PositionalStats(IStanceProvider stanceProvider) : base(stanceProvider)
        {}

        public PositionalStats(IStanceProvider stanceProvider,
            ICharacterBasicStats attackingStance, 
            ICharacterBasicStats neutralStance, 
            ICharacterBasicStats defendingStance) 
            : base(stanceProvider, attackingStance, neutralStance, defendingStance)
        {}

        public static PositionalStats GenerateStats(IStanceProvider stanceProvider)
        {
            return new PositionalStats(stanceProvider,
                new CharacterCombatStatsBasic(),
                new CharacterCombatStatsBasic(),
                new CharacterCombatStatsBasic());
        }
    }
}
