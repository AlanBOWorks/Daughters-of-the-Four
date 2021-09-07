using System.Collections.Generic;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{
    //TODO add to CombatingEntity
    public class FateSkillsHandler
    {
        public FateSkillsHandler(CombatingEntity user, int fateAmount = 1)
        {
            _user = user;
            FateAmount = fateAmount;
            _usedSkills = new Stack<FateValue>(fateAmount);
        }

        private readonly CombatingEntity _user;
        private readonly Stack<FateValue> _usedSkills;
        /// <summary>
        /// The amount of repetitions (Fate Skills) that can be used
        /// </summary>
        public int FateAmount;

        public void SaveSkill(CombatingEntity target, CombatSkill usedSkill)
        {
            // [Other] types are special types (Wait, Scripted events, global effects) that
            // can be problematic if used with the Fate system
            if(usedSkill.Preset.GetSkillType() == EnumSkills.TargetingType.Other) 
                return;
            _usedSkills.Push(new FateValue(target,usedSkill));
        }

        public void InvokeFateSkills()
        {
            PerformSkillHandler performSkill = CombatSystemSingleton.PerformSkillHandler;
            for (int i = 0; i < FateAmount && _usedSkills.Count > 0; i++)
            {
                var fateSkill = _usedSkills.Pop();
                performSkill.DoSkill(fateSkill.UsedSkill,_user,fateSkill.Target);
            }
            _usedSkills.Clear();
        }


        private struct FateValue
        {
            public readonly CombatingEntity Target;
            public readonly CombatSkill UsedSkill;

            public FateValue(CombatingEntity target, CombatSkill usedSkill)
            {
                Target = target;
                UsedSkill = usedSkill;
            }
        }
    }

    public class FateSkillsInvoker : ITempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            entity.FateSkills.InvokeFateSkills();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
        }
    }
}
