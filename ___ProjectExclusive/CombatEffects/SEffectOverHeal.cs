using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "OverHEAL [Effect]",
        menuName = "Combat/Effects/Over Heal")]
    public class SEffectOverHeal : SEffectHeal
    {
        public override void DoEffect(CombatingEntity target, float overHealAmount)
        {
            UtilsCombatStats.DoOverHealTo(target, overHealAmount);
        }
    }
}
