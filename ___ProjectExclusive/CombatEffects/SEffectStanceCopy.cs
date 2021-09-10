using Characters;
using Skills;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Stance Copy (SELF) [Preset]",
        menuName = "Combat/Effects/Stance Copy")]
    public class SEffectStanceCopy : SEffectBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float randomCheck = 1)
        {
            if(FailRandom(randomCheck)) return;
            var user = arguments.User;
            var targetStance = target.AreasDataTracker.GetCurrentPositionState();
            UtilsArea.ToggleStance(user,targetStance);
        }


        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            
        }
    }
}
