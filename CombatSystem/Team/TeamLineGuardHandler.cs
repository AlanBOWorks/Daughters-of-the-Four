using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class TeamLineGuardHandler 
    {
        [ShowInInspector]
        private CombatEntity _currentGuarder;

        public bool CanGuard() => _currentGuarder != null && _currentGuarder.CanBeTarget();
        public CombatEntity GetCurrentGuarder() => _currentGuarder;
        public void SetGuarder(in CombatEntity guarder)
        {
            _currentGuarder = guarder;
        }


        public void OnEntityRequestSequence(in CombatEntity entity)
        {
            if (entity == _currentGuarder) _currentGuarder = null;
        }

    }
}
