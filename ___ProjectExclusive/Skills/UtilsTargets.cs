using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
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

        
        public static void InjectPossibleTargets(SkillBase skill, 
            CombatingEntity user, List<CombatingEntity> injectInList)
        {
            injectInList.Clear();
            PrepareTargets();

            void PrepareTargets()
            {
                var skillPreset = skill.Preset;
                var mainEffect = skillPreset.GetMainEffect();

                EnumSkills.TargetingType skillType = skill.GetMainType();


                if (skillType == EnumSkills.TargetingType.SelfOnly || mainEffect == null)
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
                    if (skillType == EnumSkills.TargetingType.Support)
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
                        AddIfConscious(entity);
                    }
                }
                void AddByEnemyTeam()
                {
                    Debug.Log($"Using skill: {skill.SkillName}");
                    Debug.Log($"Agent: {user.CharacterName}");
                    var userAreaTracker = user.AreasDataTracker;

                    CombatingTeam enemyTeam = user.CharacterGroup.Enemies;
                    AddAllIfConscious();

                    switch (userAreaTracker.RangeType)
                    {
                        case EnumCharacter.RangeType.Melee:
                            RemoveFarTargets();
                            break;
                        case EnumCharacter.RangeType.Range:
                            RemoveCloseTargets();
                            break;
                        case EnumCharacter.RangeType.HybridRange:
                            break;
                        default:
                            throw new NotImplementedException("Ranged type not implemented in targeting " +
                                                              $"{userAreaTracker.RangeType}");
                    }



                    void AddAllIfConscious()
                    {
                        foreach (var target in enemyTeam)
                        {
                            AddIfConscious(target);
                        }
                    }

                    void RemoveCloseTargets()
                    {
                        // (Inverse targets because backLine are the first to be removed)
                        for (int i = injectInList.Count - 1; i >= 0 && injectInList.Count > 1; i--)
                        {
                            CombatingEntity target = enemyTeam[i];
                            if (UtilsCharacterArchetypes.IsInCloseRange(user, target))
                                injectInList.Remove(target);
                        }
                    }

                    void RemoveFarTargets()
                    {
                        
                        for (int i = injectInList.Count - 1; i >= 0 && injectInList.Count > 1; i--)
                        {
                            CombatingEntity target = enemyTeam[i];
                            if (!UtilsCharacterArchetypes.IsInCloseRange(user, target))
                                injectInList.Remove(target);
                        }

                    }

                }

                void AddIfConscious(CombatingEntity entity)
                {
                    if (entity.IsConscious())
                        injectInList.Add(entity);
                }
            }
        }
    }
}
