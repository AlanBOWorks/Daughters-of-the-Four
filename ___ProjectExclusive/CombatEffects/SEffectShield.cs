using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "SHIELDS [Effect]",
        menuName = "Combat/Effects/Shields")]
    public class SEffectShield : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float shields = UtilsCombatStats.CalculateShieldsPower(user.CombatStats);

            DoEffect(target,shields);
        }


        public override void DoEffect(CombatingEntity target, float shields)
        {
            UtilsCombatStats.DoGiveShieldsTo(target,shields);
        }
    }
}
