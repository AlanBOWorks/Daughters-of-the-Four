using System;
using System.Collections.Generic;
using _CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public static class CharacterArchetypes
    {
        public const int FrontLinerIndex = 0;
        public const int MidLinerIndex = FrontLinerIndex + 1;
        public const int BackLinerIndex = MidLinerIndex + 1;
        public const int AmountOfArchetypes = BackLinerIndex + 1;

        public enum TeamPosition
        {
            FrontLine = FrontLinerIndex,
            MidLine = MidLinerIndex,
            BackLine = BackLinerIndex
        }

        public static TeamPosition GetTeamPosition(int indexEquivalent)
        {
            return (TeamPosition) indexEquivalent;
        }


        public enum FieldPosition
        {
            InTeam,
            InEnemyTeam,
            OutFormation
        }
        public static FieldPosition GetPosition(int index)
        {
            return (FieldPosition)index;
        }
        public enum RangeType
        {
            /// <summary>
            /// Can only target closeRange foes
            /// </summary>
            Melee,
            /// <summary>
            /// Can only target ranged foes
            /// </summary>
            Range,
            /// <summary>
            /// Are the combination of <see cref="Melee"/> and <see cref="Range"/>
            /// </summary>
            Hybrid
        }

        /// <summary>
        /// Used for determinate if a <see cref="RangeType.Range"/> is in close position towards an enemy
        /// </summary>
        public static bool IsInCloseRange(FieldPosition user, FieldPosition enemy)
        {
            switch (user)
            {
                case FieldPosition.InTeam:
                    return enemy == FieldPosition.InEnemyTeam;
                case FieldPosition.InEnemyTeam:
                    return enemy == FieldPosition.InTeam;
                case FieldPosition.OutFormation:
                default:
                    throw new NotImplementedException("Position is not implemented for CloseRange");
            }
        }
        /// <summary>
        /// <inheritdoc cref="IsInCloseRange(FieldPosition,FieldPosition)"/>
        /// </summary>
        public static bool IsInCloseRange(CombatingEntity user, CombatingEntity enemy)
        {
            return IsInCloseRange(user.AreasDataTracker.combatFieldPosition, enemy.AreasDataTracker.combatFieldPosition);
        }

        public static bool IsValid<T>(ICharacterArchetypesData<T> data) where T : class
        {
            return data.BackLiner != null && data.MidLiner != null && data.FrontLiner != null;
        }

        public static bool IsValid<T>(T[] elements) => elements.Length == AmountOfArchetypes;
        public static bool IsValid<T>(List<T> elements) => elements.Count == AmountOfArchetypes;

        public delegate TParse InjectionFunc<in T, out TParse>(T original);
        public delegate T ParsingFunc<out T,in TParse>(TParse element);
        public static void DoInjection<T, TParse>(
            ICharacterArchetypesData<T> original,
            ICharacterArchetypesInjection<TParse> injectIn,
            InjectionFunc<T, TParse> parseFunc)
        {
            injectIn.FrontLiner = parseFunc(original.FrontLiner);
            injectIn.MidLiner = parseFunc(original.MidLiner);
            injectIn.BackLiner = parseFunc(original.BackLiner);
        }

        public static CharacterArchetypesList<T> ParseToList<T,TParse>
            (ICharacterArchetypesData<TParse> parsing, ParsingFunc<T, TParse> parsingFunc)
        {
            CharacterArchetypesList<T> generated = new CharacterArchetypesList<T>
            {
                parsingFunc(parsing.FrontLiner), 
                parsingFunc(parsing.MidLiner), 
                parsingFunc(parsing.BackLiner)
            };

            if (generated.Count != AmountOfArchetypes)
                throw new ArgumentException("Generated archetypes are invalid",
                    new IndexOutOfRangeException($"Amount of entities are ({generated.Count}) " +
                                                 $"instead of ({AmountOfArchetypes})"));

            return generated;
        }

        public static List<T> GenerateList<T>(ICharacterArchetypesData<T> fromData)
        {
            List<T> generation = new List<T>()
            {
                fromData.FrontLiner,
                fromData.MidLiner,
                fromData.BackLiner
            };
            return generation;
        }

        public static void AddToList<T>(ICharacterArchetypesData<T> injection, List<T> onList)
        {
            onList.Add(injection.FrontLiner);
            onList.Add(injection.MidLiner);
            onList.Add(injection.BackLiner);
        }


        public static T GetElement<T>(ICharacterArchetypesData<T> elements, int index)
        {
            return GetElement(elements, (TeamPosition) index);
        }
        public static T GetElement<T>(ICharacterArchetypesData<T> elements, TeamPosition archetype)
        {
            switch (archetype)
            {
                case TeamPosition.FrontLine:
                    return elements.FrontLiner;
                case TeamPosition.MidLine:
                    return elements.MidLiner;
                case TeamPosition.BackLine:
                    return elements.BackLiner;
                default:
                    throw new NotImplementedException("Invalid archetype", 
                        new NotImplementedException($"Index of {archetype}"));
            }
        }

    }

    public class CharacterArchetypesList<T> : List<T>, ICharacterArchetypes<T>
    {
        public T FrontLiner
        {
            get => this[FrontLinerIndex];
            set => this[FrontLinerIndex] = value;
        }
        public T MidLiner
        {
            get => this[MidLinerIndex];
            set => this[MidLinerIndex] = value;
        }
        public T BackLiner
        {
            get => this[BackLinerIndex];
            set => this[BackLinerIndex] = value;
        }
        
        public CharacterArchetypesList(int amountOfTypes = AmountOfArchetypes) : base(amountOfTypes)
        {
            
        }

        public CharacterArchetypesList(ICharacterArchetypesData<T> references) : this()
        {
            Add(references.FrontLiner);
            Add(references.MidLiner);
            Add(references.BackLiner);
        }

        public void InjectParse<TParse>(
            ICharacterArchetypesData<TParse> references,
            CharacterArchetypes.ParsingFunc<T, TParse> parsingFunc)
        {
            T frontLiner = parsingFunc(references.FrontLiner);
            T midLiner = parsingFunc(references.MidLiner);
            T backLiner = parsingFunc(references.BackLiner);
            if (Count >= AmountOfArchetypes)
            {
                FrontLiner = frontLiner;
                MidLiner = midLiner;
                BackLiner = backLiner;
            }
            else
            {
                if (Count != 0)
                    throw new ArgumentException("Trying to add elements in an invalid List",
                        new IndexOutOfRangeException("Adding is only possible if Empty during injection"));
                   

                Add(frontLiner);
                Add(midLiner);
                Add(backLiner);
            }
        }



        public const int FrontLinerIndex = CharacterArchetypes.FrontLinerIndex;
        public const int MidLinerIndex = CharacterArchetypes.MidLinerIndex;
        public const int BackLinerIndex = CharacterArchetypes.BackLinerIndex;
        public const int AmountOfArchetypes = CharacterArchetypes.AmountOfArchetypes;
    }

    public class CharacterArchetypes<T> : ICharacterArchetypes<T>
    {
        [ShowInInspector]
        public T FrontLiner { get; set; }
        [ShowInInspector]
        public T MidLiner { get; set; }
        [ShowInInspector]
        public T BackLiner { get; set; }

        public CharacterArchetypes()
        { }
        public CharacterArchetypes(ICharacterArchetypesData<T> data)
        {
            FrontLiner = data.FrontLiner;
            MidLiner = data.MidLiner;
            BackLiner = data.BackLiner;
        }
    }

    public class SerializableCharacterArchetypes<T> : ICharacterArchetypes<T>
    {
        [SerializeField] private T _frontLiner;
        [SerializeField] private T _midLiner;
        [SerializeField] private T _backLiner;


        public T FrontLiner
        {
            get => _frontLiner;
            set => _frontLiner = value;
        }
        public T MidLiner
        {
            get => _midLiner;
            set => _midLiner = value;
        }
        public T BackLiner
        {
            get => _backLiner;
            set => _backLiner = value;
        }

    }

    public interface ICharacterArchetypes<T> : ICharacterArchetypesData<T>, ICharacterArchetypesInjection<T>
    { }

    public interface ICharacterArchetypesData<out T>
    {
        T FrontLiner { get; }
        T MidLiner { get; }
        T BackLiner { get; }
    }

    public interface ICharacterArchetypesInjection<in T>
    {
        T FrontLiner { set; }
        T MidLiner { set; }
        T BackLiner { set; }
    }
}
