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
            stance = Stance.Neutral;
        }

        public readonly CombatingTeam Team;

        [ShowInInspector,Range(-1,1)] 
        public float TeamControlAmount = 0;
        [ShowInInspector,Range(-1,1)] 
        public float BurstControlAmount = 0;

        public float ControlAmount => TeamControlAmount + BurstControlAmount;

        [ShowInInspector]
        public Stance stance;

        public bool IsInDanger()
        {
            return ControlAmount <= Team.StatsHolder.LoseControlThreshold;
        }

        public void DoBurstControl(float targetBurst)
        {
            BurstControlAmount = targetBurst;
        }

        public void FinishBurstControl()
        {
            BurstControlAmount = 0;
        }

        public bool IsInBurstControl()
        {
            return BurstControlAmount > 0;
        }

        public enum Stance
        {
            Attacking = 1, //These values are to convert to percentage in Control from (-1,1) values if needed
            Neutral = 0,
            Defending = -1
        }

        public static T GetStance<T>(IStanceArchetype<T> archetype, Stance stance)
        {
            switch (stance)
            {
                case Stance.Attacking:
                    return archetype.GetAttacking();
                case Stance.Neutral:
                    return archetype.GetNeutral();
                case Stance.Defending:
                    return archetype.GetDefending();
                default:
                    throw new ArgumentException("Can't get Stance from the passed arguments",
                        new NotImplementedException($"Target stance is not implemented: {stance}"));
            }
        }

        public static T GetStance<T>(ISkillPositions<T> skills, Stance stance) where T : class 
        {
            switch (stance)
            {
                case Stance.Attacking:
                    return skills.AttackingSkills;
                case Stance.Neutral:
                    return skills.NeutralSkills;
                case Stance.Defending:
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

        public static string GetKeyword(TeamCombatState.Stance target)
        {
            switch (target)
            {
                case TeamCombatState.Stance.Attacking:
                    return AttackKeyword;
                case TeamCombatState.Stance.Neutral:
                    return NeutralKeyword;
                case TeamCombatState.Stance.Defending:
                    return DefendingKeyword;
                default:
                    throw new ArgumentException($"Invalid {typeof(TeamCombatState.Stance)} target;",
                        new NotImplementedException("There's a state that wasn't implemented: " +
                                                    $"{target}"));
            }
        }
    }


}
