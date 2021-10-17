using System.Collections;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using UnityEngine;

namespace CombatEffects
{
    public class SGuarding : SEffect
    {
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            if(effectValue < 1 || effectValue <= Random.value) return;

            var user = values.User;
            user.GuardHandler.GuardTarget(effectTarget);
        }
    }

    public class GuardHandler
    {
        public GuardHandler(CombatingEntity user)
        {
            _user = user;
        }

        private readonly CombatingEntity _user;
        public CombatingEntity CurrentGuarding { get; private set; }
        public CombatingEntity GuardedBy { get; private set; }

        public void GuardTarget(CombatingEntity guardTarget)
        {
            CurrentGuarding?.GuardHandler.RemoveGuardedBy();
            CurrentGuarding = guardTarget;
            guardTarget.GuardHandler.ReceiveGuard(_user);
        }

        private void ReceiveGuard(CombatingEntity guardedBy)
        {
            GuardedBy?.GuardHandler.RemoveGuardingTarget();
            GuardedBy = guardedBy;
        }


        private void RemoveGuardingTarget() => CurrentGuarding = null;
        private void RemoveGuardedBy() => GuardedBy = null;

        public void RemoveGuarding()
        {
            CurrentGuarding?.GuardHandler.RemoveGuardedBy();
            CurrentGuarding = null;
        }

        public void VariateTarget(SkillValuesHolders values)
        {
            if (GuardedBy != null)
                values.DoGuardSwitch(GuardedBy);
        }
    }
}
