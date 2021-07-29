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

        public TempoHandler.TickType GetTickType() => tickType;
        public int MaxStack => maxStack;

        public void DoBuff(CombatingEntity user, float modifier)
        {
            DoDirectEffects(user,user,modifier);
        }


        private const string BuffPresetPrefix = " - [Delay BUFF Preset]";
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + " - " + tickType + BuffPresetPrefix;
        }
    }
}
