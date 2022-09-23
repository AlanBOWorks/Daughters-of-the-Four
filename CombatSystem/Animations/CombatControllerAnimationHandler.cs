using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animations
{
    public sealed class CombatControllerAnimationHandler : ITempoEntityMainStatesListener, 
        ICombatStartListener, ICombatTerminationListener,
        ISkillUsageListener, IEffectUsageListener
    {

        public const float PerformToReceiveTimeOffset = .5f;
        public const float FromReceiveToFinishTimeOffset = .3f;
        public const float MaxAnimationDuration = PerformToReceiveTimeOffset + FromReceiveToFinishTimeOffset;

        private static ICombatEntityAnimator GetAnimator(CombatEntity entity) => entity.Body.GetAnimator();
        
        public void PerformActionAnimation(ISkill usedSkill, CombatEntity performer, CombatEntity target)
        {
            var animator = GetAnimator(performer);
            animator.PerformActionAnimation(usedSkill, in target);
        }

      
        private static void PerformReceiveAnimation(ISkill usedSkill, CombatEntity target, CombatEntity performer)
        {
            var targetAnimator = GetAnimator(target);
            targetAnimator.ReceiveActionAnimation(usedSkill, performer);
        }

        private static void PerformReceiveAnimation(IEffect effect, CombatEntity performer, CombatEntity target)
        {
            var targetAnimator = GetAnimator(target);
            targetAnimator.ReceiveActionAnimation(effect,performer);
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            var animator = GetAnimator(entity);
            animator.OnRequestSequenceAnimation();
        }
        

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
            var animator = GetAnimator(entity);
            animator.OnEndSequenceAnimation();
        }

        private const float IterationWait = .12f;
        public void DoInitialAnimations(CombatTeam team)
        {
            CombatCoroutinesTracker.StartCombatCoroutine(_IterationCall());
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

        [ShowInInspector]
        private CombatTeam _playerTeam;
        [ShowInInspector]
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

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatQuit()
        {
        }


        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            PerformActionAnimation(values.UsedSkill, values.Performer, values.Target);
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {

        }

        public void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            PerformReceiveAnimation(values.Effect,entities.Performer, entities.Target);
        }

        public void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
        }

        public void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
        }
    }
}
