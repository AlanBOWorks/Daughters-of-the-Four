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
            _activeMembers = new Dictionary<CombatEntity, bool>();
            _allControllingMembers = new List<CombatEntity>();
            _trinityControllingMembers = new List<CombatEntity>();
            _offControllingMembers = new List<CombatEntity>();
        }

        private readonly Dictionary<CombatEntity, bool> _activeMembers;

        [ShowInInspector, HorizontalGroup()] 
        private readonly List<CombatEntity> _allControllingMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _trinityControllingMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _offControllingMembers;

        public IReadOnlyDictionary<CombatEntity, bool> GetActiveMembers() => _activeMembers;
        public IReadOnlyList<CombatEntity> GetAllMembers() => _allControllingMembers;
        public IReadOnlyList<CombatEntity> GetTrinityMembers() => _trinityControllingMembers;
        public IReadOnlyList<CombatEntity> GetOffMembers() => _offControllingMembers;

        /// <summary>
        /// Check if it has entities that reach the initiative threshold (but not if can act)
        /// </summary>
        public bool IsActive() => _activeMembers.Count > 0;
        /// <summary>
        /// If has entities than can act
        /// </summary>
        public bool CanControl() => _allControllingMembers.Count > 0;


        public void AddActiveEntity(in CombatEntity entity, in bool canControl)
        {
            _activeMembers.Add(entity,canControl);
            if(!canControl) return;

            _allControllingMembers.Add(entity);
            bool isTrinity = UtilsTeam.IsTrinityRole(in entity);
            if(isTrinity)
                _trinityControllingMembers.Add(entity);
            else 
                _offControllingMembers.Add(entity);
        }

        public void SafeRemoveActive(in CombatEntity entity)
        {
            if (!_activeMembers.ContainsKey(entity)) return;
            _activeMembers.Remove(entity);
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


        public void Clear()
        {
            _activeMembers.Clear();
            _allControllingMembers.Clear();
            _trinityControllingMembers.Clear();
            _offControllingMembers.Clear();
        }

        
    }
}
