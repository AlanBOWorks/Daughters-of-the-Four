﻿using System;
using _CombatSystem;
using Characters;
using UnityEditor;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Stance Toggle - N [Preset]",
        menuName = "Combat/Effects/Stance Toggle")]
    public class SEffectStanceToggle : SEffectBase
    {
        [SerializeField] private TeamCombatData.Stance targetStance = TeamCombatData.Stance.Neutral;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            if(FailRandom(effectModifier)) return;

            UtilsArea.ToggleStance(target,targetStance);
            target.Events.InvokeAreaChange();
        }



        private void OnValidate()
        {
            name = $"Stance Toggle - {targetStance.ToString().ToUpper()} [Preset]";
            RenameAsset();
        }
    }
}