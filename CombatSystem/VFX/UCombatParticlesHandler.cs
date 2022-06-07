using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Player;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.VFX
{
    public class UCombatParticlesHandler : MonoBehaviour, IOppositionTeamStructureRead<ParticlesSpawnHandler>
    {
#if UNITY_EDITOR
        [Title("Params")]
        [SerializeField] private bool disableInjectionForDebugging = false; 
#endif

        [Title("Player")]
        [SerializeField, HorizontalGroup()] 
        private ParticlesSpawnHandler playerParticlesHolder;
        [Title("Enemy")]
        [SerializeField,HorizontalGroup()]
        private ParticlesSpawnHandler enemyParticlesHolder;



        public ParticlesSpawnHandler PlayerTeamType => playerParticlesHolder;
        public ParticlesSpawnHandler EnemyTeamType => enemyParticlesHolder;

        private static CombatEntityEventsHolder GetPlayerEventsHolder() =>
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder;

        private static CombatEntityEventsHolder GetEnemyEventsHolder() =>
            EnemyCombatSingleton.EnemyEventsHolder.DiscriminationEventsHolder;

        private void Awake()
        {
#if UNITY_EDITOR
            if (disableInjectionForDebugging) return;
#endif

            Subscription(GetPlayerEventsHolder(),playerParticlesHolder);
            Subscription(GetEnemyEventsHolder(), enemyParticlesHolder);
        }
        private void OnDestroy()
        {
            UnSubscription(GetPlayerEventsHolder(), playerParticlesHolder);
            UnSubscription(GetEnemyEventsHolder(), enemyParticlesHolder);
        }


        private static void Subscription(CombatEntityEventsHolder eventsHolder, ICombatEventListener handler)
        {
            eventsHolder.Subscribe(handler);
        }
        public static void UnSubscription(CombatEntityEventsHolder eventsHolder, ICombatEventListener handler)
        {
            eventsHolder.UnSubscribe(handler);
        }
}
}
