using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Team
{
    public class TeamCombatState : IStanceProvider
    {
        public TeamCombatState(CombatingTeam team)
        {
            Team = team;
            _normalStance = EnumTeam.Stances.Neutral;
        }

        public readonly CombatingTeam Team;

        [field: ShowInInspector]
        [field: Range(-1,1)]
        public float TeamControlAmount = 0;


        [ShowInInspector] 
        public float BurstControlAmount { get; private set; }


        public float GetControlAmount()
        {
            float control = TeamControlAmount + BurstControlAmount;

            return control;
        }

        public EnumTeam.Stances CurrentStance => IsForcedStance ? ForceStance : _normalStance;
        [ShowInInspector]
        public EnumTeam.Stances ForceStance { get; private set; }

        [ShowInInspector]
        private EnumTeam.Stances _normalStance;

        [ShowInInspector] 
        public bool IsForcedStance;

        [ShowInInspector] 
        public bool IsBurstStance;

        public bool IsInDanger()
        {
            return GetControlAmount() <= 
                   Team.StatsHolder.LoseControlThreshold;
        }

        public void VariateStance(EnumTeam.Stances target)
        {
            _normalStance = target;
        }

        public void DoForceStance(EnumTeam.Stances target)
        {
            IsForcedStance = true;
            ForceStance = target;
        }

        public void FinishForceStance()
        {
            IsForcedStance = false;
        }

        public void DoBurstControl(float targetBurst)
        {
            BurstControlAmount = targetBurst;
            IsBurstStance = true;
        }

        public void DoBurstVariation(float variation)
        {
            BurstControlAmount += variation;
        }

        public void FinishBurstControl()
        {
            BurstControlAmount = 0;
            IsBurstStance = false;
        }
    }

    public static class EnumTeam
    {
        public enum Stances
        {
            Attacking = 1, //These values are to convert to percentage in Control from (-1,1) values if needed
            Neutral = 0,
            Defending = -1
        }
    }

    public static class UtilsTeam
    {

        public const string AttackKeyword = "Attacking";
        public const string NeutralKeyword = "Neutral";
        public const string DefendingKeyword = "Defending";

        public static string GetKeyword(EnumTeam.Stances target)
        {
            switch (target)
            {
                case EnumTeam.Stances.Attacking:
                    return AttackKeyword;
                case EnumTeam.Stances.Neutral:
                    return NeutralKeyword;
                case EnumTeam.Stances.Defending:
                    return DefendingKeyword;
                default:
                    throw new ArgumentException($"Invalid {typeof(EnumTeam.Stances)} target;",
                        new NotImplementedException("There's a state that wasn't implemented: " +
                                                    $"{target}"));
            }
        }

        public static T GetElement<T>(IStanceData<T> stances, EnumTeam.Stances target)
        {
            switch (target)
            {
                case EnumTeam.Stances.Attacking:
                    return stances.AttackingStance;
                case EnumTeam.Stances.Neutral:
                    return stances.NeutralStance;
                case EnumTeam.Stances.Defending:
                    return stances.DefendingStance;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }
    }
}
