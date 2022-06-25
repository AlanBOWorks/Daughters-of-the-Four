using System;
using CombatSystem._Core;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using CombatSystem.Team.VanguardEffects;
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


        public UVanguardEffectsTooltipWindowHandler PlayerTeamType => playerEffectTooltipsHandler;
        public UVanguardEffectsTooltipWindowHandler EnemyTeamType => enemyEffectTooltipsHandler;
    }
}
