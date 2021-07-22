using System.Collections.Generic;
using DG.Tweening;
using MEC;
using Skills;
using UnityEngine;

namespace Characters
{

    public class CharacterAnimatorHandler 
    {
        public static void DoReactAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            foreach (CombatingEntity target in targets)
            {
                DoReaction(target);
            }


            void DoReaction(CombatingEntity target)
            {
                if (target == user) return;
                if (skill.GetMainType() != SSkillPreset.SkillType.Offensive)
                {
                    target.CombatAnimator.ReceiveSupport(user, target, skill);
                }
                else
                    target.CombatAnimator.ReceiveAttack(user, target, skill);
            }
        }
    }

    /// <summary>
    /// Used when a character doesn't have an Animator; It has a provisional and simple
    /// transform movement
    /// </summary>
    public class ProvisionalCharacterAnimator : ICharacterCombatAnimator
    {
        //TODO make a true Provisional Animator for Skip animations (with Idle animations)
        public static ProvisionalCharacterAnimator ProvisionalAnimator = new ProvisionalCharacterAnimator();


        public void DoInitialAnimation()
        {
            
        }

        public IEnumerator<float> _DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            ProvisionalAnimation();
            CharacterAnimatorHandler.DoReactAnimation(user,targets,skill);
            yield return Timing.WaitForSeconds(2);

            void ProvisionalAnimation()
            {
                user.Holder.transform.DOPunchPosition(Vector3.up, 1,3);
            }
        }

        public void ReceiveSupport(CombatingEntity actor, CombatingEntity target, CombatSkill skill)
        {
            target.Holder.transform.DOPunchPosition(Vector3.right , 1.4f);
        }
        public void ReceiveAttack(CombatingEntity actor, CombatingEntity target, CombatSkill skill)
        {
            target.Holder.transform.DOPunchPosition(Vector3.forward, 1.4f);
        }
    }

    public interface ICharacterCombatAnimator
    {
        void DoInitialAnimation();
        IEnumerator<float> _DoAnimation(CombatingEntity user, List<CombatingEntity> targets,CombatSkill skill);
        void ReceiveSupport(CombatingEntity actor, CombatingEntity receiver, CombatSkill skill);
        void ReceiveAttack(CombatingEntity actor, CombatingEntity receiver, CombatSkill skill);
    }
}
