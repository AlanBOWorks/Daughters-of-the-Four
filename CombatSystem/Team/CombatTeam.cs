using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeam : 
        ITeamPositionStructureRead<IReadOnlyList<CombatEntity>>,
        ITeamRoleStructureRead<IReadOnlyList<CombatEntity>>,
        IReadOnlyList<CombatEntity>,
        IStanceDataRead
    {
        private CombatTeam(int membersLength)
        {
            Stats = new TeamStats();

            _members = new List<CombatEntity>(membersLength);

            _frontLine = new List<CombatEntity>();
            _midLine = new List<CombatEntity>();
            _backLine = new List<CombatEntity>();

            _groupWrapper = new GroupWrapper
            {
                FrontLineType = _frontLine,
                MidLineType = _midLine,
                BackLineType = _backLine
            };
        }


        public CombatTeam(ITeamPositionStructureRead<ICombatEntityProvider> members)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            Add(members.FrontLineType);
            Add(members.MidLineType);
            Add(members.BackLineType);
        }
        public CombatTeam(IReadOnlyCollection<ICombatEntityProvider> members) : this(members.Count)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));


            foreach (var provider in members)
            {
                Add(provider);
            }
        }


        // ------------ DATA ------------ 
        public readonly TeamStats Stats;

        // ------------ MEMBERS ------------ 

        private readonly List<CombatEntity> _members;
        [ShowInInspector]
        private readonly List<CombatEntity> _frontLine;
        [ShowInInspector]
        private readonly List<CombatEntity> _midLine;
        [ShowInInspector]
        private readonly List<CombatEntity> _backLine;

        private readonly GroupWrapper _groupWrapper;

        
        private sealed class GroupWrapper : ITeamPositionStructureRead<ICollection<CombatEntity>>
        {
            public ICollection<CombatEntity> FrontLineType { get; set; }
            public ICollection<CombatEntity> MidLineType { get; set; }
            public ICollection<CombatEntity> BackLineType { get; set; }
        }


        public IReadOnlyList<CombatEntity> FrontLineType => _frontLine;
        public IReadOnlyList<CombatEntity> MidLineType => _midLine;
        public IReadOnlyList<CombatEntity> BackLineType => _backLine;

        public IReadOnlyList<CombatEntity> VanguardType => _frontLine;
        public IReadOnlyList<CombatEntity> AttackerType => _midLine;
        public IReadOnlyList<CombatEntity> SupportType => _backLine;




        public IReadOnlyList<CombatEntity> GetMemberGroup(in CombatEntity entity)
        {
            return Contains(entity) 
                ? UtilsTeam.GetElement(entity.PositioningType, this) 
                : null;
        }
        private ICollection<CombatEntity> GetGroup(in CombatEntity entity)
        {
            return UtilsTeam.GetElement(entity.PositioningType, _groupWrapper);
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

            var group = GetGroup(in entity);
            group.Add(entity);
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

            var group = GetGroup(in entity);
            _members.Remove(entity);
            group.Remove(entity);

            entity.SwitchTeam(null);
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
        public EnumTeam.StanceFull CurrentStance { get; set; }

        public CombatEntity this[int index] => _members[index];
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
