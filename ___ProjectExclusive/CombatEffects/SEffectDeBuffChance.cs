using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "(Chance)DE N [Effect]",
        menuName = "Combat/Effects/DeBuff/Chance Type")]
    public class SEffectDeBuffChance : SEffectBase
    {
        [SerializeField] private SEffectBuffBase defuffMirror;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float debuffModifier = 1)
        {
            float userDebuffChance = arguments.UserStats.GetDeBuffPower() * Random.value;
            float targetDebuffChance = target.CombatStats.GetDeBuffReduction() * Random.value;

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
