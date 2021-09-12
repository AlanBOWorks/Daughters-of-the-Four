using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
    public static class UtilsCharacterArchetypes
    {
        public const int FrontLinerIndex = EnumTeam.FrontLinerIndex;
        public const int MidLinerIndex = FrontLinerIndex + 1;
        public const int BackLinerIndex = MidLinerIndex + 1;
        public const int AmountOfArchetypesAmount = BackLinerIndex + 1;
        public const int AllPositionIndex = AmountOfArchetypesAmount;
        public const int PositionsTypesAmount = AllPositionIndex + 1;

        public static EnumTeam.GroupPositioning GetTeamPosition(int indexEquivalent)
        {
            return (EnumTeam.GroupPositioning) indexEquivalent;
        }

        public static EnumCharacter.FieldPosition GetPosition(int index)
        {
            return (EnumCharacter.FieldPosition)index;
        }
        

        /// <summary>
        /// Used for determinate if a <see cref="EnumCharacter.RangeType.Range"/> is in close position towards an enemy
        /// </summary>
        public static bool IsInCloseRange(EnumCharacter.FieldPosition user, EnumCharacter.FieldPosition enemy)
        {
            switch (user)
            {
                case EnumCharacter.FieldPosition.InTeam:
                    return enemy == EnumCharacter.FieldPosition.InEnemyTeam;
                case EnumCharacter.FieldPosition.InEnemyTeam:
                    return enemy == EnumCharacter.FieldPosition.InTeam;
                case EnumCharacter.FieldPosition.OutFormation:
                default:
                    throw new NotImplementedException("Position is not implemented for CloseRange");
            }
        }
        /// <summary>
        /// <inheritdoc cref="IsInCloseRange(EnumCharacter.FieldPosition,EnumCharacter.FieldPosition)"/>
        /// </summary>
        public static bool IsInCloseRange(CombatingEntity user, CombatingEntity enemy)
        {
            return IsInCloseRange(user.AreasDataTracker.CombatFieldPosition, enemy.AreasDataTracker.CombatFieldPosition);
        }

        public static bool IsValid<T>(ICharacterArchetypesData<T> data) where T : class
        {
            return data.Support != null && data.Attacker != null && data.Vanguard != null;
        }

        public static bool IsValid<T>(T[] elements) => elements.Length == AmountOfArchetypesAmount;
        public static bool IsValid<T>(List<T> elements) => elements.Count == AmountOfArchetypesAmount;

        public static void DoInjection<T, TParse>(
            ICharacterArchetypesData<T> original,
            ICharacterArchetypesInjection<TParse> injectIn,
            Func<T, TParse> parseFunc)
        {
            injectIn.Vanguard = parseFunc(original.Vanguard);
            injectIn.Attacker = parseFunc(original.Attacker);
            injectIn.Support = parseFunc(original.Support);
        }

        public static void DoAction<T>(
            ICharacterArchetypesData<T> elements,
            Action<T> action)
        {
            action(elements.Vanguard);
            action(elements.Attacker);
            action(elements.Support);
        }
        public static void DoParse<T, TParse>(
            ICharacterArchetypesData<T> elements,
            ICharacterArchetypesData<TParse> parsing,
            Action<T, TParse> action)
        {
            action(elements.Vanguard, parsing.Vanguard);
            action(elements.Attacker, parsing.Attacker);
            action(elements.Support, parsing.Support);
        }

        public static CharacterArchetypesList<T> ParseToList<T,TParse>
            (ICharacterArchetypesData<TParse> parsing, Func<TParse,T> parsingFunc)
        {
            CharacterArchetypesList<T> generated = new CharacterArchetypesList<T>
            {
                parsingFunc(parsing.Vanguard), 
                parsingFunc(parsing.Attacker), 
                parsingFunc(parsing.Support)
            };

            if (generated.Count != AmountOfArchetypesAmount)
                throw new ArgumentException("Generated archetypes are invalid",
                    new IndexOutOfRangeException($"Amount of entities are ({generated.Count}) " +
                                                 $"instead of ({AmountOfArchetypesAmount})"));

            return generated;
        }

        public static List<T> GenerateList<T>(ICharacterArchetypesData<T> fromData)
        {
            List<T> generation = new List<T>()
            {
                fromData.Vanguard,
                fromData.Attacker,
                fromData.Support
            };
            return generation;
        }

        public static void AddToList<T>(ICharacterArchetypesData<T> injection, List<T> onList)
        {
            onList.Add(injection.Vanguard);
            onList.Add(injection.Attacker);
            onList.Add(injection.Support);
        }


        public static T GetElement<T>(ICharacterArchetypesData<T> elements, int index)
        {
            return GetElement(elements, (EnumTeam.GroupPositioning) index);
        }

        public static T GetElement<T>(ICharacterArchetypesData<T> elements, EnumCharacter.RoleArchetype archetype)
        {
            return GetElement(elements, (EnumTeam.GroupPositioning) archetype);
        }


        public static T GetElement<T>(ICharacterArchetypesData<T> elements, EnumTeam.GroupPositioning archetype)
        {
            switch (archetype)
            {
                case EnumTeam.GroupPositioning.FrontLine:
                    return elements.Vanguard;
                case EnumTeam.GroupPositioning.MidLine:
                    return elements.Attacker;
                case EnumTeam.GroupPositioning.BackLine:
                    return elements.Support;
                default:
                    throw new NotImplementedException("Invalid archetype", 
                        new NotImplementedException($"Index of {archetype}"));
            }
        }


        public static void InjectInDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary,
            ICharacterArchetypesData<TKey> keys, ICharacterArchetypesData<TValue> values)
        {
            dictionary.Add(keys.Vanguard, values.Vanguard);
            dictionary.Add(keys.Attacker, values.Attacker);
            dictionary.Add(keys.Support, values.Support);
        }
    }

    public class CharacterArchetypesList<T> : List<T>, ICharacterArchetypes<T>
    {
        public T Vanguard
        {
            get => this[FrontLinerIndex];
            set => this[FrontLinerIndex] = value;
        }
        public T Attacker
        {
            get => this[MidLinerIndex];
            set => this[MidLinerIndex] = value;
        }
        public T Support
        {
            get => this[BackLinerIndex];
            set => this[BackLinerIndex] = value;
        }
        
        public CharacterArchetypesList(int amountOfTypes = AmountOfArchetypes) : base(amountOfTypes)
        {
            
        }

        public void InjectParse<TParse>(
            ICharacterArchetypesData<TParse> references,
            Func<TParse,T> parsingFunc)
        {
            T frontLiner = parsingFunc(references.Vanguard);
            T midLiner = parsingFunc(references.Attacker);
            T backLiner = parsingFunc(references.Support);
            if (Count >= AmountOfArchetypes)
            {
                Vanguard = frontLiner;
                Attacker = midLiner;
                Support = backLiner;
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



        public const int FrontLinerIndex = UtilsCharacterArchetypes.FrontLinerIndex;
        public const int MidLinerIndex = UtilsCharacterArchetypes.MidLinerIndex;
        public const int BackLinerIndex = UtilsCharacterArchetypes.BackLinerIndex;
        public const int AmountOfArchetypes = UtilsCharacterArchetypes.AmountOfArchetypesAmount;
    }



    public class CharacterArchetypes<T> : ICharacterArchetypes<T>
    {
        [ShowInInspector]
        public T Vanguard { get; set; }
        [ShowInInspector]
        public T Attacker { get; set; }
        [ShowInInspector]
        public T Support { get; set; }

        public CharacterArchetypes()
        { }
        public CharacterArchetypes(ICharacterArchetypesData<T> data)
        {
            Vanguard = data.Vanguard;
            Attacker = data.Attacker;
            Support = data.Support;
        }
    }

    public abstract class CharacterArchetypes<T, TInjector> : CharacterArchetypes<T>
    {
        public void DoInjection(ICharacterArchetypesData<TInjector> injector)
        {
            Vanguard = GetElement(injector.Vanguard);
            Attacker = GetElement(injector.Attacker);
            Support = GetElement(injector.Support);
        }

        public abstract T GetElement(TInjector injector);
    }

    [Serializable]
    public class MonoCharacterArchetypes<T> : ICharacterArchetypesData<T> where T : Object
    {
        [SerializeField] private T vanguard;
        [SerializeField] private T attacker;
        [SerializeField] private T support;
        public T Vanguard => vanguard;
        public T Attacker => attacker;
        public T Support => support;
    }

    [Serializable]
    public class SerializableCharacterArchetypes<T> : ICharacterArchetypes<T> where T : new()
    {
        [SerializeField] private T _frontLiner = new T();
        [SerializeField] private T _midLiner = new T();
        [SerializeField] private T _backLiner = new T();


        public T Vanguard
        {
            get => _frontLiner;
            set => _frontLiner = value;
        }
        public T Attacker
        {
            get => _midLiner;
            set => _midLiner = value;
        }
        public T Support
        {
            get => _backLiner;
            set => _backLiner = value;
        }
    }


    public abstract class CharacterArchetypesConstructor<T> : ICharacterArchetypesData<T>
    {
        public T Vanguard => GenerateObject();
        public T Attacker => GenerateObject();
        public T Support => GenerateObject();

        public ICharacterArchetypes<T> GenerateHolder()
        {
            CharacterArchetypes<T> generated = new CharacterArchetypes<T>
            {
                Vanguard = GenerateObject(), Attacker = GenerateObject(), Support = GenerateObject()
            };
            return generated;
        }

        protected abstract T GenerateObject();
    }

    public interface ICharacterArchetypes<T> : ICharacterArchetypesData<T>, ICharacterArchetypesInjection<T>
    {
        new T Vanguard { get; set; }
        new T Attacker { get; set; }
        new T Support { get; set; }
    }

    public interface ICharacterArchetypesData<out T>
    {
        T Vanguard { get; }
        T Attacker { get; }
        T Support { get; }
    }

    public interface ICharacterArchetypesInjection<in T>
    {
        T Vanguard { set; }
        T Attacker { set; }
        T Support { set; }
    }
}
