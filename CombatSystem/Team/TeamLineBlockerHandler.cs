using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class TeamLineBlockerHandler 
    {
        [ShowInInspector]
        private CombatEntity _currentGuarder;

        public bool IsGuarding() => _currentGuarder != null && _currentGuarder.CanBeTarget();
        public CombatEntity GetCurrentGuarder() => _currentGuarder;
        public void SetGuarder(CombatEntity guarder)
        {
            _currentGuarder = guarder;
        }


        public void OnEntityRequestSequence(CombatEntity entity)
        {
            if (entity == _currentGuarder) _currentGuarder = null;
        }

    }
}
