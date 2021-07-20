using Characters;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(fileName = "Stance Copy - N [Preset]",
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
