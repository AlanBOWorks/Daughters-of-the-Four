using CombatEntity;
using CombatTeam;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __ProjectExclusive.Enemy
{
    public interface IEnemyCombatProvider : ITeamProvider
    {
        string GetCombatScenePath();
        bool IsEliteCombat();
        void InvokeCombatWithThisTeam();
    }
}
