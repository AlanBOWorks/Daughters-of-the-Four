using System.Collections.Generic;
using DG.Tweening;
using MEC;
using Skills;
using UnityEngine;

namespace Characters
{

    public class CharacterAnimatorHandler 
    {
       
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

        private CoroutineHandle _currentAnimationHandle;

        
        public CoroutineHandle DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            Timing.KillCoroutines(_currentAnimationHandle);
            _currentAnimationHandle = Timing.RunCoroutine(_DoProvisionalAction());

            return _currentAnimationHandle;

            IEnumerator<float> _DoProvisionalAction()
            {
                user.Holder.transform.DOPunchPosition(Vector3.up, 1, 3);
                yield return Timing.WaitForSeconds(2);
            }
        }

        public CoroutineHandle ReceiveAction(CombatingEntity actor, CombatingEntity receiver, bool isSupport)
        {
            Timing.KillCoroutines(_currentAnimationHandle);
            _currentAnimationHandle = Timing.RunCoroutine(_DoProvisionalAnimation());

            return _currentAnimationHandle;

            IEnumerator<float> _DoProvisionalAnimation()
            {
                if (isSupport)
                {
                    ReceiveSupport(receiver);
                }
                else
                {
                    ReceiveAttack(receiver);
                }

                yield return Timing.WaitForSeconds(ReceiveActionDuration);
            }
        }

        private const float ReceiveActionDuration = 1.4f;
        public void ReceiveSupport(CombatingEntity target)
        {
            target.Holder.transform.DOPunchPosition(Vector3.right , 1.4f);
        }
        public void ReceiveAttack( CombatingEntity target)
        {
            target.Holder.transform.DOPunchPosition(Vector3.forward, 1.4f);
        }
    }

    public interface ICharacterCombatAnimator
    {
        void DoInitialAnimation();
        CoroutineHandle DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill);
        CoroutineHandle ReceiveAction(CombatingEntity actor, CombatingEntity receiver, bool isSupport);
    }
}
