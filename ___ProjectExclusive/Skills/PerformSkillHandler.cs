using System;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    public class PerformSkillHandler : ICombatAfterPreparationListener,ITempoListener
    {
        [ShowInInspector]
        private CombatingEntity _currentUser;
        [ShowInInspector]
        private readonly SkillTargets _currentSkillTargets;

        public bool SkipAnimations = false;

        public PerformSkillHandler()
        {
            int sizeAllocation = CharacterUtils.PredictedAmountOfCharactersInBattle;
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
                effectTargets = UtilsTargets.GetEffectTargets(effect, target);
                foreach (CombatingEntity effectTarget in effectTargets)
                {
                    effect.DoEffect(_currentUser, effectTarget);
                }
            }
        }

        public List<CombatingEntity> GetPossibleTargets(CombatSkill skill)
        {
            _currentSkillTargets.Clear();
            _currentSkillTargets.UsingSkill = skill;
            PrepareTargets();
            return _currentSkillTargets;

            void PrepareTargets()
            {
                var user = _currentUser;
                var skillPreset = skill.Preset;
                SEffectBase.EffectType effectType = skillPreset.MainEffectType;


                if (effectType == SEffectBase.EffectType.SelfOnly)
                {
                    _currentSkillTargets.Add(_currentUser);
                    return;
                }

                SEffectBase.EffectTarget targetType = skillPreset.MainEffectTarget;
                if (targetType == SEffectBase.EffectTarget.All)
                {
                    AddByPredefinedTargets(CombatSystemSingleton.Characters.AllEntities);
                }
                else
                {
                    if (effectType == SEffectBase.EffectType.Support)
                    {
                        AddByPredefinedTargets(user.CharacterGroup.Team);
                    }
                    else
                    {
                        AddByEnemyTeam();
                    }
                }

                void AddByPredefinedTargets(List<CombatingEntity> targets)
                {
                    foreach (CombatingEntity entity in targets)
                    {
                        _currentSkillTargets.Add(entity);
                    }
                }

                void AddByEnemyTeam()
                {
                    CombatingTeam enemyTeam = user.CharacterGroup.Enemies;
                    AddIfCan(enemyTeam.FrontLiner);
                    if (_currentUser.areasTracker.CombatPosition !=
                        CharacterArchetypes.PositionType.InEnemyTeam) return;
                    
                    var enemyAttacker 
                        = enemyTeam.MidLiner;

                    if(enemyAttacker.IsConscious())
                        _currentSkillTargets.Add(enemyAttacker);
                    else
                        AddIfCan(enemyTeam.BackLiner);
                }
            }

            void AddIfCan(CombatingEntity target)
            {
                if (target.IsConscious()) //There should be someone alive, if not the combat simply just ended
                {
                    _currentSkillTargets.Add(target);
                }
            }
        }




    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;
        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {}
    }

}
