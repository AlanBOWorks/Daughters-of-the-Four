using Characters;
using Skills;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Restore Stance Effect [Preset]",
        menuName = "Combat/Effects/Restore Stance")]
    public class SEffectRestoreStance : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float randomCheck = 1)
        {
            DoEffect(target, randomCheck);
        }


        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            if (FailRandom(randomCheck)) return;
            target.AreasDataTracker.ForceStateFinish();

            target.Events.InvokeAreaChange();
        }
    }
}
