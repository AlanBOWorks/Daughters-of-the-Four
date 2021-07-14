using Characters;
using UnityEngine;

namespace _Player
{
    public class UCharacterOverTargetTooltip : UCharacterOverTooltipBase
    {
        protected override Vector3 GetUIPosition(UCharacterHolder holder)
        {
            Transform targetTransform = holder.targetTransform;
            if (targetTransform == null) targetTransform = holder.meshTransform;

            return targetTransform.position;
        }
    }
}
