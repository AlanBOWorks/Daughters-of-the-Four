using System;
using CombatSystem.Animations;
using CombatSystem.Skills;
using DG.Tweening;
using UnityEngine;

namespace CombatSystem.Entity
{
    public abstract class UCombatEntityAnimator : MonoBehaviour, ICombatEntityAnimator
    {
        protected CombatEntity Entity;


        public void Injection(in CombatEntity user)
        {
            Entity = user;
        }

        public abstract void PerformInitialCombatAnimation();
        public abstract void PerformEndCombatAnimation();


        public abstract void OnRequestSequenceAnimation();
        public abstract void OnEndSequenceAnimation();
        protected abstract void DoPerformActionAnimation(in EnumsSkill.Archetype type);
        protected abstract void DoReceiveActionAnimation(in EnumsSkill.Archetype type);


        public void PerformActionAnimation(in CombatSkill skill, in CombatEntity onTarget)
        {
            var type = skill.Archetype;
            DoPerformActionAnimation(in type);
        }



        public void ReceiveActionAnimation(in CombatSkill fromSkill, in CombatEntity fromPerformer)
        {
            var type = UtilsTarget.GetReceiveSkillType(in fromSkill, in fromPerformer, Entity);
            DoReceiveActionAnimation(in type);
        }
    }

    public sealed class ProvisionalCombatAnimator : ICombatEntityAnimator
    {
        public void Injection(in CombatEntity user)
        {
        }

        public void PerformInitialCombatAnimation()
        {
        }

        public void PerformEndCombatAnimation()
        {
        }


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
}
