using Characters;
using Skills;
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

        public override EnumSkills.StatDriven GetEffectStatDriven()
            => EnumSkills.StatDriven.Stance;

        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            
        }
    }
}
