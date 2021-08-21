using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "DAMAGE Static [OFFENSIVE Effect]",
        menuName = "Combat/Effects/DAMAGE Static")]
    public class SEffectDamageStatic : SEffectBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            float staticDamage = UtilsCombatStats.CalculateFinalStaticDamage(
                arguments.User.CombatStats,
                target.CombatStats,
                effectModifier);

            DoEffect(target, staticDamage);
        }


        public override void DoEffect(CombatingEntity target, float damage)
        {
            UtilsCombatStats.DoStaticDamage(target, damage);

#if UNITY_EDITOR
            Debug.Log($"STATIC Effect: {target.CharacterName} => {damage}");
#endif
        }
    }
}
