using ___ProjectExclusive;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UCharacterOverTargetTooltip : UCharacterOverTooltipBase
    {
        [SerializeField] private Image targetIcon;

        public override void Injection(CombatingEntity entity, bool isPlayer)
        {
            base.Injection(entity, isPlayer);
            HandleTargetIcon(isPlayer);
        }

        private void HandleTargetIcon(bool isPlayer)
        {
            var colorHolder = GameThemeSingleton.ThemeColors.playerColors;
            Color targetColor = isPlayer 
                ? colorHolder.controllableColor 
                : colorHolder.enemyColor;

            targetIcon.color = targetColor;
        }

        protected override Vector3 GetUIPosition(UCharacterHolder holder)
        {
            Transform targetTransform = holder.targetTransform;
            if (targetTransform == null) targetTransform = holder.meshTransform;

            return targetTransform.position;
        }
    }
}
