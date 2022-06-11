using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Collections;

namespace CombatSystem.Team
{
    public sealed class CombatTeamMembersHolder : 
        ITeamFlexPositionStructureRead<IReadOnlyCollection<CombatEntity>>,
        ITeamFlexStructureRead<IReadOnlyCollection<CombatEntity>>,
        ITeamFullStructureRead<CombatEntity>
    {
        public CombatTeamMembersHolder()
        {
            _allMembers = new List<CombatEntity>();
            _positionWrapper = new PositionWrapper();
            _roleWrapper = new RoleWrapper();
        }

        public const int LineMembersMaxCapacity = 3;
        public const int TeamLinesMaxCapacity = 4;
        public const int TeamMembersMaxCapacity = TeamLinesMaxCapacity * LineMembersMaxCapacity;

        private readonly List<CombatEntity> _allMembers;
        [ShowInInspector]
        private readonly PositionWrapper _positionWrapper;
        [ShowInInspector]
        private readonly RoleWrapper _roleWrapper;
      


        public IReadOnlyCollection<CombatEntity> AllMembers => _allMembers;
        public bool Contains(in CombatEntity entity) => _allMembers.Contains(entity);


        public IReadOnlyCollection<CombatEntity> FrontLineType => _positionWrapper.FrontLineType;
        public IReadOnlyCollection<CombatEntity> MidLineType => _positionWrapper.MidLineType;
        public IReadOnlyCollection<CombatEntity> BackLineType => _positionWrapper.BackLineType;
        public IReadOnlyCollection<CombatEntity> FlexLineType => _positionWrapper.FlexLineType;


        public IReadOnlyCollection<CombatEntity> VanguardType => _roleWrapper.VanguardType;


        public IReadOnlyCollection<CombatEntity> AttackerType => _roleWrapper.AttackerType;
        public IReadOnlyCollection<CombatEntity> SupportType => _roleWrapper.SupportType;
        public IReadOnlyCollection<CombatEntity> FlexType => _roleWrapper.FlexType;

        public ITeamFlexPositionStructureRead<CombatEntity> GetMainPositions() => _positionWrapper;
        public IReadOnlyCollection<CombatEntity> GetMainRoles() => _roleWrapper.MainRoles;
        public IReadOnlyCollection<CombatEntity> GetOffRoles() => _roleWrapper.OffRoles;
        public IEnumerable<CombatEntity> GetSecondaryRoles() => _roleWrapper.SecondaryRoles;
        public IEnumerable<CombatEntity> GetThirdRoles() => _roleWrapper.ThirdRoles;

        public ITeamAlimentStructureRead<IEnumerable<CombatEntity>> GetAlimentRoles() => _roleWrapper;


        CombatEntity ITeamTrinityStructureRead<CombatEntity>.VanguardType 
            => GetMember(_roleWrapper.VanguardType, EnumTeam.MainRoleIndex);
        CombatEntity ITeamTrinityStructureRead<CombatEntity>.AttackerType
            => GetMember(_roleWrapper.AttackerType, EnumTeam.MainRoleIndex);
        CombatEntity ITeamTrinityStructureRead<CombatEntity>.SupportType
            => GetMember(_roleWrapper.SupportType, EnumTeam.MainRoleIndex);
        CombatEntity ITeamFlexStructureRead<CombatEntity>.FlexType
            => GetMember(_roleWrapper.FlexType, EnumTeam.MainRoleIndex);

        public CombatEntity SecondaryVanguardElement
            => GetMember(_roleWrapper.VanguardType, EnumTeam.SecondaryRoleIndex);
        public CombatEntity SecondaryAttackerElement 
            => GetMember(_roleWrapper.AttackerType, EnumTeam.SecondaryRoleIndex);
        public CombatEntity SecondarySupportElement 
            => GetMember(_roleWrapper.SupportType, EnumTeam.SecondaryRoleIndex);
        public CombatEntity SecondaryFlexElement 
            => GetMember(_roleWrapper.FlexType, EnumTeam.SecondaryRoleIndex);

        public CombatEntity ThirdVanguardElement
            => GetMember(_roleWrapper.VanguardType, EnumTeam.ThirdRoleIndex);
        public CombatEntity ThirdAttackerElement
            => GetMember(_roleWrapper.AttackerType, EnumTeam.ThirdRoleIndex);
        public CombatEntity ThirdSupportElement
            => GetMember(_roleWrapper.SupportType, EnumTeam.ThirdRoleIndex);
        public CombatEntity ThirdFlexElement
            => GetMember(_roleWrapper.FlexType, EnumTeam.ThirdRoleIndex);


        private static CombatEntity GetMember(IReadOnlyList<CombatEntity> group, int index)
        {
            return group.Count > index ? group[index] : null;
        }

        public bool IsMainRole(in CombatEntity entity) => _roleWrapper.IsMainRole(in entity);
        public bool IsTrinityRole(in CombatEntity entity) => _roleWrapper.IsTrinityRole(in entity);

        public void AddMember([NotNull] in CombatEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }

            var positioning = entity.PositioningType;
            var roleType = entity.RoleType;

            _allMembers.Add(entity);
            _positionWrapper.AddMember(in positioning,in entity);
            _roleWrapper.AddMember(in roleType, in entity);
        }

