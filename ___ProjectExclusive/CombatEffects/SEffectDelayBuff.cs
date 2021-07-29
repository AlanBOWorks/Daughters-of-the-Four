using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "N - [Delay Buff Effect]",
        menuName = "Combat/Effects/Delay Buff")]
    public class SEffectDelayBuff : SEffectBase
    {
        [SerializeField] private SDelayBuffPreset buff = null;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            target.DelayBuffHandler.EnqueueBuff(buff);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            target.DelayBuffHandler.EnqueueBuff(buff);
        }

        private const string DelayEffectPrefix = " - [Delay Buff Effect]";
        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = buff.SkillName + DelayEffectPrefix;
            base.RenameAsset();
        }
    }
}
