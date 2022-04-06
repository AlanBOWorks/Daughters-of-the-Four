using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;

namespace CombatSystem.Team
{
    public sealed class TeamStandByMembersHandler
    {
        public TeamStandByMembersHandler()
        {
            _members = new HashSet<CombatEntity>();
        }
        [ShowInInspector]
        private readonly HashSet<CombatEntity> _members;


        public void PutOnStandBy(in CombatEntity member)
        {
            if(_members.Contains(member)) return;
            _members.Add(member);
        }

        public void RemoveFromStandBy(in CombatEntity member)
        {
            if (!_members.Contains(member)) return;
            _members.Remove(member);
        }
    }
}
