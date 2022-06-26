using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    internal sealed class CombatTeamControlMembers 
    {
        public CombatTeamControlMembers()
        {
            _nonControllingMembers = new HashSet<CombatEntity>();
            _allControllingMembers = new List<CombatEntity>();
            _trinityControllingMembers = new List<CombatEntity>();
            _offControllingMembers = new List<CombatEntity>();
        }

        [ShowInInspector, HorizontalGroup()] 
        private readonly HashSet<CombatEntity> _nonControllingMembers;

        [ShowInInspector, HorizontalGroup()] 
        private readonly List<CombatEntity> _allControllingMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _trinityControllingMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _offControllingMembers;

        public IEnumerable<CombatEntity> GetNonControllingMembers() => _nonControllingMembers;
        public IReadOnlyList<CombatEntity> GetAllControllingMembers() => _allControllingMembers;
        public IReadOnlyList<CombatEntity> GetControllingTrinityMembers() => _trinityControllingMembers;
        public IReadOnlyList<CombatEntity> GetControllingOffMembers() => _offControllingMembers;

        /// <summary>
        /// Check if it has entities that reach the initiative threshold (but not if can act)
        /// </summary>
        public bool IsActive() => _allControllingMembers.Count > 0 || _nonControllingMembers.Count > 0;
        /// <summary>
        /// If has entities than can act
        /// </summary>
        public bool CanControl() => _allControllingMembers.Count > 0;

        public bool IsActive(CombatEntity member) => _allControllingMembers.Contains(member);

        public void AddActiveEntity(CombatEntity entity, bool canControl)
        {
            if(!canControl)
            {
                _nonControllingMembers.Add(entity);
                return;
            }

            _allControllingMembers.Add(entity);
            bool isTrinity = UtilsTeam.IsTrinityRole(in entity);
            if(isTrinity)
                _trinityControllingMembers.Add(entity);
            else 
                _offControllingMembers.Add(entity);
        }


        public void SafeRemoveControlling(in CombatEntity entity)
        {

            if (!_allControllingMembers.Contains(entity)) return;
            _allControllingMembers.Remove(entity);

            if (_trinityControllingMembers.Contains(entity))
            {
                _trinityControllingMembers.Remove(entity);
                return;
            }
            if(!_offControllingMembers.Contains(entity)) return;

            _offControllingMembers.Remove(entity);
        }


        public void ClearNotControllingMembers()
        {
            _nonControllingMembers.Clear();
        }

        public void Clear()
        {
            _nonControllingMembers.Clear();
            _allControllingMembers.Clear();
            _trinityControllingMembers.Clear();
            _offControllingMembers.Clear();
        }

        
    }
}
