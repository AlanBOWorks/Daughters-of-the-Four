using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class PositionGroup : IReadOnlyList<CombatEntity>
    {
        public PositionGroup(EnumTeam.Positioning groupType)
        {
            GroupType = groupType;
            _members = new List<CombatEntity>();
        }

        public readonly EnumTeam.Positioning GroupType;
        [ShowInInspector]
        private readonly List<CombatEntity> _members;

        public bool IsMainEntity(in CombatEntity entity) => _members[0] == entity;

        public void Add(in CombatEntity entity)
        {
            entity.SwitchPositioning(this);
            _members.Add(entity);
        }
        public void Remove(in CombatEntity entity)
        {
            _members.Remove(entity);
        }

        public void SwitchToThis(in CombatEntity entity)
        {
            var previousGroup = entity.PositionGroup;
            previousGroup?.Remove(in entity);

            Add(in entity);
        }

        public void Clear() => _members.Clear();

        #region ---- IReadOnlyList<CombatEntity> ----
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
        #endregion
    }
}
