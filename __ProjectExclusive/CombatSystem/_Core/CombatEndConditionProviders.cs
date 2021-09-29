using CombatEntity;
using CombatTeam;
using UnityEngine;

namespace CombatSystem
{
    public interface ICombatEndConditionProvider
    {
        bool IsCombatFinish();
        CombatingTeam GetWinningTeam(); // This exits so in special conditions the wining team can be checked
    }


    public class GenericWinCondition : ICombatEndConditionProvider
    {
        public bool IsCombatFinish()
        {
            var playerTeam = CombatSystemSingleton.VolatilePlayerTeam;
            var enemyTeam = CombatSystemSingleton.VolatileEnemyTeam;
            return !enemyTeam.HasLivingEntities() || !playerTeam.HasLivingEntities();
        }

        public CombatingTeam GetWinningTeam()
        {
            var playerTeam = CombatSystemSingleton.VolatilePlayerTeam;
            var enemyTeam = CombatSystemSingleton.VolatileEnemyTeam;


            return (playerTeam.HasLivingEntities()) ? playerTeam : enemyTeam;
        }

    }
}
