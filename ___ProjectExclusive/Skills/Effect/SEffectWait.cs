using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Wait Effect [Preset]",
        menuName = "Combat/Effects/Waits")]
    public class SEffectWait : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.SetInitiative(target.CombatStats, effectModifier);
            UtilsCombatStats.SetActionAmount(target.CombatStats);
        }
    }
}
