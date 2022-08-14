using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Stats;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeam 
    {
        private CombatTeam(bool isPlayerTeam)
        {
            IsPlayerTeam = isPlayerTeam;

            DataValues = new TeamDataValues();
            GuardHandler = new TeamLineBlockerHandler();


            _membersHolder = new CombatTeamMembersHolder();
            _controlMembers = new CombatTeamControlMembers();
        }


        private CombatTeam(bool isPlayerTeam,IEnumerable<ICombatEntityProvider> members) : this(isPlayerTeam)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));


            foreach (var memberProvider in members)
            {
                GenerateEntityAndAdd(memberProvider);
            }

            IReadOnlyList<CombatEntity> mainVanguards = UtilsTeam.GetFrontMostElement(_membersHolder as
                ITeamFlexStructureRead<IReadOnlyList<CombatEntity>>);
            
            var mainVanguardResponsible = mainVanguards[0];
            if(mainVanguardResponsible == null) return;

            VanguardEffectsHolder = new VanguardEffectsHolder(mainVanguardResponsible);
        }

        public CombatTeam(bool isPlayerTeam, ICombatTeamProvider provider) : this(isPlayerTeam,provider.GetSelectedCharacters())
        { }


        [Title("Team Data")]
        public readonly bool IsPlayerTeam;


        // ------------ DATA ------------ 
        [ShowInInspector]
        public readonly TeamDataValues DataValues;
        public readonly TeamLineBlockerHandler GuardHandler;
        [ShowInInspector]
        public readonly VanguardEffectsHolder VanguardEffectsHolder;










        // ------------ MEMBERS ------------ 
        [ShowInInspector, Title("All Members")]
        private readonly CombatTeamMembersHolder _membersHolder;

        private void GenerateEntityAndAdd(ICombatEntityProvider provider)
        {
            if (provider == null) return;

            CombatEntity entity = new CombatEntity(provider, this);
            _membersHolder.AddMember(entity);
        }

        public bool Contains(CombatEntity entity)
        {
            return entity.Team == this;
        }


        public IReadOnlyList<CombatEntity> GetAllMembers() => _membersHolder.AllMembers;
        public IReadOnlyList<CombatEntity> GetMainRoles() => _membersHolder.GetMainRoles();
        public IReadOnlyList<CombatEntity> GetOffRoles() => _membersHolder.GetOffRoles();
        public IEnumerable<CombatEntity> GetSecondaryRoles() => _membersHolder.GetSecondaryRoles();
        public IEnumerable<CombatEntity> GetThirdRoles() => _membersHolder.GetThirdRoles();
        public ITeamAlimentStructureRead<IEnumerable<CombatEntity>> GetRolesAliments() => _membersHolder.GetAlimentRoles();
        public ITeamFlexStructureRead<IReadOnlyList<CombatEntity>> GetRolesStructures() => _membersHolder;


        [ShowInInspector]
        private readonly CombatTeamControlMembers _controlMembers;
        public ITeamFlexPositionStructureRead<CombatEntity> GetMainPositions() => _membersHolder.GetMainPositions();
        public ITeamFlexPositionStructureRead<IReadOnlyCollection<CombatEntity>> GetAllPositions() => _membersHolder;
        public ITeamFlexStructureRead<IReadOnlyCollection<CombatEntity>> GetAllRoles() => _membersHolder;
        public ITeamFullStructureRead<CombatEntity> GetAllEntities() => _membersHolder;


       
        public bool IsActive(CombatEntity member) => _controlMembers.IsActive(member);
        /// <summary>
        /// <inheritdoc cref="CombatTeamControlMembers.CanControl()"/>
        /// </summary>
        public bool CanControl() => _controlMembers.CanControl();
        public bool HasTrinityControl() => _controlMembers.GetControllingTrinityMembers().Count > 0;
        public IEnumerable<CombatEntity> GetNonControllingMembers() => _controlMembers.GetNonControllingMembers();
        public IReadOnlyList<CombatEntity> GetTrinityActiveMembers() => _controlMembers.GetControllingTrinityMembers();
        public IReadOnlyList<CombatEntity> GetOffMembersActiveMembers() => _controlMembers.GetControllingOffMembers();
        public IReadOnlyList<CombatEntity> GetControllingMembers() => _controlMembers.GetAllControllingMembers();

        public bool IsMainRole(in CombatEntity entity) => _membersHolder.IsMainRole(entity);
        public bool IsTrinityRole(in CombatEntity entity) => _membersHolder.IsTrinityRole(entity);

        

        public void ClearNonControllingMembers()
        {
            _controlMembers.ClearNotControllingMembers();
        }

        public void ClearControllingMembers()
        {
            _controlMembers.Clear();
        }

        public void AddActiveEntity(CombatEntity entity, bool canControl)
        {
            _controlMembers.AddActiveEntity(entity, canControl);
        }

        public void OnEntityRequestSequence(CombatEntity entity)
        {
            GuardHandler.OnEntityRequestSequence(entity);
        }
        public void RemoveFromControllingEntities(CombatEntity entity, bool isForcedByController)
        {
            if (isForcedByController) return;
            //This will be removed with Clear OnTempoForceFinish
            _controlMembers.SafeRemoveControlling(entity);
        }

        public void OnTempoForceFinish()
        {
            _controlMembers.Clear();
        }

        public void OnControlStart()
        {
            
        }
        public void OnControlFinnish()
        {
            ClearControllingMembers();
            var allMembers = GetAllMembers();
            foreach (var member in allMembers)
            {
                UtilsCombatStats.ResetActions(member.Stats);
            }
        }

        // ------------ OTHErS ------------ 
        public CombatTeam EnemyTeam { get; private set; }

        public void Injection(CombatTeam enemyTeam) => EnemyTeam = enemyTeam;


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
        public EnumTeam.Positioning PositioningType => (positioning == EnumTeam.Positioning.ByRolePosition)
        ? UtilsTeam.GetEquivalent(role) : positioning;
    }
}
