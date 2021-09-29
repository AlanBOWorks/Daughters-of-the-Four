using CombatEntity;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Enemy
{
    public interface IEnemyCombatProvider : ITeamProvider
    {
        void InvokeCombatWithThisTeam();
    }
}
