using System;
using Characters;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Guarding - N [Effect]",
        menuName = "Combat/Effects/Guarding")]
    public class SEffectGuard : SEffectBase
    {
        [SerializeField, Tooltip("If is requesting a guarding from target instead of giving guard")] 
        private bool isProtectMe = false;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            var userGuarding = user.Guarding;
            if (isProtectMe)
            {
                userGuarding.ProtectMe(target,effectModifier);
            }
            else
            {
                userGuarding.GuardTarget(target,effectModifier);
            }
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
#if UNITY_EDITOR
            throw new AccessViolationException("Guarding requires two [CombatEntity]s. This invoke could be " +
                                               "because this effect was called from a passive (or an opening passive)");
#endif
        }
    }
}
