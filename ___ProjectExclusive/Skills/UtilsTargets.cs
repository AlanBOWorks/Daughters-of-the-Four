using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{
    public static class UtilsTargets
    {
        public static List<CombatingEntity> GetEffectTargets(EffectParams effect,CombatingEntity user, CombatingEntity target)
        {
            List<CombatingEntity> applyEffectOn;
            SEffectBase.EffectTarget targetType = effect.GetEffectTarget();
            if (targetType == SEffectBase.EffectTarget.All)
                return CombatSystemSingleton.Characters.AllEntities;

            CharacterSelfGroup targetGroup;
            if (effect.GetEffectType() == SEffectBase.EffectType.SelfOnly)
            {
                targetGroup = user.CharacterGroup;
            }
            else
            {
                targetGroup = target.CharacterGroup;
            }


            switch (targetType)
            {
                case SEffectBase.EffectTarget.Target:
                    applyEffectOn = targetGroup.Self;
                    break;
                case SEffectBase.EffectTarget.TargetTeam:
                    applyEffectOn = targetGroup.Team;
                    break;
                case SEffectBase.EffectTarget.TargetTeamExcluded:
                    applyEffectOn = targetGroup.TeamNotSelf;
                    break;
                default:
                    throw new ArgumentException($"Target type is not defined: {(int) targetType}");
            }

            return applyEffectOn;
        }

        public static List<CombatingEntity> GetPossibleTargets(CombatSkill skill,
            CombatingEntity user, SkillTargets injectInList)
        {
            injectInList.UsingSkill = skill;
            return GetPossibleTargets(skill, user, injectInList as List<CombatingEntity>);
        }
        public static List<CombatingEntity> GetPossibleTargets(CombatSkill skill, 
            CombatingEntity user, List<CombatingEntity> InjectInList)
        {
            InjectInList.Clear();
            PrepareTargets();
            return InjectInList;

            void PrepareTargets()
            {
                var skillPreset = skill.Preset;
                SEffectBase.EffectType effectType = skillPreset.MainEffectType;


                if (effectType == SEffectBase.EffectType.SelfOnly)
                {
                    InjectInList.Add(user);
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
                        InjectInList.Add(entity);
                    }
                }

                void AddByEnemyTeam()
                {

                    var userAreaTracker = user.AreasDataTracker;

                    CombatingTeam enemyTeam = user.CharacterGroup.Enemies;

                    switch (userAreaTracker.RangeType)
                    {
                        case CharacterArchetypes.RangeType.Melee:
                            AddMeleeType();
                            break;
                        case CharacterArchetypes.RangeType.Range:
                            AddRangedType();
                            break;
                        case CharacterArchetypes.RangeType.Hybrid:
                            AddHybridType();
                            break;
                        default:
                            throw new NotImplementedException("Ranged type not implemented in targeting " +
                                                              $"{userAreaTracker.RangeType}");
                    }

                    void AddRangedType()
                    {
                        for (var i = 0; i < enemyTeam.Count; i++)
                        {
                            CombatingEntity target = enemyTeam[i];
                            //Vanguards are always added (i == 0 : Vanguard)
                            if (i == 0 || !CharacterArchetypes.IsInCloseRange(user, target))
                                AddIfConscious(target);
                        }
                    }

                    void AddMeleeType()
                    {
                        for (var i = 0; i < enemyTeam.Count; i++)
                        {
                            CombatingEntity target = enemyTeam[i];
                            //Vanguards are always added (i == 0 : Vanguard)
                            if (i == 0 || CharacterArchetypes.IsInCloseRange(user, target))
                                AddIfConscious(target);
                        }
                    }

                    void AddHybridType()
                    {
                        foreach (var target in enemyTeam)
                        {
                            AddIfConscious(target);
                        }
                    }

                    void AddIfConscious(CombatingEntity entity)
                    {
                        if(entity.IsConscious())
                            InjectInList.Add(entity);
                    }
                }
            }
        }
    }
}
