using _CombatSystem;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N (T) - [Delay BUFF Skill Preset]",
        menuName = "Combat/Buffs/Delay Buff")]
    public class SDelayBuffPreset : SEffectSetPreset, IDelayBuff
    {
        [SerializeField,TitleGroup("Stats")] 
        protected TempoHandler.TickType tickType 
            = TempoHandler.TickType.OnBeforeSequence;

        [SerializeField,TitleGroup("Stats"),Range(0,100)]
        private int maxStack = 1;

        [SerializeField,TitleGroup("Effects"), Tooltip("This aren't affected by maxStack")]
        private EffectParams[] maxLessEffects = new EffectParams[0];

        public TempoHandler.TickType GetTickType() => tickType;
        public int MaxStack => maxStack;

        public void DoBuff(CombatingEntity user, float stacks)
        {
            float modifier = stacks;
                if (modifier > maxStack) modifier = maxStack;
            DoEffects(user,user, modifier);
            DoEffects(user,user,stacks,maxLessEffects);
        }


        private const string BuffPresetPrefix = " - [Delay BUFF Preset]";
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + " - " + tickType + BuffPresetPrefix;
        }
    }
}
