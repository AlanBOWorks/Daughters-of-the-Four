using CombatSystem.Skills;
using DG.Tweening;
using UnityEngine;

namespace CombatSystem.Entity
{
    public abstract class UCombatEntityAnimator : MonoBehaviour, ICombatEntityAnimator
    {
        public abstract void OnRequestSequenceAnimation();
        public abstract void PerformActionAnimation(in CombatSkill skill, in CombatEntity onTarget);
        public abstract void ReceiveActionAnimation(in CombatSkill fromSkill, in CombatEntity fromPerformer);
        public abstract void OnEndSequenceAnimation();
    }

    public sealed class ProvisionalCombatAnimator : ICombatEntityAnimator
    {
        internal void Injection(in Transform body)
        {
            _body = body;
        }

        private Transform _body;

        private const float AnimationDuration = .4f;
        public void OnRequestSequenceAnimation()
        {
            _body.DOPunchPosition(Vector3.up, AnimationDuration, 4);
        }

        public void PerformActionAnimation(in CombatSkill skill, in CombatEntity onTarget)
        {
            _body.DOPunchPosition(Vector3.forward, AnimationDuration, 4);
        }

        public void ReceiveActionAnimation(in CombatSkill fromSkill, in CombatEntity fromPerformer)
        {
            _body.DOPunchScale(Vector3.one * .1f, AnimationDuration);
        }

        public void OnEndSequenceAnimation()
        {
            _body.DOPunchScale(Vector3.one * -0.1f, AnimationDuration);
        }
    }

    public interface ICombatEntityAnimator
    {
        void OnRequestSequenceAnimation();
        void PerformActionAnimation(in CombatSkill skill, in CombatEntity onTarget);
        void ReceiveActionAnimation(in CombatSkill fromSkill, in CombatEntity fromPerformer);
        void OnEndSequenceAnimation();
    }
}
