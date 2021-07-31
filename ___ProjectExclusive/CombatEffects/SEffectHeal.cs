using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Heal Effect [Effect]",
        menuName = "Combat/Effects/Heal")]
    public class SEffectHeal : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float heal = user.CombatStats.HealPower;
            heal *= effectModifier;

            DoEffect(target,heal);
        }

        public override void DoEffect(CombatingEntity target, float heal)
        {
            UtilsCombatStats.DoHealTo(target, heal);
#if UNITY_EDITOR
            Debug.Log($"Heal Effect: {target.CharacterName} => {heal}");
#endif
        }
    }
}
 