using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "DAMAGE Direct [OFFENSIVE Effect]",
        menuName = "Combat/Effects/DAMAGE Direct")]
    public class SEffectDamage : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float damage = UtilsCombatStats.CalculateFinalDamage(
                user.CombatStats, 
                target.CombatStats,
                effectModifier);

            DoEffect(target,damage);
            target.Events.OnHitEvent.OnHit(damage);
        }

        public override EnumSkills.StatDriven GetEffectStatDriven()
            => EnumSkills.StatDriven.Health;

        public override void DoEffect(CombatingEntity target, float damage)
        {
            UtilsCombatStats.DoDamageTo(target, damage);

#if UNITY_EDITOR
            Debug.Log($"Damage Effect: {target.CharacterName} => {damage}");
#endif
        }
    }
}
