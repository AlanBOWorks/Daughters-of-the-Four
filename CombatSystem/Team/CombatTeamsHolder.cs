using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamsHolder : IOppositionTeamStructureRead<CombatTeam>
    {
        [ShowInInspector,HorizontalGroup()]
        public CombatTeam PlayerTeamType { get; internal set; }
        [ShowInInspector,HorizontalGroup()]
        public CombatTeam EnemyTeamType { get; internal set; }
    }
}
