using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamBasicGroupStructure<T> : ITeamRoleStructureRead<T>, ITeamPositionStructureRead<T>
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

    public class FlexPositionMainGroupStructure<T> : TeamBasicGroupStructure<T>, ITeamFlexRoleStructureRead<T>, ITeamFlexPositionStructureRead<T>
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

    public class FlexPositionMainGroupClass<T> : FlexPositionMainGroupStructure<T> where T : new()
    {
        public FlexPositionMainGroupClass()
        {
            FrontLineType = new T();
            MidLineType = new T();
            BackLineType = new T();
            FlexLineType = new T();
        }
    }
}
