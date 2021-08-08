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
    public class PerformSkillHandler : ICombatAfterPreparationListener, ITempoListener, ISkippedTempoListener
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


            //>>>>>>>>>>>>>>>>>>> DO Randomness
            float randomValue = Random.value;
            bool isCritical;
            var combatStats = _currentUser.CombatStats;
            if (UtilsCombatStats.IsCriticalPerformance(combatStats,skill, randomValue))
            {
                isCritical = true;
                float defaultHarmonyAddition 
                    = CombatSystemSingleton.ParamsVariable.criticalHarmonyAddition;
                UtilsCombatStats.AddHarmony(target, defaultHarmonyAddition);

                var criticalBuff = _currentUser.CharacterCriticalBuff;
                criticalBuff?.OnCriticalAction();
            }
            else
            {
                isCritical = false;
            }
            DoSkillArguments skillArguments = new DoSkillArguments(_currentUser,target,isCritical);

            //>>>>>>>>>>>>>>>>>>> DO Main Effect
            List<CombatingEntity> effectTargets = skillPreset.GetMainEffectTargets(_currentUser, target);

            skillPreset.DoMainEffect(ref skillArguments);
            yield return Timing.WaitUntilDone(
                    _currentUser.CombatAnimator._DoAnimation(_currentUser, effectTargets, _currentSkillTargets.UsingSkill));
            skillPreset.DoSecondaryEffects(ref skillArguments);

            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            skill.OnSkillUsage();
            CombatSystemSingleton.TempoHandler.DoSkillCheckFinish(_currentUser);

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


    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;
        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {}
    }

}
