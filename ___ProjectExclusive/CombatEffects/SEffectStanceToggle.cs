using System;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Stance Toggle - N [Preset]",
        menuName = "Combat/Effects/Stance Toggle")]
    public class SEffectStanceToggle : SEffectBase
    {
        [SerializeField] private TeamCombatData.Stance targetStance = TeamCombatData.Stance.Neutral;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float randomCheck = 1)
        {
            DoEffect(target, randomCheck);
        }

        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            if (FailRandom(randomCheck)) return;

            UtilsArea.ToggleStance(target, targetStance);
            target.Events.InvokeAreaChange();
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = $"Stance Toggle - {targetStance.ToString().ToUpper()} [Preset]";
            RenameAsset();
        }
    }
}
