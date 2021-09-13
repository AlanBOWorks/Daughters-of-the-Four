using _CombatSystem;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N (T) - [Delay BUFF Skill Preset]",
        menuName = "Combat/Buffs/Delay Buff", order = 100)]
    public class SDelayBuffPreset : SSkillPreset, IDelayBuff
    {
        [SerializeField,TitleGroup("Stats")] 
        protected TempoTicker.TickType tickType 
            = TempoTicker.TickType.OnBeforeSequence;

        [SerializeField,TitleGroup("Stats"),Range(0,100)]
        private int maxStack = 1;

        /*
        // Remove for simplicity; But could be re-implemented

         [SerializeField,TitleGroup("Effects"), Tooltip("This aren't affected by maxStack")]
        private EffectParams[] maxLessEffects = new EffectParams[0];
        */

        public TempoTicker.TickType GetTickType() => tickType;
        public int MaxStack => maxStack;

        protected override void DoEffect(SkillArguments arguments, int effectIndex)
        {
            //Just to avoid repetition of EnqueueBuff (since the effectIndex depends of effects.Length)
            if (effectIndex > 0) return;
            var user = arguments.User;
            var target = arguments.InitialTarget;
            target.DelayBuffHandler.EnqueueBuff(this, user);
        }

        public override void DoDirectEffects(CombatingEntity user, CombatingEntity target)
        {
            target.DelayBuffHandler.EnqueueBuff(this,user);
        }

        public void DoBuff(CombatingEntity user,CombatingEntity target, float stacks)
        {
            float modifier = stacks;
                if (modifier > maxStack) modifier = maxStack;
            base.DoEffects(user,target, modifier);

            // Remove for simplicity; But could be re-implemented
            // base.DoEffects(user,target,stacks,maxLessEffects);
        }


        private const string BuffPresetPrefix = " - [Delay BUFF Preset]";
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + " - " + tickType + BuffPresetPrefix;
        }
    }
}
