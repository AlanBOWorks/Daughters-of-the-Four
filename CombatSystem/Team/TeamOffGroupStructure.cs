using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamOffGroupStructure<T> : IEnumerator<T>, 
        ITeamFullRoleStructureRead<T[]>, ITeamFullPositionStructureRead<T[]>
    {
        public TeamOffGroupStructure()
        {
            _frontLineType = new T[EnumTeam.OffRoleTypesLength];
            _midLineType = new T[EnumTeam.OffRoleTypesLength];
            _backLineType = new T[EnumTeam.OffRoleTypesLength];
            _flexLineType = new T[EnumTeam.OffRoleTypesLength];

            _membersIndex = -1;
        }

        private int _membersIndex;
        [ShowInInspector,HorizontalGroup("Front")]
        private readonly T[] _frontLineType;
        [ShowInInspector,HorizontalGroup("Front")]
        private readonly T[] _midLineType;
        [ShowInInspector,HorizontalGroup("Back")]
        private readonly T[] _backLineType;
        [ShowInInspector,HorizontalGroup("Back")]
        private readonly T[] _flexLineType;


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

        public T GetElement(in EnumTeam.ActiveRole role) => UtilsTeam.GetElement(role, this);


        public bool MoveNext()
        {
            _membersIndex++;
            return _membersIndex < EnumTeam.OffRoleIndexCount;
        }

        public void Reset()
        {
            _membersIndex = -1;
        }

        public T Current => UtilsTeam.GetElement(_membersIndex, this);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public T[] VanguardType => _frontLineType;
        public T[] AttackerType => _midLineType;
        public T[] SupportType => _backLineType;
        public T[] FlexType => _flexLineType;

        public T[] FrontLineType => _frontLineType;
        public T[] MidLineType => _midLineType;
        public T[] BackLineType => _backLineType;
        public T[] FlexLineType => _flexLineType;


        public T SecondaryVanguardElement
        {
            get => _frontLineType[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => _frontLineType[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondaryAttackerElement
        {
            get => _midLineType[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => _midLineType[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondarySupportElement
        {
            get => _backLineType[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => _backLineType[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T SecondaryFlexElement
        {
            get => _flexLineType[EnumTeam.SecondaryRoleInOffArrayIndex];
            set => _flexLineType[EnumTeam.SecondaryRoleInOffArrayIndex] = value;
        }

        public T ThirdVanguardElement
        {
            get => _frontLineType[EnumTeam.ThirdRoleInOffArrayIndex];
            set => _frontLineType[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdAttackerElement
        {
            get => _midLineType[EnumTeam.ThirdRoleInOffArrayIndex];
            set => _midLineType[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdSupportElement
        {
            get => _backLineType[EnumTeam.ThirdRoleInOffArrayIndex];
            set => _backLineType[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }

        public T ThirdFlexElement
        {
            get => _flexLineType[EnumTeam.ThirdRoleInOffArrayIndex];
            set => _flexLineType[EnumTeam.ThirdRoleInOffArrayIndex] = value;
        }
    }
}
