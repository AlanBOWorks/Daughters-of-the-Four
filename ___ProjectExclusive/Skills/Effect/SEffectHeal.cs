using System;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Heal Effect - N [Preset]",
        menuName = "Combat/Effects/Heal")]
    public class SEffectHeal : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float heal = user.CombatStats.HealPower;
            heal *= effectModifier;

#if UNITY_EDITOR
            Debug.Log($"Heal Effect: {target.CharacterName} => {heal}");
#endif


            UtilsCombatStats.DoHealTo(target.CombatStats,heal);
            target.Events.InvokeTemporalStatChange();
        }
    }
}
 