using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamBasicGroupStructure<T> : ITeamTrinityStructureRead<T>, ITeamPositionStructureRead<T>
    {
        public TeamBasicGroupStructure() : this(new T[EnumTeam.BasicPositioningAmount])
        { }

        public TeamBasicGroupStructure(T[] members)
        {
            Members = members;
        }

        [ReadOnly]
        public readonly T[] Members;

        public T FrontLineType
        {
            get => Members[EnumTeam.FrontLineIndex];
            protected set => Members[EnumTeam.FrontLineIndex] = value;
        }

        public T MidLineType
        {
            get => Members[EnumTeam.MidLineIndex];
            protected set => Members[EnumTeam.MidLineIndex] = value;
        }

        public T BackLineType
        {
            get => Members[EnumTeam.BackLineIndex];
            protected set => Members[EnumTeam.BackLineIndex] = value;
        }
        public T VanguardType
        {
            get => Members[EnumTeam.VanguardIndex];
            protected set => Members[EnumTeam.VanguardIndex] = value;
        }

        public T AttackerType
        {
            get => Members[EnumTeam.AttackerIndex];
            protected set => Members[EnumTeam.AttackerIndex] = value;
        }

        public T SupportType
        {
            get => Members[EnumTeam.SupportIndex];
            protected set => Members[EnumTeam.SupportIndex] = value;
        }


        public IEnumerator GetEnumerator()
        {
            return Members.GetEnumerator();
        }
    }

    public class TeamBasicGroupHashSet<TKey> : TeamBasicGroupStructure<TKey>
    {
        public TeamBasicGroupHashSet() : base()
        {
            HashSet = new HashSet<TKey>();
        }
        [ShowInInspector]
        protected readonly HashSet<TKey> HashSet;
        public IReadOnlyCollection<TKey> GetHashSet() => HashSet;
    }


    public class FlexPositionMainGroupStructure<T> : TeamBasicGroupStructure<T>, ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T>
    {
        public FlexPositionMainGroupStructure() : base(new T[EnumTeam.PositioningAmount])
        { }

        protected FlexPositionMainGroupStructure(T[] membersHolder) : base(membersHolder)
        {
            
        }

        public T FlexType
        {
            get => Members[EnumTeam.FlexIndex];
            protected set => Members[EnumTeam.FlexIndex] = value;
        }
        public T FlexLineType
        {
            get => Members[EnumTeam.FlexLineIndex];
            protected set => Members[EnumTeam.FlexLineIndex] = value;
        }
    }

    [Serializable]
    public class FlexPositionMainGroupClass<T> : ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T> where T : new()
    {
        [SerializeField] private T frontLineType = new T();
        [SerializeField] private T midLineType = new T();
        [SerializeField] private T backLineType = new T();
        [SerializeField] private T flexLineType = new T();

        public T VanguardType => frontLineType;
        public T AttackerType => midLineType;
        public T SupportType => backLineType;
        public T FlexType => flexLineType;

        public T FrontLineType => frontLineType;
        public T MidLineType => midLineType;
        public T BackLineType => backLineType;
        public T FlexLineType => flexLineType;
    }
}
