using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animator
{
    [CreateAssetMenu(fileName = "N [Combat Idle Animations]",
        menuName = "Combat/Animations/Idle")]
    public class SCombatIdleAnimationsHolder : SCombatStandByAnimationsHolder
    {
        [Title("Initial")]
        [SerializeField] private AnimationClip initialAnimation;
        [Title("Idle")]
        [SerializeField] private AnimationClip defaultClip;

        public override AnimationClip GetStartingCombatAnimation()
            => initialAnimation;

        public override AnimationClip GetIdleAnimation(CombatingEntity user)
            => defaultClip;
    }
}
