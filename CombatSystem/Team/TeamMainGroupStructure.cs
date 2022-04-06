using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamMainGroupStructure<T> : ITeamRoleStructureRead<T>, ITeamPositionStructureRead<T>, IEnumerable
    {
        public TeamMainGroupStructure() : this(new T[EnumTeam.BasicPositioningAmount])
        { }

        public TeamMainGroupStructure(T[] members)
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

    public class FullPositionMainGroupStructure<T> : TeamMainGroupStructure<T>, ITeamFullRoleStructureRead<T>, ITeamFullPositionStructureRead<T>
    {
        public FullPositionMainGroupStructure() : base(new T[EnumTeam.PositioningAmount])
        { }
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

    public class FullPositionMainGroupClass<T> : FullPositionMainGroupStructure<T> where T : new()
    {
        public FullPositionMainGroupClass()
        {
            FrontLineType = new T();
            MidLineType = new T();
            BackLineType = new T();
            FlexLineType = new T();
        }
    }
}
