using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    internal sealed class CombatTeamActiveMembers : ITempoDedicatedEntityStatesListener
    {
        public CombatTeamActiveMembers()
        {
            _allEntities = new List<CombatEntity>();
            _trinityMembers = new List<CombatEntity>();
            _offMembers = new List<CombatEntity>();
        }

        [ShowInInspector, HorizontalGroup()] 
        private readonly List<CombatEntity> _allEntities;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _trinityMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _offMembers;

        public IReadOnlyList<CombatEntity> GetAllMembers() => _allEntities;
        public IReadOnlyList<CombatEntity> GetTrinityMembers() => _trinityMembers;
        public IReadOnlyList<CombatEntity> GetOffMembers() => _offMembers;


        public bool IsActive() => _allEntities.Count > 0;

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;
            _allEntities.Add(entity);
            _trinityMembers.Add(entity);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;
            _allEntities.Add(entity);
            _offMembers.Add(entity);
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
        }

        public void SafeRemove(in CombatEntity entity)
        {
            if(!_allEntities.Contains(entity)) return;
            _allEntities.Remove(entity);

            if (_trinityMembers.Contains(entity))
            {
                _trinityMembers.Remove(entity);
                return;
            }
            if(!_offMembers.Contains(entity)) return;

            _offMembers.Remove(entity);
        }


        public void Clear()
        {
            _allEntities.Clear();
            _trinityMembers.Clear();
            _offMembers.Clear();
        }

        
    }
}
