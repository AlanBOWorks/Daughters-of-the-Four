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
        private readonly TeamCombatState _state;

        [ShowInInspector]
        public ICharacterArchetypesData<float> ControlLoseOnDeath { get; private set; }

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

        public float LoseControlThreshold;
        public float ReviveTime;

        public TeamCombatStatsHolder(TeamCombatState state)
        {
            _state = state;
            AttackingStats = new CompositeStats();
            NeutralStats = new CompositeStats();
            DefendingStats = new CompositeStats();
            OnAttackPassives = new FilterPassivesHolder();
            OnNeutralPassives = new FilterPassivesHolder();
            OnDefendingPassives = new FilterPassivesHolder();

            LoseControlThreshold = DefaultLoseThreshold;
            ReviveTime = DefaultReviveTime;
            ControlLoseOnDeath = TeamControlLoses.BackUpData;
        }

        public const float DefaultLoseThreshold = -.6f;
        public const float DefaultReviveTime = 2.5f;
        public TeamCombatStatsHolder(TeamCombatState state, ITeamCombatControlHolder stats)
        {
            _state = state;
            InjectPreset(stats);
        }


        public void InjectPreset(ITeamCombatControlHolder holder)
        {
            InjectNewStats(holder);
            InjectNewPassives(holder);
            LoseControlThreshold = holder.GetLoseThreshold();
            ReviveTime = holder.GetReviveTime();
            ControlLoseOnDeath = holder.GetControlLosePoints() ?? TeamControlLoses.BackUpData;
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
            return _state.stance switch
            {
                TeamCombatState.Stance.Neutral => NeutralStats,
                TeamCombatState.Stance.Attacking => AttackingStats,
                TeamCombatState.Stance.Defending => DefendingStats,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_state.stance}]")
            };
        }

        public FilterPassivesHolder GetCurrentPassives()
        {
            return _state.stance switch
            {
                TeamCombatState.Stance.Neutral => OnNeutralPassives,
                TeamCombatState.Stance.Attacking => OnAttackPassives,
                TeamCombatState.Stance.Defending => OnDefendingPassives,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_state.stance}]")
            };
        }

        public CompositeStats GetAttacking() => AttackingStats;
        public CompositeStats GetNeutral() => NeutralStats;
        public CompositeStats GetDefending() => DefendingStats;
    }
}
