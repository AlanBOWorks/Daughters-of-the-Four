using CombatTeam;
using UnityEngine;

namespace CombatSystem.PositionHandlers
{
    public class UPositionProviderSimple : UPositionProviderBase
    {
        [SerializeField] private TeamRoleTransforms playerTransforms = new TeamRoleTransforms();
        [SerializeField] private TeamRoleTransforms enemyTransforms = new TeamRoleTransforms();

        public override ITeamRoleStructureRead<Transform> GetPlayerTeam() => playerTransforms;
        public override ITeamRoleStructureRead<Transform> GetEnemyTeam() => enemyTransforms;
    }
}
