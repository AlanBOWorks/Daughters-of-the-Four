using System;
using UnityEngine;

namespace _Team
{

    public static class EnumTeam
    {
        public enum Stances
        {
            Attacking = 1, //These values are to convert to percentage in Control from (-1,1) values if needed
            Neutral = 0,
            Defending = -1
        }

        public const int FrontLinerIndex = 0;
        public const int MidLinerIndex = FrontLinerIndex + 1;
        public const int BackLinerIndex = MidLinerIndex + 1;
        public const int AmountOfArchetypesAmount = BackLinerIndex + 1;
        public const int AllPositionIndex = AmountOfArchetypesAmount;
        public const int PositionsTypesAmount = AllPositionIndex + 1;

        public enum GroupPositioning
        {
            FrontLine = FrontLinerIndex,
            MidLine = MidLinerIndex,
            BackLine = BackLinerIndex
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
