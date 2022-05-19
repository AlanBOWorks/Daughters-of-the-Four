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
        ITeamFullRolesStructureRead<CombatEntity>,
        ITeamFlexPositionStructureRead<IEnumerable<CombatEntity>>,
        IReadOnlyList<CombatEntity>
    {
        private CombatTeam(bool isPlayerTeam)
        {
            IsPlayerTeam = isPlayerTeam;

            DataValues = new TeamDataValues();
            GuardHandler = new TeamLineBlockerHandler();

            _members = new List<CombatEntity>();

            StandByMembers = new TeamStandByMembersHandler();

            _mainPositionsWrapper = new MainPositionsWrapper();
            _mainRoleWrapper = new MainRoleWrapper();
            _offRolesGroup = new TeamOffGroupStructure<CombatEntity>();

            _controlMembers = new CombatTeamControlMembers();

            _teamSkills = new List<CombatTeamSkill>();
        }

        private CombatTeam(bool isPlayerTeam,IEnumerable<ICombatEntityProvider> members) : this(isPlayerTeam)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));


            foreach (var provider in members)
            {
                Add(provider);
            }
        }

        public CombatTeam(bool isPlayerTeam, ICombatTeamProvider provider) : this(isPlayerTeam,provider.GetSelectedCharacters())
        {
            var skills = provider.GetTeamSkills();
            foreach (var skillPreset in skills)
            {
                Add(skillPreset);
            }
        }


        [Title("Team Data")]
        public readonly bool IsPlayerTeam;


        // ------------ DATA ------------ 
        public readonly TeamDataValues DataValues;
        public readonly TeamLineBlockerHandler GuardHandler;
        [ShowInInspector]
        private readonly List<CombatTeamSkill> _teamSkills;

        public IReadOnlyList<CombatTeamSkill> GetTeamSkills() => _teamSkills;

        public void InstantiationSkill(IEnumerable<ITeamSkillPreset> skills)
        {
            _teamSkills.Clear();
            foreach (var preset in skills)
            {
                CombatTeamSkill skill = new CombatTeamSkill(preset);
                _teamSkills.Add(skill);
            }
        }


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

        [ShowInInspector, ShowIf("ShowRoles")]
        private readonly TeamOffGroupStructure<CombatEntity> _offRolesGroup;
        [ShowInInspector]
        private readonly CombatTeamControlMembers _controlMembers;

        public int Count => _members.Count;

        public bool IsActive() => _controlMembers.IsActive();
        public bool CanControl() => _controlMembers.CanControl();

        public IReadOnlyList<CombatEntity> MainRoleMembers => _mainRoleWrapper.Members;
        public IReadOnlyList<CombatEntity> MainPositioningMembers => _mainPositionsWrapper.Members;
        public FlexPositionMainGroupStructure<CombatEntity> GetMainMembers() => _mainRoleWrapper;

        public IEnumerable<CombatEntity> GetNonControllingMembers() => _controlMembers.GetNonControllingMembers();
        public IReadOnlyList<CombatEntity> GetTrinityActiveMembers() => _controlMembers.GetControllingTrinityMembers();
        public IReadOnlyList<CombatEntity> GetOffMembersActiveMembers() => _controlMembers.GetControllingOffMembers();
        public IReadOnlyList<CombatEntity> GetControllingMembers() => _controlMembers.GetAllControllingMembers();

        public IEnumerable<CombatEntity> OffRoleMembers => _offRolesGroup;
        public TeamOffGroupStructure<CombatEntity> GetOffMembersStructure() => _offRolesGroup;

        public IEnumerable<CombatEntity> FrontLineType =>
            GetEnumerable(_mainPositionsWrapper.FrontLineType, _offRolesGroup.FrontLineType);
        public IEnumerable<CombatEntity> MidLineType =>
            GetEnumerable(_mainPositionsWrapper.MidLineType, _offRolesGroup.MidLineType);
        public IEnumerable<CombatEntity> BackLineType =>
            GetEnumerable(_mainPositionsWrapper.BackLineType, _offRolesGroup.BackLineType);
        public IEnumerable<CombatEntity> FlexLineType =>
            GetEnumerable(_mainPositionsWrapper.FlexLineType, _offRolesGroup.FlexLineType);

        private static IEnumerable<CombatEntity> GetEnumerable(CombatEntity mainMember,
            IEnumerable<CombatEntity> offMembers)
        {
            yield return mainMember;
            foreach (var member in offMembers)
            {
                yield return member;
            }
        }

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

        public CombatEntity SecondaryVanguardElement => _offRolesGroup.SecondaryVanguardElement;
        public CombatEntity SecondaryAttackerElement => _offRolesGroup.SecondaryAttackerElement;
        public CombatEntity SecondarySupportElement => _offRolesGroup.SecondarySupportElement;
        public CombatEntity SecondaryFlexElement => _offRolesGroup.SecondaryFlexElement;
        public CombatEntity ThirdVanguardElement => _offRolesGroup.ThirdVanguardElement;
        public CombatEntity ThirdAttackerElement => _offRolesGroup.ThirdAttackerElement;
        public CombatEntity ThirdSupportElement => _offRolesGroup.ThirdSupportElement;
        public CombatEntity ThirdFlexElement => _offRolesGroup.ThirdFlexElement;



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

        private void Add(ITeamSkillPreset preset)
        {
            if(preset == null) return;

            CombatTeamSkill skill = new CombatTeamSkill(preset);
            Add(skill);
        }


        private void Add(CombatEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Passed member in team is null");
            }

            var roleType = entity.RoleType;
            var positioning = entity.PositioningType;

            _members.Add(entity);
            _mainPositionsWrapper.AddMember(in positioning,in entity);
            _mainRoleWrapper.AddMember(in entity, in roleType, out bool isMainAddition);

            if(!isMainAddition) _offRolesGroup.AddMember(roleType, in entity);
        }

        private void Add(CombatTeamSkill skill)
        {
            _teamSkills.Add(skill);
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

            CombatSystemSingleton.EventsHolder.OnDestroyEntity(in entity, in IsPlayerTeam);

            return true;
        }

        public void ClearNonControllingMembers()
        {
            _controlMembers.ClearNotControllingMembers();
        }

        public void ClearControllingMembers()
        {
            _controlMembers.Clear();
        }

        public void AddActiveEntity(in CombatEntity entity, in bool canControl)
        {
            _controlMembers.AddActiveEntity(in entity, in canControl);
        }

        public void OnEntityRequestSequence(CombatEntity entity)
        {
            GuardHandler.OnEntityRequestSequence(in entity);
        }
        public void RemoveFromControllingEntities(CombatEntity entity, in bool isForcedByController)
        {
            if (isForcedByController) return;
            //This will be removed with Clear OnTempoForceFinish
            _controlMembers.SafeRemoveControlling(in entity);
        }

        public void OnTempoForceFinish()
        {
            _controlMembers.Clear();
        }


        public IEnumerator<CombatEntity> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public CombatEntity this[int index] => _members[index];


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
        


        public bool IsMainRole(in CombatEntity entity) => _mainRoleWrapper.IsMainRole(in entity);
        public bool IsTrinityRole(in CombatEntity entity) => _mainRoleWrapper.IsTrinityRole(in entity);

        //This is just for ICollection (utility)
        private sealed class MainPositionsWrapper : FlexPositionMainGroupStructure<CombatEntity>
        {
            public void AddMember(in CombatEntity member)
            {
                var positioning = member.PositioningType;
                AddMember(in positioning, in member);
            }

            public void AddMember(in EnumTeam.Positioning targetPosition, in CombatEntity member)
            {
                int positioningIndex = EnumTeam.GetPositioningIndex(in targetPosition);
                bool isMainRole = Members[positioningIndex] == null;
                if (!isMainRole) return;

                Members[positioningIndex] = member;
                member.ActiveRole = EnumTeam.ParseMainActiveRole(in targetPosition);
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

        private sealed class MainRoleWrapper : FlexPositionMainGroupStructure<CombatEntity>
        {
            public void AddMember(in CombatEntity entity, in EnumTeam.Role role, out bool isMainEntity)
            {
                int roleIndex = EnumTeam.GetRoleIndex(role);
                isMainEntity = Members[roleIndex] == null;
                if (isMainEntity) Members[roleIndex] = entity;
            }

            public void AddMember(in CombatEntity entity, out bool isMainEntity)
            {
                AddMember(in entity, entity.RoleType, out isMainEntity);
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
