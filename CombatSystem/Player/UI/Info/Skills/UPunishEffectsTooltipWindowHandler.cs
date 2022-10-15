using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UPunishEffectsTooltipWindowHandler : MonoBehaviour
    {
        [SerializeField] 
        private UAllEffectTooltipsHandler punishEffectHandler;


        public void AddVanguardEffects(IVanguardSkill skill, CombatEntity performer)
        {
            var effects = skill.GetPunishEffects();
            punishEffectHandler.HandleEffects(effects,skill,performer);
        }

        public void ReleaseElements()
        {
            punishEffectHandler.Clear();
        }
    }
}
