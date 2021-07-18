using System;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using Characters;
using MEC;
using Sirenix.OdinInspector;
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

        public bool SkipAnimations = false;

        public PerformSkillHandler()
        {
            int sizeAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _currentSkillTargets = new SkillTargets(sizeAllocation); // it could be a whole targets
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
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

        private IEnumerator<float> _DoSkill(CombatingEntity target)
        {
            CombatSkill skill = _currentSkillTargets.UsingSkill;
            if (skill is null)
            {
                throw new NullReferenceException("DoSkills() was invoked before preparation");
            }

            skill.OnSkillUsage();


            var skillEffects = skill.GetEffects();
            if (skillEffects is null || skillEffects.Count <= 0)
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
            if (UtilsCombatStats.IsCriticalPerformance(_currentUser.CombatStats,skill, randomValue))
            {
                isCritical = true;

                //ADD critical buff to the character
               CriticalActionBuff.AddCriticalBuff(_currentUser);
            }
            else
            {
                isCritical = false;
            }

            //>>>>>>>>>>>>>>>>>>> DO Main Effect
            List<CombatingEntity> effectTargets;
            var mainEffect = skillEffects[0];
            DoEffectOnTargets(mainEffect);
            yield return Timing.WaitUntilDone(_currentUser.CombatAnimator
                ._DoAnimation(_currentUser, effectTargets, _currentSkillTargets.UsingSkill));



            //>>>>>>>>>>>>>>>>>>> DO Secondary Effects
            // 1 since the main Effect is skillEffects[0]
            for (int i = 1; i < skillEffects.Count; i++)
            {
                DoEffectOnTargets(skillEffects[i]);
            }


            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            CombatSystemSingleton.TempoHandler.OnSkillActionFinish(_currentUser);


            //////////////////////
            void DoEffectOnTargets(EffectParams effect)
            {
                effectTargets = UtilsTargets.GetEffectTargets(effect,_currentUser, target);
                foreach (CombatingEntity effectTarget in effectTargets)
                {
                    UpdateRandomness();
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

        public List<CombatingEntity> GetPossibleTargets(CombatSkill skill)
        {
            return UtilsTargets.GetPossibleTargets(skill, _currentUser, _currentSkillTargets);
        }
    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;
        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {}
    }

}
