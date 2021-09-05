using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using Random = UnityEngine.Random;

namespace Skills
{
    /// <summary>
    /// Perform the skill with all event calls in a sequence that syncs with Animations and such
    /// </summary>
    public class PerformSkillHandler : ICombatAfterPreparationListener, ITempoListener, ISkippedTempoListener
    {
        [ShowInInspector] private CombatingEntity _currentUser;
        [ShowInInspector] private readonly SkillTargets _currentSkillTargets;
        private readonly SkillActionHandler _skillActionHandler;

        public PerformSkillHandler()
        {
            int sizeAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _currentSkillTargets = new SkillTargets(sizeAllocation); // it could be a whole targets
            _skillActionHandler = new SkillActionHandler();
        }

        public void OnAfterPreparation(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {

        }


        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _currentUser = entity;
            _currentSkillTargets.UsingSkill = null;
            _currentSkillTargets.Clear();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            _currentUser = null;
            _currentSkillTargets.UsingSkill = null;
            _currentSkillTargets.Clear();
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            OnFinisAllActions(entity);
        }

        private CoroutineHandle _doSkillHandle;

        public void DoSkill(CombatingEntity target)
        {
            _doSkillHandle =
                Timing.RunCoroutineSingleton(_DoSkill(target), _doSkillHandle, SingletonBehavior.Wait);
        }

        public static void SendDoSkill(CombatingEntity target)
        {
            CombatSystemSingleton.PerformSkillHandler.DoSkill(target);
        }

        //TODO Change Skill to Effect
        private IEnumerator<float> _DoSkill(CombatingEntity target)
        {
            var skill = _currentSkillTargets.UsingSkill;
            var mainEffect = skill.Preset.GetMainEffect();

            List<CombatingEntity> effectTargets;
            if (mainEffect != null)
                effectTargets = UtilsTargets.GetEffectTargets(_currentUser, target, mainEffect.GetEffectTarget());
            else
                effectTargets = target.CharacterGroup.Self;


            // TODO make a waitUntil(Animation call for Skill)
            yield return Timing.WaitUntilDone(_currentUser.CombatAnimator.DoAnimation(
                _currentUser, effectTargets,
                _currentSkillTargets.UsingSkill));
            _skillActionHandler.DoSkill(skill, _currentUser, target);

            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            skill.OnSkillUsage();
            CombatSystemSingleton.TempoHandler.DoSkillCheckFinish(_currentUser);
            CombatSystemSingleton.CharacterEventsTracker.Invoke();



        }

        public List<CombatingEntity> HandlePossibleTargets(CombatSkill skill)
        {
            UtilsTargets.InjectPossibleTargets(skill, _currentUser, _currentSkillTargets);
            return _currentSkillTargets;
        }

        public static List<CombatingEntity> SendHandlePossibleTargets(CombatSkill skill)
        {
            return CombatSystemSingleton.PerformSkillHandler.HandlePossibleTargets(skill);
        }


        public class SkillActionHandler : SkillArguments
        {
            //private readonly EffectsSeparationHandler _effectPool;

            private void Injection(CombatingEntity user, CombatingEntity target)
            {
                User = user;
                UserStats = user.CombatStats;

                Injection(target);
            }

            private void Injection(CombatingEntity target)
            {
                InitialTarget = target;
            }


            public void DoSkill(CombatSkill skill, CombatingEntity user, CombatingEntity target)
            {
                Injection(user, target);
                var skillPreset = skill.Preset;
                if (skill is null)
                {
                    throw new NullReferenceException("DoSkills() was invoked before preparation");
                }

                bool isOffensiveSkill = skillPreset.GetSkillType() == EnumSkills.TargetingType.Offensive;
                var targetGuarding = target.Guarding;
                if (isOffensiveSkill && targetGuarding.HasProtector())
                {
                    targetGuarding.VariateTarget(ref target);
                    InitialTarget = target;
                }

                //>>>>>>>>>>>>>>>>>>> DO Randomness
                float randomValue = Random.value;
                bool isCritical;
                var combatStats = user.CombatStats;
                if (UtilsCombatStats.IsCriticalPerformance(combatStats, skill, randomValue))
                {
                    isCritical = true;
                    float defaultHarmonyAddition
                        = CombatSystemSingleton.ParamsVariable.criticalHarmonyAddition;
                    UtilsCombatStats.VariateHarmony(target, defaultHarmonyAddition);

                    var criticalBuff = user.CharacterCriticalBuff;
                    criticalBuff?.OnCriticalAction();
                }
                else
                {
                    isCritical = false;
                }

                IsCritical = isCritical;

                // vvvvv Simple alternative vvvvv
                skillPreset.DoEffects(this);
            }
        }

    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;

        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {
        }
    }


    
}