        private sealed class PositionWrapper : ITeamFlexPositionStructureRead<List<CombatEntity>>,
            ITeamFlexPositionStructureRead<CombatEntity>
        {
            [ShowInInspector, HorizontalGroup("FrontLine")]
            private readonly List<CombatEntity> _frontLine;
            [ShowInInspector, HorizontalGroup("FrontLine")]
            private readonly List<CombatEntity> _midLine;
            [ShowInInspector, HorizontalGroup("BackLine")]
            private readonly List<CombatEntity> _backLine;
            [ShowInInspector, HorizontalGroup("BackLine")]
            private readonly List<CombatEntity> _flexLine;

            public PositionWrapper()
            {
                _frontLine = new List<CombatEntity>(LineMembersMaxCapacity);
                _midLine = new List<CombatEntity>(LineMembersMaxCapacity);
                _backLine = new List<CombatEntity>(LineMembersMaxCapacity);
                _flexLine = new List<CombatEntity>(LineMembersMaxCapacity);
            }


            public List<CombatEntity> FrontLineType => _frontLine;
            public List<CombatEntity> MidLineType => _midLine;
            public List<CombatEntity> BackLineType => _backLine;
            public List<CombatEntity> FlexLineType => _flexLine;

            CombatEntity ITeamPositionStructureRead<CombatEntity>.FrontLineType => GetLineFirstMember(in _frontLine);
            CombatEntity ITeamPositionStructureRead<CombatEntity>.MidLineType => GetLineFirstMember(in _midLine);
            CombatEntity ITeamPositionStructureRead<CombatEntity>.BackLineType => GetLineFirstMember(in _backLine);
            CombatEntity ITeamFlexPositionStructureRead<CombatEntity>.FlexLineType => GetLineFirstMember(in _flexLine);

            public void AddMember(in EnumTeam.Positioning positioning, in CombatEntity member)
            {
                ITeamFlexPositionStructureRead<List<CombatEntity>> structure = this;
                var line = UtilsTeam.GetElement(positioning, structure);
                line.Add(member);
            }

            private static CombatEntity GetLineFirstMember(in List<CombatEntity> line) => (line.Count == 0) ? null : line[0];
        }
        private sealed class RoleWrapper : ITeamFlexStructureRead<List<CombatEntity>>, ITeamAlimentStructureRead<IEnumerable<CombatEntity>>
        {
            public RoleWrapper()
            {
                VanguardType = new List<CombatEntity>(LineMembersMaxCapacity);
                AttackerType = new List<CombatEntity>(LineMembersMaxCapacity);
                SupportType = new List<CombatEntity>(LineMembersMaxCapacity);
                FlexType = new List<CombatEntity>(LineMembersMaxCapacity);

                _mainRoles = new List<CombatEntity>(TeamLinesMaxCapacity);
                _offRoles = new OffRolesCollection();
            }


            private readonly List<CombatEntity> _mainRoles;
            [ShowInInspector]
            private readonly OffRolesCollection _offRoles;

            public IReadOnlyCollection<CombatEntity> MainRoles => _mainRoles;
            public IReadOnlyCollection<CombatEntity> OffRoles => _offRoles;
            public IEnumerable<CombatEntity> SecondaryRoles => _offRoles.GetFirstEnumerable();
            public IEnumerable<CombatEntity> ThirdRoles => _offRoles.GetSecondEnumerable();

            [ShowInInspector, HorizontalGroup("FrontLine")]
            public List<CombatEntity> VanguardType { get; }
            [ShowInInspector, HorizontalGroup("FrontLine")]
            public List<CombatEntity> AttackerType { get; }
            [ShowInInspector, HorizontalGroup("BackLine")]
            public List<CombatEntity> SupportType { get; }
            [ShowInInspector, HorizontalGroup("BackLine")]
            public List<CombatEntity> FlexType { get; }


            public IEnumerable<CombatEntity> MainRole => _mainRoles;
            public IEnumerable<CombatEntity> SecondaryRole => _offRoles.GetFirstEnumerable();
            public IEnumerable<CombatEntity> ThirdRole => _offRoles.GetSecondEnumerable();

            public void AddMember(in EnumTeam.Role role, in CombatEntity member)
            {
                var roleGroup = UtilsTeam.GetElement(role, this);
                int roleGroupCount = roleGroup.Count;
                bool isMainRole = roleGroupCount == 0;
                roleGroup.Add(member);

                if(!isMainRole)
                {
                    bool isSecondRole = roleGroupCount == 1;
                    var offRoleList = isSecondRole 
                        ? _offRoles.GetFirstList() 
                        : _offRoles.GetSecondList();

                    offRoleList.Add(member);
                    return;
                }

                if(role == EnumTeam.Role.Vanguard)
                {
                    _mainRoles.Insert(0,member); //Main Vanguard should always be first
                    return;
                }
                _mainRoles.Add(member);
            }


            public bool IsMainRole(in CombatEntity entity) => _mainRoles.Contains(entity);
            public bool IsTrinityRole(in CombatEntity entity)
            {
                var role = entity.RoleType;
                if (role == EnumTeam.Role.Flex || role == EnumTeam.Role.InvalidRole) return false;
                return _mainRoles.Contains(entity);
            }

            private sealed class OffRolesCollection : ConcatDualList<CombatEntity>
            {
                public OffRolesCollection() : base(TeamLinesMaxCapacity)
                { }
            }
        }
    }
}
