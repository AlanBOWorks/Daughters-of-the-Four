using System;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatSystem.Enemy;
using MEC;
using Sirenix.OdinInspector;

namespace CombatSystem
{
    public sealed class EntityActionRequestHandler : ICombatDisruptionListener
    {
        public EntityActionRequestHandler()
        {
            ActionsQueue = new Queue<IEnumerator<float>>();
            _skillValues = new SkillValuesHolders();
        }

        [ShowInInspector]
        private readonly SkillValuesHolders _skillValues;

        public readonly Queue<IEnumerator<float>> ActionsQueue;
        public static IEntitySkillRequestHandler PlayerForcedEntitySkillRequestHandler;
        public static IEntitySkillRequestHandler EnemyForcedEntitySkillRequestHandler;
        
        public void OnCombatPause()
        {

        }

        public void OnCombatResume()
        {
        }

        public void OnCombatExit()
        {
            EnemyForcedEntitySkillRequestHandler = null;
            PlayerForcedEntitySkillRequestHandler = null;
        }

        private CoroutineHandle _priorityActionHandle;
        public void RequestPriorityAction(IEnumerator<float> resumeAfterAction)
        {
            if (_priorityActionHandle.IsRunning)
                throw new NotSupportedException($"[{typeof(EntityActionRequestHandler)}] can't handle two pausing coroutines at the same time;" +
                                                "Let the current pausing coroutine finish before requesting another pause.");
            _priorityActionHandle = Timing.RunCoroutine(resumeAfterAction);
            Timing.WaitForOtherHandles(_currentHandle,_priorityActionHandle);
        }

        private static IEntitySkillRequestHandler GetTeamTempoController(CombatingEntity entity)
        {
            return CombatSystemSingleton.VolatilePlayerTeam.Contains(entity) //Is Players?

                ? GetPlayerEntityRequestHandler()
                : GetEnemyEntityRequestHandler();
            IEntitySkillRequestHandler GetPlayerEntityRequestHandler()
            {
                return PlayerForcedEntitySkillRequestHandler ?? PlayerCombatSingleton.EntitySkillRequestHandler;
            }
            IEntitySkillRequestHandler GetEnemyEntityRequestHandler()
            {
                return EnemyForcedEntitySkillRequestHandler ?? EnemyCombatSingleton.EntitySkillRequestHandler;
            }
        }

        private CoroutineHandle _currentHandle;
        public IEnumerator<float> _RequestEntityActions(CombatingEntity currentActingEntity)
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            _currentHandle = Timing.CurrentCoroutine;

            yield return Timing.WaitForOneFrame; //TODO request start actions animation; ( UI shows the entity turns on screen)

            _skillValues.Inject(currentActingEntity);
            eventsHolder.OnFirstAction(currentActingEntity);
            var requestHandler = GetTeamTempoController(currentActingEntity);


            yield return Timing.WaitForOneFrame;

            while (currentActingEntity.CanAct())
            {
                _skillValues.OnActionClear();
                _currentHandle = Timing.RunCoroutine(requestHandler.HandleRequestAction(_skillValues));
                yield return Timing.WaitUntilTrue(_skillValues.IsValid);
                _currentHandle = Timing.RunCoroutine(_PerformSkill());
                yield return Timing.WaitUntilDone(_currentHandle);
                eventsHolder.OnFinishAction(currentActingEntity);
                // Death events are meant to be the last events to be send (since some previous events could prevent death
                // conditions and this could create false positives)
                CombatSystemSingleton.EntityDeathHandler.HandleDeaths();
            }
            eventsHolder.OnFinishAllActions(currentActingEntity);
            _skillValues.Clear();
        }

        private const float SpaceBetweenAnimations = .12f;
        private const float MaxWaitBetweenAnimations = 1f;
        public IEnumerator<float> _PerformSkill()
        {
            var values = _skillValues;
            values.Target.GuardHandler.VariateTarget(values);
            values.RollForCritical();

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);
            PerformSkill();

            var performer = values.Performer;
            var entityHolder = performer.InstantiatedHolder;
            var animationHandler = entityHolder.AnimationHandler;
            var eventHolder = CombatSystemSingleton.EventsHolder;
            eventHolder.OnBeforeAnimation(values);

            if (entityHolder != null && animationHandler != null)
            {
                // Todo check for special animation and do wait below
                // yield return Timing.WaitUntilDone(animationHandler._DoPerformSkillAnimation(values));
                animationHandler.DoPerformSkillAnimation(values);
                yield return Timing.WaitForSeconds(MaxWaitBetweenAnimations);
            }
            eventHolder.OnAnimationHaltFinish(values);

            yield return Timing.WaitForOneFrame;

            yield return Timing.WaitForSeconds(SpaceBetweenAnimations);

            void PerformSkill()
            {
                var skill = values.UsedSkill;
                var mainEffect = skill.GetMainEffect();
                var effects = skill.GetEffects();

                // Before effects because OnSkillUse could have buff/reaction effects that mitigates/amplified effects
                var eventsHolder = CombatSystemSingleton.EventsHolder;
                eventsHolder.OnSkillUse(values);
                skill.OnUseIncreaseCost();
                eventsHolder.OnSkillCostIncreases(values);

                if (!skill.IsMainEffectAfterListEffects)
                    mainEffect.DoActionEffect(values);

                foreach (EffectParameter effectParameter in effects)
                {
                    effectParameter.DoActionEffect(values);
                }
                if (skill.IsMainEffectAfterListEffects)
                    mainEffect.DoActionEffect(values);
            }
        }

    }

    public interface IEntitySkillRequestHandler
    {
        // These parameters are for giving the player the possibility of choosing various action in one go
        // even in between animations; For AI this is not necessary since the AI can just decide in the fly.
        // More advance AI could inject a sequence of action in the queue so it feels more intelligent
        IEnumerator<float> HandleRequestAction(SkillValuesHolders skillValues);

    }
}
