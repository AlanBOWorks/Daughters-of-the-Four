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
    public class PositionalStats : PositionalStructureBase<IBasicStats>
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
        /// <inheritdoc cref="PositionalStructureBase{T}.GenerateProvisionalGeneric"/>
        /// <br></br>_____<br></br>
        /// Additionally initialize three [<seealso cref="CombatStatsBasic"/>] in each
        /// respective position
        /// </summary>
        public static PositionalStats GenerateProvisionalBasics()
            => new PositionalStats(NeutralProvisionalProvider);
    }
}
