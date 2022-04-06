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
        ITeamFullPositionStructureRead<CombatEntity>,
        ITeamFullRoleStructureRead<CombatEntity>,
        IReadOnlyList<CombatEntity>
    {
        private CombatTeam(bool isPlayerTeam)
        {
            IsPlayerTeam = isPlayerTeam;

            DataValues = new TeamDataValues();

            _members = new List<CombatEntity>();

            StandByMembers = new TeamStandByMembersHandler();
            _mainPositionsWrapper = new MainPositionsWrapper();
            _mainRoleWrapper = new MainRoleWrapper();

            AliveTargeting = new CombatTeamAliveTargeting(this);

            _offRolesGroup = new TeamOffGroupStructure<CombatEntity>();
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

        [ShowInInspector]
        public readonly TeamStandByMembersHandler StandByMembers;

        
        [ShowInInspector, ShowIf("ShowPositions")]
        private readonly MainPositionsWrapper _mainPositionsWrapper;
        [ShowInInspector, ShowIf("ShowRoles")]
        private readonly MainRoleWrapper _mainRoleWrapper;

        private readonly TeamOffGroupStructure<CombatEntity> _offRolesGroup;


        public IReadOnlyList<CombatEntity> MainRoleMembers => _mainRoleWrapper.Members;
        public IReadOnlyList<CombatEntity> MainPositioningMembers => _mainPositionsWrapper.Members;
        public IEnumerator<CombatEntity> OffRoleMembers => _offRolesGroup;


        public CombatEntity GetSecondaryMember(EnumTeam.ActiveRole role) =>
            UtilsTeam.GetElement(role, _offRolesGroup);

        public CombatEntity FrontLineType => _mainPositionsWrapper.FrontLineType;
        public CombatEntity MidLineType => _mainPositionsWrapper.MidLineType;
        public CombatEntity BackLineType => _mainPositionsWrapper.BackLineType;
        public CombatEntity FlexLineType => _mainPositionsWrapper.FlexLineType;


        /// <summary>
        /// Main Vanguard Role
        /// </summary>
        public CombatEntity VanguardType => _mainRoleWrapper.VanguardType;

        /// <summary>
        /// Main Attacker Role
        /// </summary>
        public CombatEntity AttackerType => _mainRoleWrapper.AttackerType;

        /// <summary>
        /// Main Support Role
        /// </summary>
        public CombatEntity SupportType => _mainRoleWrapper.SupportType;
        /// <summary>
        /// Main Flex Role
        /// </summary>
        public CombatEntity FlexType => _mainRoleWrapper.FlexType;



        public readonly CombatTeamAliveTargeting AliveTargeting;

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

            _mainPositionsWrapper.AddMember(in entity);
            _mainRoleWrapper.AddMember(in entity);
            _members.Add(entity);


            AliveTargeting.AddMember(in entity);
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

            _mainPositionsWrapper.RemoveMember(in entity);
            _mainRoleWrapper.RemoveMember(in entity);
            _members.Remove(entity);

            AliveTargeting.RemoveMember(in entity);

            CombatSystemSingleton.EventsHolder.OnDestroyEntity(in entity, in IsPlayerTeam);

            return true;
        }
        


        public void Clear()
        {
            _members.Clear();
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

        public bool IsMainRole(in CombatEntity entity) => _mainRoleWrapper.IsMainRole(in entity);
        public bool IsTrinityRole(in CombatEntity entity) => _mainRoleWrapper.IsTrinityRole(in entity);

        //This is just for ICollection (utility)
        private sealed class MainPositionsWrapper : FullPositionMainGroupStructure<CombatEntity>
        {
            public void AddMember(in CombatEntity member)
            {
                var positioning = member.PositioningType;
                AddMember(in positioning, in member);
            }

            private void AddMember(in EnumTeam.Positioning targetPosition, in CombatEntity member)
            {
                int positioningIndex = EnumTeam.GetPositioningIndex(in targetPosition);
                bool isMainRole = Members[positioningIndex] == null;
                if (isMainRole) Members[positioningIndex] = member;
            }

            public void RemoveMember(in CombatEntity member)
            {
                var position = member.PositioningType;
                RemoveMember(in position, in member);
            }

            private void RemoveMember(in EnumTeam.Positioning targetPosition, in CombatEntity member)
            {
                int positioningIndex = EnumTeam.GetPositioningIndex(in targetPosition);
                bool isMainRole = Members[positioningIndex] == member;
                if (isMainRole) Members[positioningIndex] = null;
            }
        }

        private sealed class MainRoleWrapper : FullPositionMainGroupStructure<CombatEntity>
        {
            public void AddMember(in CombatEntity entity)
            {
                int roleIndex = EnumTeam.GetRoleIndex(entity.RoleType);
                if (Members[roleIndex] == null) Members[roleIndex] = entity;
            }

            public void RemoveMember(in CombatEntity entity)
            {
                int roleIndex = EnumTeam.GetRoleIndex(entity.RoleType);
                if (Members[roleIndex] == entity) Members[roleIndex] = null;
            }


            public bool IsMainRole(in CombatEntity member)
            {
                foreach (var entity in Members)
                {
                    if (member == entity) return true;
                }

                return false;
            }

            public bool IsTrinityRole(in CombatEntity member)
            {
                for (int i = 0; i < EnumTeam.BasicRolesAmount; i++)
                {
                    if (Members[i] == member) return true;
                }

                return false;
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
