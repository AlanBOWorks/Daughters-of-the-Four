using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;

namespace _Team
{
    public class TeamTempoTicker : ICombatPreparationListener, ITempoTicker
    {
        [ShowInInspector]
        private TeamMemberDeathHandler _playerDeathHandler;
        [ShowInInspector]
        private TeamMemberDeathHandler _enemyDeathHandler;

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _playerDeathHandler = playerEntities.DeathHandler;
            _enemyDeathHandler = enemyEntities.DeathHandler;
        }

        public void TempoTick(float deltaVariation)
        {
            _playerDeathHandler.TempoTick(deltaVariation);
            _enemyDeathHandler.TempoTick(deltaVariation);
        }
    }
}
