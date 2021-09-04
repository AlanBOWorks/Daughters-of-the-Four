using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using CombatEffects;
using MEC;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stats
{
    /// <summary>
    /// Its job is to keep track the <see cref="CombatStatsFull"/> of all participants in one calculation
    /// and then execute the requested Skills/actions in a one go.<br></br>
    /// <br></br>
    /// This exits because some effects could have conditions and it could trigger false positives 
    /// as a consequence of having the stats doing their job step by step instead before the SKill
    /// <example>(eg: a skill could increase the damage
    /// done if the enemy has 50% or less HP, but in the start of the operation the enemy didn't have less than
    /// 50%, yet because a previous effect of dealing damage it triggers a false positive)</example>
    /// </summary>
    public class StatsInteractionHandler
    {
        public StatsInteractionHandler()
        {
            _skillArguments = new SkillArguments();
            //_effectPool = new EffectsSeparationHandler();
        }

        [ShowInInspector]
        private readonly SkillArguments _skillArguments;

        //private readonly EffectsSeparationHandler _effectPool;

        private void Injection(CombatingEntity user, CombatingEntity target)
        {
            _skillArguments.User = user;
            _skillArguments.UserStats = user.CombatStats;

            Injection(target);
        }

        private void Injection(CombatingEntity target)
        {
            _skillArguments.InitialTarget = target;
        }


        public void DoSkill(CombatSkill skill, CombatingEntity user, CombatingEntity target)
        {
            Injection(user,target);
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
                _skillArguments.InitialTarget = target;
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
                UtilsCombatStats.AddHarmony(target, defaultHarmonyAddition);

                var criticalBuff = user.CharacterCriticalBuff;
                criticalBuff?.OnCriticalAction();
            }
            else
            {
                isCritical = false;
            }

            _skillArguments.IsCritical = isCritical;

            // vvvvv Simple alternative vvvvv
            skillPreset.DoEffects(_skillArguments);
        }

        /// <summary>
        /// This is to separate the effect targeting and be able to separate passives/filters without
        /// conflicting with other targets
        /// </summary>
        private class EffectsSeparationHandler
        {
            public EffectsSeparationHandler()
            { 
                _targets = new List<CombatingEntity>();
                _effectsForTarget = new List<Queue<EffectParams>>();

                _effectsPooling = new Queue<Queue<EffectParams>>();
            }


            /// <summary>
            /// Keeps tracks of the targets; The target index will be the same than <see cref="_effectsForTarget"/>'s indexes
            /// </summary>
            private readonly List<CombatingEntity> _targets;
            /// <summary>
            /// Keeps track of all effect applying to the <see cref="_targets"/> (they both share the same indexes)
            /// </summary>
            private readonly List<Queue<EffectParams>> _effectsForTarget;

            /// <summary>
            /// Just save the [<see cref="Queue{T}"/>] by pooling so it avoids GC (it doesn't participates
            /// in the calculations)
            /// </summary>
            private readonly Queue<Queue<EffectParams>> _effectsPooling;

            private Queue<EffectParams> PoolQueue()
            {
                if (_effectsPooling.Count <= 0)
                    return new Queue<EffectParams>();
                return _effectsPooling.Dequeue();
            }

            private void AddEffect(CombatingEntity target, EffectParams effect)
            {
                if (!_targets.Contains(target))
                {
                    PoolQueueAndInjectEffect();
                    return;
                }

                EnQueueEffect();

                void PoolQueueAndInjectEffect()
                {
                    var pooledQueue = PoolQueue();
                    _targets.Add(target);
                    _effectsForTarget.Add(pooledQueue);

                    pooledQueue.Enqueue(effect);
                }
                void EnQueueEffect()
                {
                    int index = _targets.IndexOf(target);
                    if (index <= 0) return;

                    var effects = _effectsForTarget[index];
                    effects.Enqueue(effect);
                }
            }

            private void AddEffect(List<CombatingEntity> targets, EffectParams effect)
            {
                foreach (CombatingEntity effectTarget in targets)
                {
                    AddEffect(effectTarget, effect);
                }
            }

            public void PreparePool(CombatSkill skill,CombatingEntity user, CombatingEntity target)
            {
                var effects = skill.Preset.Effects;
                foreach (EffectParams effect in effects)
                {
                    var targets = UtilsTargets.GetEffectTargets(user, target, effect.GetEffectTarget());
                    AddEffect(targets,effect);
                }
            }

            public void DoPoolEffects(SkillArguments arguments)
            {
                for (var i = _targets.Count - 1; i >= 0; i--)
                {
                    var queue = _effectsForTarget[i];
                    _effectsForTarget.RemoveAt(i);
                    var effectTarget = _targets[i];
                    _targets.RemoveAt(i);

                    DoEffectsOn(queue, arguments, effectTarget);
                }
            }

            private void DoEffectsOn(Queue<EffectParams> effects, SkillArguments arguments, CombatingEntity target)
            {
                bool isCritical = arguments.IsCritical;
                while (effects.Count > 0)
                {
                    var effect = effects.Dequeue();
                    float effectValue = CalculateRandom() * effect.power;

                    // Effect filtering
                    arguments.User.PassivesHolder.EffectFilters.DoFilterOnAction(effect, ref effectValue);
                    target.PassivesHolder.EffectFilters.DoFilterOnReaction(effect,ref effectValue);

                    // Do
                    effect.DoEffect(arguments, target, effectValue);


                    float CalculateRandom()
                    {
                        if (effect.CanPerformRandom())
                        {
                            return isCritical 
                                ? UtilsCombatStats.RandomHigh 
                                : UtilsCombatStats.CalculateRandomModifier();
                        }

                        return 1;
                    }
                }

                // Pool to avoid GC
                _effectsPooling.Enqueue(effects);
            }

        }
    }
}
