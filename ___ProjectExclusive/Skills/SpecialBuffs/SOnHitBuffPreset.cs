using _CombatSystem;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(fileName = "N (T) - [On HIT BUFF]",
        menuName = "Combat/Buffs/On HIT Buff")]
    public class SOnHitBuffPreset : SSkillPreset, IDelayBuff
    {
        [SerializeField, TitleGroup("Stats"), Range(0, 100)]
        private int maxStack = 1;
        [SerializeField] private bool isOnHit = false;

        public TempoHandler.TickType GetTickType() => TempoHandler.TickType.OnBeforeSequence;
        public int MaxStack => maxStack;

        protected override void DoEffect(ref DoSkillArguments arguments, int effectIndex)
        {
            if(effectIndex > 0) return;

            var actor = arguments.User;
            var target = arguments.Target;
            target.Events.OnHitEvent.AddBuff(this,actor,isOnHit);
        }

        public override void DoDirectEffects(CombatingEntity user, CombatingEntity target)
        {
            target.Events.OnHitEvent.AddBuff(this,user,isOnHit);
        }

        public void DoBuff(CombatingEntity user, CombatingEntity target, float stacks)
        {
            Debug.Log($"Invoking_ User: {user.CharacterName} / target: {target.CharacterName}" );
            float modifier = stacks;
            if (modifier > maxStack) modifier = maxStack;
            base.DoEffects(user, target, modifier);
        }

        protected override string FullAssetName(IEffect mainEffect)
        {
            var onHit = isOnHit 
                ? " - [On " 
                : " - [On Avoid ";

            return skillName + ValidationName(mainEffect) + " - " + GetTickType() +
                   onHit + "HIT BUFF Preset]";
        }
    }
}
