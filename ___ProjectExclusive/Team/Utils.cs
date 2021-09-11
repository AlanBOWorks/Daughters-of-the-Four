using System;
using System.Collections.Generic;
using Characters;
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
        public static T GetElement<T>(ICharacterArchetypesData<T> elements, EnumTeam.GroupPositioning positioning)
        {
            return positioning switch
            {
                EnumTeam.GroupPositioning.FrontLine => elements.Vanguard,
                EnumTeam.GroupPositioning.MidLine => elements.Attacker,
                EnumTeam.GroupPositioning.BackLine => elements.Support,
                _ => throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null)
            };
        }

        public static void InjectInDictionary<TKey,TValue>(Dictionary<TKey, TValue> dictionary,
            ITeamsData<ICharacterArchetypesData<TKey>> keys, ITeamsData<ICharacterArchetypesData<TValue>> values)
        {
            UtilsCharacterArchetypes.InjectInDictionary(dictionary, keys.PlayerData, values.PlayerData);
            UtilsCharacterArchetypes.InjectInDictionary(dictionary, keys.EnemyData, values.EnemyData);
        }

        public static void DoInjection<T, TInjection>(ITeamsData<ICharacterArchetypesData<T>> elements,
            ITeamsData<ICharacterArchetypesData<TInjection>> injections, Action<T, TInjection> injectionAction)
        {
            UtilsCharacter.DoAction(elements.PlayerData,injections.PlayerData,injectionAction);
            UtilsCharacter.DoAction(elements.EnemyData,injections.EnemyData,injectionAction);
        }

        /// <summary>
        /// Generates object of [<see cref="T"/>] and injects into the [<see cref="elements"/>]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DoGenerationInjection<T>(ITeamDataFull<T> elements)
        {
            var playerData = elements.PlayerData;
            var enemyData = elements.EnemyData;

            DoInjection(playerData);
            DoInjection(enemyData);
            void DoInjection(ICharacterArchetypes<T> holder)
            {
                holder.Vanguard = elements.GenerateElement();
                holder.Attacker = elements.GenerateElement();
                holder.Support = elements.GenerateElement();
            }
        }
    }
}
