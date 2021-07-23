using Characters;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Stance Copy (SELF) [Preset]",
        menuName = "Combat/Effects/Stance Copy")]
    public class SEffectStanceCopy : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            if(FailRandom(effectModifier)) return;
            var targetStance = target.AreasDataTracker.PositionStance;
            UtilsArea.ToggleStance(user,targetStance);

            user.Events.InvokeAreaChange();
        }
    }
}
