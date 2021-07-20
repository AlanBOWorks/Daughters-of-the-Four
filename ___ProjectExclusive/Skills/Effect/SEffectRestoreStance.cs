using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Restore Stance Effect - N [Preset]",
        menuName = "Combat/Effects/Restore Stance")]
    public class SEffectRestoreStance : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            if(FailRandom(effectModifier)) return;
            target.AreasDataTracker.ForceStateFinish();

            target.Events.InvokeAreaChange();
        }
    }
}
