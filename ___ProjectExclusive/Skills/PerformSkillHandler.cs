using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using CombatEffects;
using MEC;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills
{
    public class PerformSkillHandler : ICombatAfterPreparationListener, ITempoListener
    {
        [ShowInInspector]
        private CombatingEntity _currentUser;
        [ShowInInspector]
        private readonly SkillTargets _currentSkillTargets;

        public PerformSkillHandler()
        {
            int sizeAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _currentSkillTargets = new SkillTargets(sizeAllocation); // it could be a whole targets
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

        private CoroutineHandle _doSkillHandle;
        public void DoSkill(CombatingEntity target)
        {
            _doSkillHandle =
                Timing.RunCoroutineSingleton(_DoSkill(target), _doSkillHandle,SingletonBehavior.Wait);
        }
        public static void SendDoSkill(CombatingEntity target)
        {
            CombatSystemSingleton.PerformSkillHandler.DoSkill(target);
        }

        private IEnumerator<float> _DoSkill(CombatingEntity target)
        {
            CombatSkill skill = _currentSkillTargets.UsingSkill;
            var skillPreset = skill.Preset;
            if (skill is null)
            {
                throw new NullReferenceException("DoSkills() was invoked before preparation");
            }

            skill.OnSkillUsage();
            var effects = skillPreset.GetEffects;
            var mainEffect = effects[0];


            if (effects.Length <= 0)
            {
#if UNITY_EDITOR
                Debug.LogError("Skill doesn't have an effect");
#endif                
                yield break;
            }


            //>>>>>>>>>>>>>>>>>>> DO Randomness
            float randomValue = Random.value;
            bool isCritical;
            float randomModifier;
            var combatStats = _currentUser.CombatStats;
            if (UtilsCombatStats.IsCriticalPerformance(combatStats,skill, randomValue))
            {
                isCritical = true;
                float defaultHarmonyAddition 
                    = CombatSystemSingleton.ParamsVariable.criticalHarmonyAddition;
                UtilsCombatStats.AddHarmony(combatStats, defaultHarmonyAddition);
                //ADD critical buff to the character
                var criticalBuff = _currentUser.CriticalBuff;
                
                if(criticalBuff != null)
                    _currentUser.DelayBuffHandler.EnqueueBuff(criticalBuff);
            }
            else
            {
                isCritical = false;
            }

            //>>>>>>>>>>>>>>>>>>> DO Main Effect
            List<CombatingEntity> effectTargets;
            DoEffectOnTargets(mainEffect);
            yield return Timing.WaitUntilDone(_currentUser.CombatAnimator
                ._DoAnimation(_currentUser, effectTargets, _currentSkillTargets.UsingSkill));



            //>>>>>>>>>>>>>>>>>>> DO Secondary Effects
            // 1 since the main Effect is skillEffects[0]
            for (int i = 1; i < effects.Length; i++)
            {
                DoEffectOnTargets(effects[i]);
            }


            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            CombatSystemSingleton.TempoHandler.OnSkillActionFinish(_currentUser);


            //////////////////////
            void DoEffectOnTargets(IEffect effect)
            {
                effectTargets 
                    = UtilsTargets.GetEffectTargets(_currentUser, target,effect.GetEffectTarget());
                foreach (CombatingEntity effectTarget in effectTargets)
                {
                    if (effect.CanPerformRandom())
                        UpdateRandomness();
                    else
                        randomModifier = 1;   

                    effect.DoEffect(_currentUser, effectTarget,randomModifier);
                }
            }

            void UpdateRandomness()
            {
                if (isCritical)
                {
                    randomModifier = UtilsCombatStats.RandomHigh;
                    return;
                }

                randomValue = Random.value;
                randomModifier = UtilsCombatStats.CalculateRandomModifier(randomValue);

            }
        }

        public List<CombatingEntity> HandlePossibleTargets(CombatSkill skill)
        {
            return UtilsTargets.GetPossibleTargets(skill, _currentUser, _currentSkillTargets);
        }

        public static List<CombatingEntity> SendHandlePossibleTargets(CombatSkill skill)
        {
            return CombatSystemSingleton.PerformSkillHandler.HandlePossibleTargets(skill);
        }


    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;
        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {}
    }

}
