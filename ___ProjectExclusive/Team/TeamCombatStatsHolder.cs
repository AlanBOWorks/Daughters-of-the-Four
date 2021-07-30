using System;
using Characters;
using Passives;
using Skills;
using UnityEngine;

namespace _Team
{
    public class TeamCombatStatsHolder : ITeamCombatControlStats,
        IStanceArchetype<CompositeStats>
    {
        private readonly TeamCombatData _data;

        public CompositeStats AttackingStats { get; private set; }
        public  CompositeStats NeutralStats { get; private set; }
        public CompositeStats DefendingStats { get; private set; }
        public FilterPassivesHolder OnAttackPassives { get; private set; }
        public FilterPassivesHolder OnNeutralPassives { get; private set; }
        public FilterPassivesHolder OnDefendingPassives { get; private set; }

        public TeamCombatStatsHolder(TeamCombatData data)
        {
            _data = data;
            AttackingStats = new CompositeStats();
            NeutralStats = new CompositeStats();
            DefendingStats = new CompositeStats();
            OnAttackPassives = new FilterPassivesHolder();
            OnNeutralPassives = new FilterPassivesHolder();
            OnDefendingPassives = new FilterPassivesHolder();
        }

        public TeamCombatStatsHolder(TeamCombatData data, IStanceArchetype<ICharacterBasicStats> stats)
        {
            _data = data;
            InjectNewStats(stats);
        }

        public TeamCombatStatsHolder(TeamCombatData data, IStanceArchetype<ICharacterBasicStats> stats,
            IStanceArchetype<FilterPassivesHolder> passives)
        : this(data, stats)
        {
            InjectNewPassives(passives);
        }

        public void InjectPreset(ITeamCombatControlHolder holder)
        {
            InjectNewStats(holder);
            InjectNewPassives(holder);
        }

        public void InjectNewStats(IStanceArchetype<ICharacterBasicStats> stats)
        {
            AttackingStats = new CompositeStats(stats.GetAttacking());
            NeutralStats = new CompositeStats(stats.GetNeutral());
            DefendingStats = new CompositeStats(stats.GetDefending());
        }

        public void InjectNewPassives(IStanceArchetype<FilterPassivesHolder> passives)
        {
            OnAttackPassives = new FilterPassivesHolder(passives.GetAttacking());
            OnNeutralPassives = new FilterPassivesHolder(passives.GetNeutral());
            OnDefendingPassives = new FilterPassivesHolder(passives.GetDefending());
        }

        public ICharacterBasicStats GetCurrentStats()
        {
            return _data.stance switch
            {
                TeamCombatData.Stance.Neutral => NeutralStats,
                TeamCombatData.Stance.Attacking => AttackingStats,
                TeamCombatData.Stance.Defending => DefendingStats,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_data.stance}]")
            };
        }

        public FilterPassivesHolder GetCurrentPassives()
        {
            return _data.stance switch
            {
                TeamCombatData.Stance.Neutral => OnNeutralPassives,
                TeamCombatData.Stance.Attacking => OnAttackPassives,
                TeamCombatData.Stance.Defending => OnDefendingPassives,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_data.stance}]")
            };
        }

        public CompositeStats GetAttacking() => AttackingStats;
        public CompositeStats GetNeutral() => NeutralStats;
        public CompositeStats GetDefending() => DefendingStats;
    }
}
