using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;

namespace CombatSystem.Team
{
    internal sealed class CombatTeamActiveMembers : ITempoDedicatedEntityStatesListener
    {
        public CombatTeamActiveMembers()
        {
            _trinityMembers = new List<CombatEntity>();
            _offMembers = new List<CombatEntity>();
        }
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _trinityMembers;
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _offMembers;

        public IReadOnlyList<CombatEntity> GetTrinityMembers() => _trinityMembers;
        public IReadOnlyList<CombatEntity> GetOffMembers() => _offMembers;

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;
            _trinityMembers.Add(entity);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;
            _offMembers.Add(entity);
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            _trinityMembers.Remove(entity);
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            _offMembers.Remove(entity);
        }
    }
}
