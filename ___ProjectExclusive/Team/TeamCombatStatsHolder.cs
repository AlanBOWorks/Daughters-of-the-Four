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
        public int BurstControlLength;
        public int BurstCounterAmount;

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

            BurstControlLength = holder.GetBurstControlLength();
            BurstCounterAmount = holder.GetBurstCounterAmount();
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

        public ICharacterBasicStatsData GetCurrentStats()
        {
            return _state.CurrentStance switch
            {
                TeamCombatState.Stances.Neutral => NeutralStats,
                TeamCombatState.Stances.Attacking => AttackingStats,
                TeamCombatState.Stances.Defending => DefendingStats,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_state.CurrentStance}]")
            };
        }

        public FilterPassivesHolder GetCurrentPassives()
        {
            return _state.CurrentStance switch
            {
                TeamCombatState.Stances.Neutral => OnNeutralPassives,
                TeamCombatState.Stances.Attacking => OnAttackPassives,
                TeamCombatState.Stances.Defending => OnDefendingPassives,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_state.CurrentStance}]")
            };
        }

        public CompositeStats GetAttacking() => AttackingStats;
        public CompositeStats GetNeutral() => NeutralStats;
        public CompositeStats GetDefending() => DefendingStats;

        public void InjectAura(SAuraPassive aura)
        {
            var position = aura.InjectionPosition();
            var stats = aura.GetStats();
            switch (position)
            {
                case CharacterArchetypes.TeamPosition.FrontLine:
                    AttackingStats.Add(stats);
                    break;
                case CharacterArchetypes.TeamPosition.MidLine:
                    NeutralStats.Add(stats);
                    break;
                case CharacterArchetypes.TeamPosition.BackLine:
                    DefendingStats.Add(stats);
                    break;
                case CharacterArchetypes.TeamPosition.All:
                    AttackingStats.Add(stats);
                    NeutralStats.Add(stats);
                    DefendingStats.Add(stats);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Target position [{position}] is not " +
                                                          $"supported in Aura Injection.");
            }

        }
    }
}
