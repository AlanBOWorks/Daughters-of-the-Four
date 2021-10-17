using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.CombatSkills;
using UnityEngine;

namespace CombatEffects
{
    public abstract class SEffect : SSkillComponentEffect, IEffect
    {
        public void DoEffect(SkillValuesHolders values, EnumEffects.TargetType effectTargetType, float effectValue, bool isCritical)
        {
            var user = values.User;
            var skillTarget = values.Target;
            var effectTargets = UtilsTarget.GetPossibleTargets(user, skillTarget, effectTargetType);
            foreach (var effectTarget in effectTargets)
            {
                DoEffectOn(values,effectTarget,effectValue, isCritical);
            }
        }

        protected abstract void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical);
    }


    public abstract class SOffensiveEffect : SEffect
    {
        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            var user = values.User;
            var effectResolution = DoEffectOn(user, effectTarget, effectValue, isCritical);

            CombatSystemSingleton.EventsHolder.OnPerformOffensiveAction(values,ref effectResolution);
            user.EventsHolder.OnPerformOffensiveAction(effectTarget,ref effectResolution);
            effectTarget.EventsHolder.OnReceiveOffensiveAction(user,ref effectResolution);
        }

        protected abstract SkillComponentResolution DoEffectOn(
            CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical);
    }

    public abstract class SSupportEffect : SEffect
    {
        protected override void DoEffectOn(
            SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            var user = values.User;
            var effectResolution = DoEffectOn(user, effectTarget, effectValue, isCritical);

            CombatSystemSingleton.EventsHolder.OnPerformSupportAction(values,ref effectResolution);
            user.EventsHolder.OnPerformSupportAction(effectTarget,ref effectResolution);
            effectTarget.EventsHolder.OnReceiveSupportAction(user,ref effectResolution);
        }

        protected abstract SkillComponentResolution DoEffectOn(
            CombatingEntity user, CombatingEntity effectTarget, float effectValue, bool isCritical);
    }
}
