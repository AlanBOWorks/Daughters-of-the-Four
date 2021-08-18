using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Position Toggle - N [Preset]",
        menuName = "Combat/Effects/Position Toggle")]
    public class SEffectPositionToggle : SEffectBase
    {
        [SerializeField] private CharacterArchetypes.FieldPosition targetPosition;
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            DoEffect(target,effectModifier);
        }
        

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsArea.TogglePosition(target,targetPosition);
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = $"POSITION Toggle - {targetPosition.ToString()} [Preset]";
            RenameAsset();
        }
    }
}
