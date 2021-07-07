using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    public class SkillUseHandler : ICombatAfterPreparationListener,ITempoListener
    {
        [ShowInInspector]
        private CombatingEntity _currentUser;
        public readonly Dictionary<CombatingEntity, SkillUsage> LastSkills;
        [ShowInInspector]
        private readonly List<CombatingEntity> _currentSkillTargets;

        public SkillUseHandler()
        {
            int sizeAllocation = CharacterUtils.PredictedAmountOfCharactersInBattle;
            LastSkills = new Dictionary<CombatingEntity, SkillUsage>(sizeAllocation);
            _currentSkillTargets = new List<CombatingEntity>(sizeAllocation); // it could be a whole targets
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            LastSkills.Clear();
            foreach (CombatingEntity entity in allEntities)
            {
                LastSkills.Add(entity, new SkillUsage());
            }
        }


        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _currentUser = entity;
        }

        public void OnActionDone(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            _currentUser = null;
        }

        public void DoSkill(CombatSkill skill, CombatingEntity target)
        {
            var skillUsage = LastSkills[_currentUser];
            skillUsage.OnTarget = target;
            skillUsage.Skill = skill;
            //TODO pass this to an SkullUsageHandler
        }

        public List<CombatingEntity> GetPossibleTargets(CombatSkill skill)
        {
            _currentSkillTargets.Clear();
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
                    if (_currentUser.PositionTracker.CombatPosition !=
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

    public class SkillUsage
    {
        public CombatSkill Skill;
        public CombatingEntity OnTarget;
    }
}
