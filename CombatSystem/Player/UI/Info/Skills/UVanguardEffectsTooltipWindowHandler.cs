using System;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UVanguardEffectsTooltipWindowHandler : UEffectTooltipWindowHandlerBasic<UEffectTooltipHolder>
    {
        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool poolHandler;
        [Serializable]
        private sealed class ToolTipWindowPool : DictionaryMonoObjectPool<CombatTeamControllerBase,UEffectTooltipHolder>
        {
            
        }

        protected override IObjectPoolBasic GetPool() => poolHandler;
        protected override IObjectPoolTracked<UEffectTooltipHolder> GetTrackedPool() => poolHandler;
    }
}
