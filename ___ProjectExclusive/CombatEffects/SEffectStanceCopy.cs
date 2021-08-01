using Characters;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Stance Copy (SELF) [Preset]",
        menuName = "Combat/Effects/Stance Copy")]
    public class SEffectStanceCopy : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float randomCheck = 1)
        {
            if(FailRandom(randomCheck)) return;
            var targetStance = target.AreasDataTracker.GetCurrentPositionState();
            UtilsArea.ToggleStance(user,targetStance);

            user.Events.InvokeAreaChange();
        }

        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            
        }
    }
}
