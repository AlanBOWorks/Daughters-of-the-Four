using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Heal [Effect]",
        menuName = "Combat/Effects/Heal")]
    public class SEffectHeal : SEffectBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            float heal = arguments.UserStats.HealPower;
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
 