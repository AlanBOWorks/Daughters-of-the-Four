using CombatEntity;
using CombatTeam;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __ProjectExclusive.Enemy
{
    public interface IEnemyCombatProvider : ITeamProvider
    {
        string GetCombatScenePath();
        void InvokeCombatWithThisTeam();
    }
}
