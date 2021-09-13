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
    public class PerformSkillHandler
    {
        public PerformSkillHandler()
        {
            int sizeAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _currentSkillTargets = new List<CombatingEntity>(sizeAllocation); // it could be a whole targets
            _skillActionHandler = new SkillActionHandler();

            TrackedDoSkill = new PerformedSkill();
        }

        [ShowInInspector]
        public readonly PerformedSkill TrackedDoSkill;

        [ShowInInspector] 
        private readonly List<CombatingEntity> _currentSkillTargets;
        private readonly SkillActionHandler _skillActionHandler;
        private CoroutineHandle _doSkillHandle;

        public void ResetOnInitiative()
        {
            _currentSkillTargets.Clear();
        }

        public void ResetOnFinish()
        {
            _currentSkillTargets.Clear();
        }


        /// <summary>
        /// Perform the current[<see cref="CombatSkill"/>] and saves it in the [<seealso cref="FateSkillsHandler"/>].
        /// The used skill will use the default behaviour (eg: cooldown) through [<see cref="CombatSkill.OnSkillUsage"/>]
        /// </summary>
        public void InjectSkill(CombatSkill skill, CombatingEntity user, CombatingEntity target)
        {
            user.FateSkills.SaveSkill(target,skill);
            TrackedDoSkill.Inject(skill,user,target);
        }


        public IEnumerator<float> _DoSkill(CombatSkill skill, CombatingEntity user, CombatingEntity target)
        {
            var mainEffect = skill.Preset.GetMainEffect();

            List<CombatingEntity> effectTargets;
            if (mainEffect != null)
                effectTargets = UtilsTargets.GetEffectTargets(user, target, mainEffect.GetEffectTarget());
            else
                effectTargets = target.CharacterGroup.Self;


            // TODO make a waitUntil(Animation call for Skill)
            yield return Timing.WaitUntilDone(user.CombatAnimator.DoAnimation(
                user, effectTargets, skill));


            _skillActionHandler.DoSkill(skill,user, target);
            CombatSystemSingleton.SkillUsagesEvent.InvokeSkill(skill);

            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            CombatSystemSingleton.CharacterEventsTracker.Invoke();
        }


        public IEnumerator<float> _DoPerformedSkill()
        {
            var skill = TrackedDoSkill.UsedSkill;
            var user = TrackedDoSkill.User;
            var target = TrackedDoSkill.Target;
            TrackedDoSkill.Clear();
            return _DoSkill(skill,user,target);
        }


        public List<CombatingEntity> GetPossibleTargets(CombatSkill skill, CombatingEntity user)
        {
            UtilsTargets.InjectPossibleTargets(skill, user, _currentSkillTargets);
            return _currentSkillTargets;
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


    public class PerformedSkill
    {
        public CombatSkill UsedSkill;
        public CombatingEntity User;
        public CombatingEntity Target;

        public bool IsValid()
        {
            return UsedSkill != null && User != null && Target != null;
        }

        public void Inject(CombatSkill skill, CombatingEntity user, CombatingEntity target)
        {
            UsedSkill = skill;
            User = user;
            Target = target;
        }

        public void Clear()
        {
            UsedSkill = null;
            User = null;
            Target = null;
        }
    }
    
}
