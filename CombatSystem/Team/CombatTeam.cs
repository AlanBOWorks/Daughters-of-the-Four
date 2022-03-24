using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeam : 
        ITeamPositionStructureRead<IReadOnlyList<CombatEntity>>,
        ITeamRoleStructureRead<CombatEntity>,
        IReadOnlyList<CombatEntity>
    {
        private CombatTeam(bool isPlayerTeam)
        {
            IsPlayerTeam = isPlayerTeam;

            DataValues = new TeamDataValues();

            _members = new List<CombatEntity>();

            _positionWrapper = new PositionWrapper();
            _roleWrapper = new RoleWrapper();
        }
        
        public CombatTeam(bool isPlayerTeam,IReadOnlyCollection<ICombatEntityProvider> members) : this(isPlayerTeam)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));


            foreach (var provider in members)
            {
                Add(provider);
            }
        }

        [Title("Team Data")]
        public readonly bool IsPlayerTeam;


        // ------------ DATA ------------ 
        public readonly TeamDataValues DataValues;

#if UNITY_EDITOR
        [Title("Members")]
        private enum MenuOptions
        {
            Members,
            Positions,
            Roles
        }

        [ShowInInspector, GUIColor(.3f, .8f, .8f), BoxGroup("Menu")]
        private MenuOptions _menuOption = MenuOptions.Positions;

        private bool ShowMembers => _menuOption == MenuOptions.Members;
        private bool ShowPositions => _menuOption == MenuOptions.Positions;
        private bool ShowRoles => _menuOption == MenuOptions.Roles;
