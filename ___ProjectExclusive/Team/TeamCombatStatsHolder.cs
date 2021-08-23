using System;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace _Team
{
    public class TeamCombatStatsHolder : ITeamCombatControlStats, 
        IStanceData<ICharacterBasicStatsData>
    {
        private readonly TeamCombatState _state;

        [ShowInInspector]
        public ICharacterArchetypesData<float> ControlLoseOnDeath { get; private set; }
        [ShowInInspector]
        public readonly PositionalStats PositionalStats;
       

        public float LoseControlThreshold;
        public float ReviveTime;
        public int BurstControlLength;
        public int BurstCounterAmount;

        public TeamCombatStatsHolder(TeamCombatState state)
        {
            _state = state;
            PositionalStats = PositionalStats.GenerateStats(state);

            LoseControlThreshold = DefaultLoseThreshold;
            ReviveTime = DefaultReviveTime;
            ControlLoseOnDeath = TeamControlLoses.BackUpData;
        }

        public const float DefaultLoseThreshold = -.6f;
        public const float DefaultReviveTime = 2.5f;
        public TeamCombatStatsHolder(TeamCombatState state, ITeamCombatControlHolder stats)
        {
            _state = state;
            PositionalStats = PositionalStats.GenerateStats(state);

            InjectPreset(stats);
        }


        public void InjectPreset(ITeamCombatControlHolder holder)
        {
            InjectNewStats(holder);
       
            LoseControlThreshold = holder.GetLoseThreshold();
            ReviveTime = holder.GetReviveTime();
            ControlLoseOnDeath = holder.GetControlLosePoints() ?? TeamControlLoses.BackUpData;

            BurstControlLength = holder.GetBurstControlLength();
            BurstCounterAmount = holder.GetBurstCounterAmount();
        }

        private void InjectNewStats(IStanceData<ICharacterBasicStats> stats)
        {
            var attackingStats = stats.AttackingStance;
            var neutralStats = stats.NeutralStance;
            var defendingStats = stats.DefendingStance;
            
            if(attackingStats != null)
                UtilsStats.Add(PositionalStats.AttackingStance, attackingStats);
            if (neutralStats != null)
                UtilsStats.Add(PositionalStats.NeutralStance, neutralStats);
            if (defendingStats != null)
                UtilsStats.Add(PositionalStats.DefendingStance, defendingStats);
        }


        public ICharacterBasicStatsData AttackingStance => PositionalStats.AttackingStance;
        public ICharacterBasicStatsData NeutralStance => PositionalStats.NeutralStance;
        public ICharacterBasicStatsData DefendingStance => PositionalStats.DefendingStance;

        public ICharacterBasicStatsData GetCurrentStanceValue() 
            => PositionalStats.GetCurrentStanceValue();
    }
}
