using System.Collections;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Guarding [Effect]",
        menuName = "Combat/Effect/Guarding")]
    public class SGuarding : SEffect
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveSupportEffect(entities,ref resolution);
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue,
            bool isCritical)
        {
            if (effectValue < 1 || effectValue <= Random.value)
                return new SkillComponentResolution(this, 0);

            user.GuardHandler.GuardTarget(effectTarget);
            return new SkillComponentResolution(this,1);

        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Shielding;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Guard;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("F1") + "%";
        }
    }

    public sealed class GuardHandler
    {
        public GuardHandler(CombatingEntity user)
        {
            _user = user;
        }

        private readonly CombatingEntity _user;
        [ShowInInspector]
        public CombatingEntity CurrentGuarding { get; private set; }
        [ShowInInspector]
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
                values.SwitchTarget(GuardedBy);
        }
    }
}
