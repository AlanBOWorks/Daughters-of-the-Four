using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Team
{
    public class TeamCombatState
    {
        public TeamCombatState(CombatingTeam team)
        {
            Team = team;
            _normalStance = Stances.Neutral;
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

        public Stances CurrentStance => IsForcedStance ? ForceStance : _normalStance;
        [ShowInInspector]
        public Stances ForceStance { get; private set; }

        [ShowInInspector]
        private Stances _normalStance;

        [ShowInInspector] 
        public bool IsForcedStance;

        [ShowInInspector] 
        public bool IsBurstStance;

        public bool IsInDanger()
        {
            return GetControlAmount() <= 
                   Team.StatsHolder.LoseControlThreshold;
        }

        public void VariateStance(Stances target)
        {
            _normalStance = target;
        }

        public void DoForceStance(Stances target)
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



        public enum Stances
        {
            Attacking = 1, //These values are to convert to percentage in Control from (-1,1) values if needed
            Neutral = 0,
            Defending = -1
        }

        public static T GetStance<T>(IStanceArchetype<T> archetype, Stances stance)
        {
            switch (stance)
            {
                case Stances.Attacking:
                    return archetype.GetAttacking();
                case Stances.Neutral:
                    return archetype.GetNeutral();
                case Stances.Defending:
                    return archetype.GetDefending();
                default:
                    throw new ArgumentException("Can't get Stance from the passed arguments",
                        new NotImplementedException($"Target stance is not implemented: {stance}"));
            }
        }

        public static T GetStance<T>(ISkillPositions<T> skills, Stances stance) where T : class 
        {
            switch (stance)
            {
                case Stances.Attacking:
                    return skills.AttackingSkills;
                case Stances.Neutral:
                    return skills.NeutralSkills;
                case Stances.Defending:
                    return skills.DefendingSkills;
                default:
                    throw new ArgumentException("Can't get Stance from the passed arguments",
                        new NotImplementedException($"Target stance is not implemented: {stance}"));
            }
        }
    }


    public interface IStanceArchetype<out T>
    {
        T GetAttacking();
        T GetNeutral();
        T GetDefending();
    }

    public static class UtilsTeam
    {

        public const string AttackKeyword = "Attacking";
        public const string NeutralKeyword = "Neutral";
        public const string DefendingKeyword = "Defending";

        public static string GetKeyword(TeamCombatState.Stances target)
        {
            switch (target)
            {
                case TeamCombatState.Stances.Attacking:
                    return AttackKeyword;
                case TeamCombatState.Stances.Neutral:
                    return NeutralKeyword;
                case TeamCombatState.Stances.Defending:
                    return DefendingKeyword;
                default:
                    throw new ArgumentException($"Invalid {typeof(TeamCombatState.Stances)} target;",
                        new NotImplementedException("There's a state that wasn't implemented: " +
                                                    $"{target}"));
            }
        }
    }


}
