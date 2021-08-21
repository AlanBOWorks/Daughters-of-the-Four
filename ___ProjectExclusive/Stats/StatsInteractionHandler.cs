using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using CombatEffects;
using MEC;
using Sirenix.OdinInspector;
using Skills;
using Random = UnityEngine.Random;

namespace Stats
{
    /// <summary>
    /// Its job is to keep track the <see cref="CharacterCombatStatsFull"/> of all participants in one calculation
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
        }

        [ShowInInspector]
        private readonly SkillArguments _skillArguments;


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

            skillPreset.DoEffects(_skillArguments);
        }


    }
}
