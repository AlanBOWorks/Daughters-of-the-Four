using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace CombatConditions
{
    [CreateAssetMenu(fileName = "TEMPORAL Stats - N [Condition]",
        menuName = "Combat/Conditions/Temporal Stat Check")]
    public class STemporalStatsCondition : SCondition
    {
        [SerializeField] private EnumStats.TemporalStats checkStats 
            = EnumStats.TemporalStats.Harmony;

        public override bool CanApply(CombatingEntity target, float checkValue)
        {
            float statValue = UtilsEnumStats.GetStat(target.CombatStats, checkStats);

            return checkValue < statValue;
        }

        [Button(ButtonSizes.Large), GUIColor(.3f, .6f, 1f)]
        private void UpdateAssetName()
        {
            name = $"TEMPORAL Stats - {checkStats.ToString().ToUpper()} [Condition]";

            UtilsGame.UpdateAssetName(this);
        }
    }
}
