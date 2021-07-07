using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Wait Effect [Preset]",
        menuName = "Combat/Effects/Waits")]
    public class SEffectWait : SEffectBase
    {
        public override EffectType GetEffectType() => EffectType.SelfOnly;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            UtilsSkill.SetInitiative(1 - effectModifier,target.CombatStats);
        }
    }
}
