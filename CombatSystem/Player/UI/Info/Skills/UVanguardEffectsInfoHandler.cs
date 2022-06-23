using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team.VanguardEffects;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UVanguardEffectsInfoHandler : MonoBehaviour, 
        IVanguardEffectStructureBaseRead<UEffectsTooltipWindowHandler>,
        IVanguardEffectUsageListener
    {
        [SerializeField] private UEffectsTooltipWindowHandler revengeVanguardEffectWindow;
        [SerializeField] private UEffectsTooltipWindowHandler punishVanguardEffectWindow;
        [SerializeField] private bool isPlayer;

        public UEffectsTooltipWindowHandler VanguardRevengeType => revengeVanguardEffectWindow;
        public UEffectsTooltipWindowHandler VanguardPunishType => punishVanguardEffectWindow;


        public void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values)
        {
        }

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
        }

        public void OnVanguardEffectPerform(VanguardSkillUsageValues values)
        {
        }
    }
}
