using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamsHolder : IOppositionTeamStructureRead<CombatTeam>
    {
        [ShowInInspector,TabGroup("Player Team")]
        public CombatTeam PlayerTeamType { get; internal set; }
        [ShowInInspector, TabGroup("Enemy Team")]
        public CombatTeam EnemyTeamType { get; internal set; }
    }
}
