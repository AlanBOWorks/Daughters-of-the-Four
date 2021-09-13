using _CombatSystem;
using Characters;
using CombatEffects;
using Stats;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "_WAIT (self) [Skill]",
        menuName = "Combat/Skill/_Wait", order = -90)]
    public class SWaitSkill : SSkillPreset, IDelayBuff
    {
        public override void DoEffects(SkillArguments arguments)
        {
            var target = arguments.User;
            var targetStats = target.CombatStats;
            int amountOfActionLeft = targetStats.ActionsLefts;
            targetStats.ResetActionsAmount();
            target.DelayBuffHandler.EnqueueBuff(this,target, amountOfActionLeft);
        }

        public TempoTicker.TickType GetTickType() => TempoTicker.TickType.OnAfterSequence;

        public void DoBuff(CombatingEntity user, CombatingEntity target, float stacks)
        {
            target.CombatStats.BurstStats.ActionsPerInitiative += stacks;
        }

        public int MaxStack => GlobalCombatParams.ActionsPerInitiativeCap;

        public override IEffect GetMainEffect() => null;
    }
}
