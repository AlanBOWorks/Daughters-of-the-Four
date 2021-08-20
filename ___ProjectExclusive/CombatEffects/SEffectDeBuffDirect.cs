using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "(Direct)DE N [Effect]",
        menuName = "Combat/Effects/DeBuff/Direct Type")]
    public class SEffectDeBuffDirect : SEffectBase
    {
        [SerializeField] private SEffectBuffBase defuffMirror;


        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            float userDebuff = arguments.UserStats.DeBuffPower * effectModifier;
            float targetDebuffResist = target.CombatStats.DeBuffReduction;

            float finalModifier = userDebuff - targetDebuffResist;
            if(finalModifier <= 0) return;

            DoEffect(target,finalModifier);
        }

        public override void DoEffect(CombatingEntity target, float debuffModifier)
        {
            defuffMirror.DoEffect(target,debuffModifier);
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = "(Direct) DE" + defuffMirror.name;
            RenameAsset();
        }
    }
}
