using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Wait Effect [Preset]",
        menuName = "Combat/Effects/Waits")]
    public class SEffectWait : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            DoEffect(target,effectModifier);
        }

        public override EnumSkills.StatDriven GetEffectStatDriven()
            => EnumSkills.StatDriven.Tempo;

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.SetInitiative(target.CombatStats, effectModifier);
            UtilsCombatStats.SetActionAmount(target.CombatStats);
        }
    }
}
