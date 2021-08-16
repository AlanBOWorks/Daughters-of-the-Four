using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Shields Effect [Effect]",
        menuName = "Combat/Effects/Shields")]
    public class SEffectShield : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float shields = user.CombatStats.HealPower;
            shields *= .5f; //By design shields are half as powerful than heals

            DoEffect(target,shields);
        }

        public override EnumSkills.StatDriven GetEffectStatDriven()
            => EnumSkills.StatDriven.Health;

        public override void DoEffect(CombatingEntity target, float shields)
        {
            UtilsCombatStats.DoGiveShieldsTo(target,shields);
        }
    }
}
