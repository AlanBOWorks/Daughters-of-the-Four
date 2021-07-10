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
        public static ProvisionalCharacterAnimator ProvisionalAnimator = new ProvisionalCharacterAnimator();

        public ProvisionalCharacterAnimator()
        {}

        public IEnumerator<float> _DoAnimation(CombatingEntity user, List<CombatingEntity> targets, CombatSkill skill)
        {
            MoveUser();
            foreach (CombatingEntity target in targets)
            {
                MoveTarget(target);
            }
            yield return Timing.WaitForSeconds(2);

            void MoveUser()
            {
                user.Holder.transform.DOPunchPosition(Vector3.up, 1,3);
            }

            void MoveTarget(CombatingEntity target)
            {
                target.CombatAnimator.ReceiveAction(user, target, skill);
            }

        }

        public void ReceiveAction(CombatingEntity actor, CombatingEntity target, CombatSkill skill)
        {
            target.Holder.transform.DOPunchPosition(Vector3.right , 1.4f);
            Debug.Log("Receiving skill: "+ target.CharacterName);
        }
    }

    public interface ICharacterCombatAnimator
    {
        IEnumerator<float> _DoAnimation(CombatingEntity user, List<CombatingEntity> targets,CombatSkill skill);
        void ReceiveAction(CombatingEntity actor, CombatingEntity receiver, CombatSkill skill);
    }
}
