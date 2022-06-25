using System;
using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVanguardEffectsInfoHandler : MonoBehaviour, IOppositionTeamStructureRead<UVanguardEffectsTooltipWindowHandler>
    {
        [SerializeField,HorizontalGroup()] 
        private UVanguardEffectsTooltipWindowHandler playerEffectTooltipsHandler;
        [SerializeField, HorizontalGroup()] 
        private UVanguardEffectsTooltipWindowHandler enemyEffectTooltipsHandler;

        private void Awake()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            HandleWindowHandlerSubscriptions(playerEvents,playerEffectTooltipsHandler);

            var enemyEvents = EnemyCombatSingleton.EnemyEventsHolder;
            HandleWindowHandlerSubscriptions(enemyEvents,enemyEffectTooltipsHandler);

            void HandleWindowHandlerSubscriptions(ControllerCombatEventsHolder eventsHolder,
                ICombatEventListener handler)
            {
                eventsHolder.DiscriminationEventsHolder.Subscribe(handler);
            }
        }

        public UVanguardEffectsTooltipWindowHandler PlayerTeamType 
            => playerEffectTooltipsHandler;
        public UVanguardEffectsTooltipWindowHandler EnemyTeamType 
            => enemyEffectTooltipsHandler;
    }
}
