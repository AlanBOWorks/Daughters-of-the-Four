using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "(Chance)DE N [Effect]",
        menuName = "Combat/Effects/DeBuff/Chance Type")]
    public class SEffectDeBuffChance : SEffectBase
    {
        [SerializeField] private SEffectBuffBase defuffMirror;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float debuffModifier = 1)
        {
            float userDebuffChance = user.CombatStats.DeBuffPower * Random.value;
            float targetDebuffChance = target.CombatStats.DeBuffReduction * Random.value;

            if(targetDebuffChance > userDebuffChance) return;
            DoEffect(target,debuffModifier);
        }

        public override void DoEffect(CombatingEntity target, float debuffModifier)
        {
            defuffMirror.DoEffect(target,debuffModifier);
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = "(Chance) DE" + defuffMirror.name;
            RenameAsset();
        }
    }
}
