using System;
using Characters;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(fileName = "Damage Effect - N [Preset]",
        menuName = "Combat/Effects/Damage")]
    public class SEffectDamage : SEffectBase
    {
        public override EffectType GetEffectType() => EffectType.Offensive;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float damage = user.CombatStats.AttackPower;
            damage *= effectModifier;

#if UNITY_EDITOR
            Debug.Log($"Damage Effect: {target.CharacterName} => {damage}");
#endif

            UtilsSkill.DoDamage(damage,target.CombatStats);
            target.Listeners.UpdateTemporalStats();
        }

    }
}
