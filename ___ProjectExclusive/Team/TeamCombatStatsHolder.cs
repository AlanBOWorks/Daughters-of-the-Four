using System;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Team
{
    public class TeamCombatStatsHolder : ITeamCombatControlStats,
        IStanceArchetype<CompositeStats>
    {
        private readonly TeamCombatData _data;

        public float LoseControlThreshold;

        [ShowInInspector, HorizontalGroup("Attacking")]
        public CompositeStats AttackingStats { get; private set; }
        [ShowInInspector, HorizontalGroup("Neutral")]
        public CompositeStats NeutralStats { get; private set; }
        [ShowInInspector, HorizontalGroup("Defending")]
        public CompositeStats DefendingStats { get; private set; }
        [ShowInInspector, HorizontalGroup("Attacking")]
        public FilterPassivesHolder OnAttackPassives { get; private set; }
        [ShowInInspector, HorizontalGroup("Neutral")]
        public FilterPassivesHolder OnNeutralPassives { get; private set; }
        [ShowInInspector,HorizontalGroup("Defending")]
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

            LoseControlThreshold = DefaultLoseThreshold;

        }

        private const float DefaultLoseThreshold = -.5f;
        public TeamCombatStatsHolder(TeamCombatData data, ITeamCombatControlHolder stats)
        {
            _data = data;
            InjectPreset(stats);
        }


        public void InjectPreset(ITeamCombatControlHolder holder)
        {
            InjectNewStats(holder);
            InjectNewPassives(holder);
            LoseControlThreshold = holder.GetLoseThreshold();
        }

        private void InjectNewStats(IStanceArchetype<ICharacterBasicStats> stats)
        {
            var attackingStats = stats.GetAttacking();
            var neutralStats = stats.GetNeutral();
            var defendingStats = stats.GetDefending();
            if(attackingStats == null)
                throw new NullReferenceException(stats.GetType().ToString());

            AttackingStats = new CompositeStats(attackingStats);
            NeutralStats = new CompositeStats(neutralStats);
            DefendingStats = new CompositeStats(defendingStats);
        }

        private void InjectNewPassives(IStanceArchetype<FilterPassivesHolder> passives)
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
