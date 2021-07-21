using System;
using Characters;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(fileName = "Damage Effect - N [Preset]",
        menuName = "Combat/Effects/Damage")]
    public class SEffectDamage : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float damage = UtilsCombatStats.CalculateFinalDamage(
                user.CombatStats, 
                target.CombatStats,
                effectModifier);

#if UNITY_EDITOR
            Debug.Log($"Damage Effect: {target.CharacterName} => {damage}");
#endif

            UtilsCombatStats.DoDamageTo(target.CombatStats, damage);
            target.Events.InvokeTemporalStatChange();
        }

    }
}
