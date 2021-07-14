using System;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UCharacterOverFeetTooltip : UCharacterOverTooltipBase
    {
        protected override Vector3 GetUIPosition(UCharacterHolder holder)
        {
            return holder.meshTransform.position;
        }
    }

}