#endif


        // ------------ MEMBERS ------------ 
        [ShowInInspector, ShowIf("ShowMembers")]
        private readonly List<CombatEntity> _members;

        public IReadOnlyList<CombatEntity> FrontLineType => _positionWrapper.FrontLineType;
        public IReadOnlyList<CombatEntity> MidLineType => _positionWrapper.MidLineType;
        public IReadOnlyList<CombatEntity> BackLineType => _positionWrapper.BackLineType;

        public void GetMainPositioningMembers(out CombatEntity frontLine, out CombatEntity midLine,
            out CombatEntity backLine)
        {
            frontLine = GetEntity(FrontLineType);
            midLine = GetEntity(MidLineType);
            backLine = GetEntity(BackLineType);
        }
        private static CombatEntity GetEntity(IReadOnlyList<CombatEntity> group)
        {
            return group.Count == 0 ? null : group[0];
        }

        [ShowInInspector, ShowIf("ShowPositions")]
        private readonly PositionWrapper _positionWrapper;
        [ShowInInspector, ShowIf("ShowRoles")]
        private readonly RoleWrapper _roleWrapper;


        /// <summary>
        /// Main Vanguard Role
        /// </summary>
        public CombatEntity VanguardType => _roleWrapper.VanguardType.GetMainMember();

        /// <summary>
        /// Main Attacker Role
        /// </summary>
        public CombatEntity AttackerType => _roleWrapper.AttackerType.GetMainMember();

        /// <summary>
        /// Main Support Role
        /// </summary>
        public CombatEntity SupportType => _roleWrapper.SupportType.GetMainMember();




        public IReadOnlyList<CombatEntity> GetMemberGroup(in CombatEntity entity)
        {
            return Contains(entity) 
                ? UtilsTeam.GetElement(entity.PositioningType, this) 
                : null;
        }
        private PositionGroup GetPositionGroup(in CombatEntity entity)
        {
            return UtilsTeam.GetElement(entity.PositioningType, _positionWrapper);
        }

        private RoleGroup GetRoleGroup(in CombatEntity entity)
        {
            return UtilsTeam.GetElement(entity.RoleType, _roleWrapper);
        }


        public void CreateMidCombat(ICombatEntityProvider provider)
        {
            CombatEntity entity = new CombatEntity(provider,this);
            CombatSystemSingleton.EventsHolder.OnCreateEntity(in entity, in IsPlayerTeam);
            Add(entity);
        }

        private void Add(ICombatEntityProvider provider)
        {
            if(provider == null) return;

            CombatEntity entity = new CombatEntity(provider,this);
            Add(entity);
        }
        
        private void Add(CombatEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }

            _positionWrapper.AddMember(in entity);
            _roleWrapper.AddMember(in entity);
            _members.Add(entity);
        }

        public void SwitchTeamMemberToThis(CombatEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }
            entity.SwitchTeam(this);
            Add(entity);
        }

        public bool Remove(CombatEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }

            bool contains = _members.Contains(entity);
            if (!contains) return false;

            _positionWrapper.RemoveMember(in entity);
            _roleWrapper.RemoveMember(in entity);
            _members.Remove(entity);
            CombatSystemSingleton.EventsHolder.OnDestroyEntity(in entity, in IsPlayerTeam);

            return true;
        }
        


        public void Clear()
        {
            _members.Clear();
            _positionWrapper.Clear();
            _roleWrapper.Clear();
        }

        public bool Contains(CombatEntity item)
        {
            return _members.Contains(item);
        }


        // ------------ OTHErS ------------ 
        public CombatTeam EnemyTeam { get; private set; }

        public void Injection(CombatTeam enemyTeam) => EnemyTeam = enemyTeam;
        public IEnumerator<CombatEntity> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _members.Count;
        public CombatEntity this[int index] => _members[index];




        //This is just for ICollection (utility)
        private sealed class PositionWrapper : ITeamPositionStructureRead<PositionGroup>
        {
            public PositionWrapper()
            {
                FrontLineType = new PositionGroup(EnumTeam.Positioning.FrontLine);
                MidLineType = new PositionGroup(EnumTeam.Positioning.MidLine);
                BackLineType = new PositionGroup(EnumTeam.Positioning.BackLine);
            }

            [ShowInInspector]
            public PositionGroup FrontLineType { get; private set; }
            [ShowInInspector]
            public PositionGroup MidLineType { get; private set; }
            [ShowInInspector]
            public PositionGroup BackLineType { get; private set; }

            public void AddMember(in CombatEntity member)
            {
                EnumTeam.Positioning targetPosition = member.PositioningType;
                AddMember(in targetPosition,in member);
            }

            public void AddMember(in EnumTeam.Positioning targetPosition, in CombatEntity member)
            {
                switch (targetPosition)
                {
                    case EnumTeam.Positioning.FrontLine:
                        AddToGroup(in member, FrontLineType, out bool isMainVanguard);
                        if(isMainVanguard)
                            CombatSystemSingleton.EventsHolder.OnMainFrontLineSwitch(in member);
                        break;


                    case EnumTeam.Positioning.MidLine:
                        AddToGroup(in member,MidLineType, out bool isMainAttacker);
                        if(isMainAttacker)
                            CombatSystemSingleton.EventsHolder.OnMainMidLineSwitch(in member);
                        break;


                    case EnumTeam.Positioning.BackLine:
                        AddToGroup(in member,BackLineType, out bool isMainSupport);
                        if(isMainSupport)
                            CombatSystemSingleton.EventsHolder.OnMainBackLineSwitch(in member);
                        break;


                    default:
                        throw new ArgumentOutOfRangeException();
                }

                void AddToGroup(in CombatEntity entity, PositionGroup group, out bool isMainMember)
                {
                    isMainMember = group.Count == 0;
                    group.Add(in entity);
                }
            }

            public void RemoveMember(in CombatEntity member)
            {
                var position = member.PositioningType;
                RemoveMember(in position, in member);
            }

            public void RemoveMember(in EnumTeam.Positioning targetPosition, in CombatEntity member)
            {
                switch (targetPosition)
                {
                    case EnumTeam.Positioning.FrontLine:
                        RemoveFromGroup(in member, FrontLineType, out bool isMainVanguard);
                        if (isMainVanguard)
                        {
                            var switchFrontLine = FrontLineType[0];
                            CombatSystemSingleton.EventsHolder.OnMainFrontLineSwitch(in switchFrontLine);
                        }
                        break;


                    case EnumTeam.Positioning.MidLine:
                        RemoveFromGroup(in member, MidLineType, out bool isMainAttacker);
                        if (isMainAttacker)
                        {
                            var switchMidLine = MidLineType[0];
                            CombatSystemSingleton.EventsHolder.OnMainMidLineSwitch(in switchMidLine);
                        }

                        break;


                    case EnumTeam.Positioning.BackLine:
                        RemoveFromGroup(in member, BackLineType, out bool isMainSupport);
                        if (isMainSupport)
                        {
                            var switchBackLine = BackLineType[0];
                            CombatSystemSingleton.EventsHolder.OnMainBackLineSwitch(in switchBackLine);
                        }

                        break;


                    default:
                        throw new ArgumentOutOfRangeException();
                }

                void RemoveFromGroup(in CombatEntity entity, PositionGroup group, out bool isMainMember)
                {
                    isMainMember = group.IsMainEntity(in entity);
                    group.Remove(in entity);
                }
            }

            public void Clear()
            {
                FrontLineType.Clear();
                MidLineType.Clear();
                BackLineType.Clear();
            }
        }

        private sealed class RoleWrapper : ITeamRoleStructureRead<RoleGroup>
        {
            public RoleWrapper()
            {
                VanguardType = new RoleGroup(EnumTeam.Role.Vanguard);
                AttackerType = new RoleGroup(EnumTeam.Role.Attacker);
                SupportType = new RoleGroup(EnumTeam.Role.Support);
            }
            [ShowInInspector]
            public RoleGroup VanguardType { get; }
            [ShowInInspector]
            public RoleGroup AttackerType { get; }
            [ShowInInspector]
            public RoleGroup SupportType { get; }

            public void AddMember(in CombatEntity entity)
            {
                var roleType = entity.RoleType;
                var targetGroup = UtilsTeam.GetElement(roleType, this);
                targetGroup.Add(entity);
            }

            public void RemoveMember(in CombatEntity entity)
            {
                var roleType = entity.RoleType;
                var targetGroup = UtilsTeam.GetElement(roleType, this);
                targetGroup.Remove(entity);
            }

            public void Clear()
            {
                VanguardType.Clear();
                AttackerType.Clear();
                SupportType.Clear();
            }
        }
    }

    [Serializable]
    public struct TeamAreaData : ITeamAreaDataRead
    {
        public TeamAreaData(EnumTeam.Role roleType, EnumTeam.Positioning positioningType)
        {
            role = roleType;
            positioning = positioningType;
        }

        public TeamAreaData(ITeamAreaDataRead data) : this(data.RoleType, data.PositioningType)
        { }

        public TeamAreaData(TeamAreaData data) : this(data.RoleType,data.PositioningType)
        { }

        [SerializeField] private EnumTeam.Role role;
        [SerializeField] private EnumTeam.Positioning positioning;

        public EnumTeam.Role RoleType => role;
        public EnumTeam.Positioning PositioningType => positioning;
    }
}
