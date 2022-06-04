using System;
using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Player;
using CombatSystem.Player.UI;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.VFX
{
    public class UCombatParticlesHandler : MonoBehaviour, IOppositionTeamStructureRead<ParticlesSpawnHandler>
    {
        [Title("Params")]
        [SerializeField] private bool doInjection;
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
            if(!doInjection) return;

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
