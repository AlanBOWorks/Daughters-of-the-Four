using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using MEC;
using UnityEngine;

namespace CombatSystem.Animations
{
    public sealed class CombatControllerAnimationHandler : ITempoEntityStatesListener, ICombatStatesListener
    {
        private static ICombatEntityAnimator GetAnimator(in CombatEntity entity) => entity.Body.GetAnimator();

        public void PerformActionAnimation(ICombatSkill usedSkill, CombatEntity performer, CombatEntity target)
        {
            var animator = GetAnimator(in performer);
            animator.PerformActionAnimation(usedSkill, in target);
        }


        public void PerformReceiveAnimations(ICombatSkill usedSkill, CombatEntity performer)
        {
            var interactions = CombatSystemSingleton.SkillTargetingHandler.GetInteractions();
            foreach (var entity in interactions)
            {
                PerformReceiveAnimation(usedSkill, entity, performer);
            }
        }
        private void PerformReceiveAnimation(ICombatSkill usedSkill, CombatEntity target, CombatEntity performer)
        {
            var targetAnimator = GetAnimator(in target);
            targetAnimator.ReceiveActionAnimation(usedSkill, performer);
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            var animator = GetAnimator(in entity);
            animator.OnRequestSequenceAnimation();
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
           
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            var animator = GetAnimator(in entity);
            animator.OnEndSequenceAnimation();
        }

        private const float IterationWait = .12f;
        public void DoInitialAnimations(CombatTeam team)
        {
            var coroutineLayer = CombatSystemSingleton.CombatCoroutineLayer;

            Timing.RunCoroutine(_IterationCall(), coroutineLayer);
            IEnumerator<float> _IterationCall()
            {
                foreach (var entity in team.GetAllMembers())
                {
                    yield return Timing.WaitForSeconds(IterationWait);
                    CallInitialAnimation(entity);
                }
            }
        }

        private static void CallInitialAnimation(CombatEntity entity)
        {
            var body = entity.Body;
            body?.GetAnimator().PerformInitialCombatAnimation();
        }

        private CombatTeam _playerTeam;
        private CombatTeam _enemyTeam;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }
        public void OnCombatStart()
        {
            DoInitialAnimations(_playerTeam);
            DoInitialAnimations(_enemyTeam);
        }

        public void OnCombatEnd()
        {
            _playerTeam = null;
            _enemyTeam = null;
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }


    }
}
