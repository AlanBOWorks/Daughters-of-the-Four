using System;
using Animancer;
using Skills;
using UnityEngine;

namespace ___ProjectExclusive.Animators
{
    [CreateAssetMenu(fileName = "N - Transition Animations [Preset]", 
        menuName = "Animations/Combat/Transition Animations")]
    public class SCombatAnimationsTransitions : SCombatAnimationsTransitionsBase
    {
        [SerializeField] private ClipTransition idleAnimation;
        [SerializeField] private ClipTransition selfSupportAnimation;
        [SerializeField] private ClipTransition offensiveAnimation;
        [SerializeField] private ClipTransition supportAnimation;
        [SerializeField] private ClipTransition receiveSupportAnimation;
        [SerializeField] private ClipTransition receiveOffensiveAnimation;

        public override ITransition IdleAnimation => idleAnimation;
        public override ITransition SelfSupportAnimation => selfSupportAnimation;
        public override ITransition OffensiveAnimation => offensiveAnimation;
        public override ITransition SupportAnimation => supportAnimation;
        public override ITransition ReceiveSupportAnimation => receiveSupportAnimation;
        public override ITransition ReceiveOffensiveAnimation => receiveOffensiveAnimation;
        public override ITransition GetActionAnimationElement(CombatSkill skill)
        {
            var animationType = UtilsSkill.GetType(skill);
            switch (animationType)
            {
                case SSkillPreset.SkillType.SelfOnly:
                    return selfSupportAnimation;
                case SSkillPreset.SkillType.Offensive:
                    return offensiveAnimation;
                case SSkillPreset.SkillType.Support:
                    return SupportAnimation;
                default:
                    throw new ArgumentOutOfRangeException(
                        "Skill type is not supported for this animation Handler: " +
                        $"{animationType}");
            }
        }
    }
}
