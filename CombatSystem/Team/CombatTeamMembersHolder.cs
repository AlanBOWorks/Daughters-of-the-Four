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
        ITeamFlexPositionStructureRead<IReadOnlyList<CombatEntity>>,
        ITeamFlexStructureRead<IReadOnlyList<CombatEntity>>,
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
      


        public IReadOnlyList<CombatEntity> AllMembers => _allMembers;
        public bool Contains(in CombatEntity entity) => _allMembers.Contains(entity);


        public IReadOnlyList<CombatEntity> FrontLineType => _positionWrapper.FrontLineType;
        public IReadOnlyList<CombatEntity> MidLineType => _positionWrapper.MidLineType;
        public IReadOnlyList<CombatEntity> BackLineType => _positionWrapper.BackLineType;
        public IReadOnlyList<CombatEntity> FlexLineType => _positionWrapper.FlexLineType;


        public IReadOnlyList<CombatEntity> VanguardType => _roleWrapper.VanguardType;
        public IReadOnlyList<CombatEntity> AttackerType => _roleWrapper.AttackerType;
        public IReadOnlyList<CombatEntity> SupportType => _roleWrapper.SupportType;
        public IReadOnlyList<CombatEntity> FlexType => _roleWrapper.FlexType;

        public ITeamFlexPositionStructureRead<CombatEntity> GetMainPositions() => _positionWrapper;
        public IReadOnlyList<CombatEntity> GetMainRoles() => _roleWrapper.MainRoles;
        public IReadOnlyList<CombatEntity> GetOffRoles() => _roleWrapper.OffRoles;
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

        public bool IsMainRole(CombatEntity entity) => _roleWrapper.IsMainRole(entity);
        public bool IsTrinityRole(CombatEntity entity) => _roleWrapper.IsTrinityRole(entity);
        public bool IsSecondaryTypeRole(CombatEntity entity) => _roleWrapper.IsSecondaryTypeMember(entity);
        public bool IsThirdTypeRole(CombatEntity entity) => _roleWrapper.IsThirdTypeMember(entity);

        public void AddMember(CombatEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }

            var positioning = entity.PositioningType;
            var roleType = entity.RoleType;

            _allMembers.Add(entity);
            _positionWrapper.AddMember(positioning,entity);
            _roleWrapper.AddMember(roleType, entity);
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

            public void AddMember(EnumTeam.Positioning positioning, CombatEntity member)
            {
                ITeamFlexPositionStructureRead<List<CombatEntity>> structure = this;
                var line = UtilsTeam.GetElement(positioning, structure);
                line.Add(member);
            }

            private static CombatEntity GetLineFirstMember(in List<CombatEntity> line) => (line.Count == 0) ? null : line[0];
        }
        private sealed class RoleWrapper : 
            ITeamFlexStructureRead<List<CombatEntity>>, 
            ITeamAlimentStructureRead<IEnumerable<CombatEntity>>
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

            public IReadOnlyList<CombatEntity> MainRoles => _mainRoles;
            public IReadOnlyList<CombatEntity> OffRoles => _offRoles;
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

            public void AddMember(EnumTeam.Role role, CombatEntity member)
            {
                var roleGroup = UtilsTeam.GetElement(role, this);
                int roleGroupCount = roleGroup.Count;
                bool isMainRole = roleGroupCount == 0;
                roleGroup.Add(member);

                if(!isMainRole)
                {
                    bool isSecondRole = roleGroupCount == 1;
                    IList<CombatEntity> offRoleList;
                    EnumTeam.RolePriorityType priorityType;
                    if (isSecondRole)
                    {
                        offRoleList = _offRoles.GetFirstList();
                        priorityType = EnumTeam.RolePriorityType.SecondaryRole;
                    }
                    else
                    {
                        offRoleList = _offRoles.GetSecondList();
                        priorityType = EnumTeam.RolePriorityType.ThirdRole;
                    }


                    offRoleList.Add(member);
                    member.Injection(priorityType);
                    return;
                }

                if(role == EnumTeam.Role.Vanguard)
                {
                    _mainRoles.Insert(0,member); //Main Vanguard should always be first
                    return;
                }
                _mainRoles.Add(member);
                member.Injection(EnumTeam.RolePriorityType.MainRole);
            }


            public bool IsMainRole(CombatEntity entity) => _mainRoles.Contains(entity);
            public bool IsTrinityRole(CombatEntity entity)
            {
                var role = entity.RoleType;
                if (role == EnumTeam.Role.Flex || role == EnumTeam.Role.InvalidRole) return false;
                return _mainRoles.Contains(entity);
            }

            public bool IsSecondaryTypeMember(CombatEntity entity) => _offRoles.GetFirstList().Contains(entity);
            public bool IsThirdTypeMember(CombatEntity entity) => _offRoles.GetSecondList().Contains(entity);


            private sealed class OffRolesCollection : ConcatDualList<CombatEntity>
            {
                public OffRolesCollection() : base(TeamLinesMaxCapacity)
                { }
            }
        }
    }
}
