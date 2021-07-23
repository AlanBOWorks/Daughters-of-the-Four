﻿using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using CombatEffects;
using UnityEngine;

namespace Skills
{
    public static class UtilsTargets
    {
        public static List<CombatingEntity> GetEffectTargets(
            CombatingEntity user, 
            CombatingEntity target, 
            SEffectBase.EffectTarget targetType)
        {
            List<CombatingEntity> applyEffectOn;

            switch (targetType)
            {
                case SEffectBase.EffectTarget.All:
                return CombatSystemSingleton.Characters.AllEntities;

                case SEffectBase.EffectTarget.Target:
                    applyEffectOn = target.CharacterGroup.Self;
                    break;
                case SEffectBase.EffectTarget.TargetTeam:
                    applyEffectOn = target.CharacterGroup.Team;
                    break;
                case SEffectBase.EffectTarget.TargetTeamExcluded:
                    applyEffectOn = target.CharacterGroup.TeamNotSelf;
                    break;
                case SEffectBase.EffectTarget.Self:
                    applyEffectOn = user.CharacterGroup.Self;
                    break;
                case SEffectBase.EffectTarget.SelfTeam:
                    applyEffectOn = user.CharacterGroup.Team;
                    break;
                case SEffectBase.EffectTarget.SelfTeamNotIncluded:
                    applyEffectOn = user.CharacterGroup.TeamNotSelf;
                    break;
                default:
                    throw new ArgumentException($"Target type is not defined: {targetType}");
            }

            return applyEffectOn;
        }

        public static List<CombatingEntity> GetPossibleTargets(CombatSkill skill,
            CombatingEntity user, SkillTargets injectInList)
        {
            injectInList.UsingSkill = skill;
            return GetPossibleTargets(skill, user, injectInList as List<CombatingEntity>);
        }
        public static List<CombatingEntity> GetPossibleTargets(Skill skill, 
            CombatingEntity user, List<CombatingEntity> injectInList)
        {
            injectInList.Clear();
            PrepareTargets();
            return injectInList;

            void PrepareTargets()
            {
                var skillPreset = skill.Preset;
                var mainEffect = skillPreset.GetMainEffect();

                SSkillPresetBase.SkillType skillType = skill.GetMainType();


                if (skillType == SSkillPresetBase.SkillType.SelfOnly)
                {
                    injectInList.Add(user);
                    return;
                }


                SEffectBase.EffectTarget targetType = mainEffect.GetEffectTarget();
                if (targetType == SEffectBase.EffectTarget.All)
                {
                    AddByPredefinedTargets(CombatSystemSingleton.Characters.AllEntities);
                }
                else
                {
                    if (skillType == SSkillPresetBase.SkillType.Support)
                    {
                        AddByPredefinedTargets(user.CharacterGroup.Team);
                    }
                    else
                    {
                        AddByEnemyTeam();
                    }
                }

                if (!skillPreset.CanTargetSelf())
                {
                    injectInList.Remove(user);
                }

                void AddByPredefinedTargets(List<CombatingEntity> targets)
                {
                    foreach (CombatingEntity entity in targets)
                    {
                        injectInList.Add(entity);
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
                            injectInList.Add(entity);
                    }
                }
            }
        }
    }
}
