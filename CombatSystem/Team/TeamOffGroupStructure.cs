using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamOffGroupStructure<T> : IEnumerator<T>, 
        ITeamFlexRoleStructureRead<T[]>, ITeamFlexPositionStructureRead<T[]>, ITeamOffStructureRead<T>
    {
        public TeamOffGroupStructure() : this(EnumTeam.OffRoleTypesLength)
        {
            
        }

        protected TeamOffGroupStructure(int arrayLength)
        {
            VanguardFrontLine = new T[arrayLength];
            AttackerMidLine = new T[arrayLength];
            SupportBackLine = new T[arrayLength];
            FlexFlexLine = new T[arrayLength];

            MembersIndex = -1;
        }


        protected int MembersIndex;
        [ShowInInspector,HorizontalGroup("Front")]
        protected readonly T[] VanguardFrontLine;
        [ShowInInspector,HorizontalGroup("Front")]
        protected readonly T[] AttackerMidLine;
        [ShowInInspector,HorizontalGroup("Back")]
        protected readonly T[] SupportBackLine;
        [ShowInInspector,HorizontalGroup("Back")]
        protected readonly T[] FlexFlexLine;


        public void AddMember(EnumTeam.Positioning positioning, in T element)
        {
            var group = UtilsTeam.GetElement(positioning, this);
            AddMember(in group, in element);
        }

        public void AddMember(EnumTeam.Role role, in T element)
        {
            var group = UtilsTeam.GetElement(role, this);
            AddMember(in group, in element);
        }

        private static void AddMember(in T[] group, in T element)
        {
            if (group[0] == null) group[0] = element;
            else if (group[1] == null) group[1] = element;
            else
            {
                throw new IndexOutOfRangeException("Out of positioning/role indexes");
            }
        }


        private const int IndexThreshold = EnumTeam.OffRoleIndexCount;
        protected virtual int MoveNextThreshold() => IndexThreshold;

        public bool MoveNext()
        {
            MembersIndex++;
            return MembersIndex < MoveNextThreshold();
        }

        public void Reset()
        {
            MembersIndex = -1;
        }

        public virtual T Current => UtilsTeam.GetElement(MembersIndex, this);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public T[] VanguardType => VanguardFrontLine;
        public T[] AttackerType => AttackerMidLine;
        public T[] SupportType => SupportBackLine;
        public T[] FlexType => FlexFlexLine;

        public T[] FrontLineType => VanguardFrontLine;
        public T[] MidLineType => AttackerMidLine;
        public T[] BackLineType => SupportBackLine;
        public T[] FlexLineType => FlexFlexLine;


        public T SecondaryVanguardElement
        {
            get => VanguardFrontLine[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => VanguardFrontLine[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondaryAttackerElement
        {
            get => AttackerMidLine[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => AttackerMidLine[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondarySupportElement
        {
            get => SupportBackLine[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => SupportBackLine[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondaryFlexElement
        {
            get => FlexFlexLine[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => FlexFlexLine[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T ThirdVanguardElement
        {
            get => VanguardFrontLine[EnumTeam.ThirdRoleInOffArrayIndex];
            set => VanguardFrontLine[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdAttackerElement
        {
            get => AttackerMidLine[EnumTeam.ThirdRoleInOffArrayIndex];
            set => AttackerMidLine[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdSupportElement
        {
            get => SupportBackLine[EnumTeam.ThirdRoleInOffArrayIndex];
            set => SupportBackLine[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdFlexElement
        {
            get => FlexFlexLine[EnumTeam.ThirdRoleInOffArrayIndex];
            set => FlexFlexLine[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }
    }

    public interface ITeamOffStructureRead<out T>
    {
        T SecondaryVanguardElement { get; }
        T SecondaryAttackerElement { get; }
        T SecondarySupportElement { get; }
        T SecondaryFlexElement { get; }

        T ThirdVanguardElement { get; }
        T ThirdAttackerElement { get; }
        T ThirdSupportElement { get; }
        T ThirdFlexElement { get; }
    }
}
