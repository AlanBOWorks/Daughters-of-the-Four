using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;

namespace _Team
{
    public class TeamTempoTicker : ICombatPreparationListener, ITempoTicker
    {
        [ShowInInspector]
        private MemberKnockOutHandler _playerKnockOutHandler;
        [ShowInInspector]
        private MemberKnockOutHandler _enemyKnockOutHandler;

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _playerKnockOutHandler = playerEntities.knockOutHandler;
            _enemyKnockOutHandler = enemyEntities.knockOutHandler;
        }

        public void TempoTick(float deltaVariation)
        {
            _playerKnockOutHandler.TempoTick(deltaVariation);
            _enemyKnockOutHandler.TempoTick(deltaVariation);
        }
    }
}
