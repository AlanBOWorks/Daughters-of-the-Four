using System;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Skills;

namespace _Team
{
    public class TeamCombatData
    {
        public TeamCombatData(CombatingTeam team)
        {
            Team = team;
            stance = Stance.Neutral;
        }

        public readonly CombatingTeam Team;

        public float ControlAmount;
        public Stance stance;

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

        public static string GetKeyword(TeamCombatData.Stance target)
        {
            switch (target)
            {
                case TeamCombatData.Stance.Attacking:
                    return AttackKeyword;
                case TeamCombatData.Stance.Neutral:
                    return NeutralKeyword;
                case TeamCombatData.Stance.Defending:
                    return DefendingKeyword;
                default:
                    throw new ArgumentException($"Invalid {typeof(TeamCombatData.Stance)} target;",
                        new NotImplementedException("There's a state that wasn't implemented: " +
                                                    $"{target}"));
            }
        }
    }


    public class CombatingTeam : CharacterArchetypesList<CombatingEntity>, ITeamCombatControlStats
    {
        public CombatingTeam(ITeamCombatControlHolder holder, int amountOfEntities = AmountOfArchetypes)
            : base(amountOfEntities)
        {
            Data = new TeamCombatData(this);
            if(holder != null)
                StatsHolder = new TeamCombatStatsHolder(Data,holder,holder);
            else
                StatsHolder = new TeamCombatStatsHolder(Data);       
        }

        [ShowInInspector]
        public readonly TeamCombatData Data;
        [ShowInInspector]
        public readonly TeamCombatStatsHolder StatsHolder;

        public ICharacterBasicStats GetCurrentStats()
            => StatsHolder.GetCurrentStats();

        public FilterPassivesHolder GetCurrentPassives()
            => StatsHolder.GetCurrentPassives();
    }
}
