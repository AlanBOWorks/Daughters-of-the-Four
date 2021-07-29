using System;
using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Support Buff - N [Preset]",
        menuName = "Combat/Effects/Buff/Support Buff")]
    public class SSupportStatBuffEffect : SEffectBuffBase
    {
        [SerializeField]
        private EnumStats.Support buffType
            = EnumStats.Support.Heal;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            effectModifier *= user.CombatStats.BuffPower;
            DoEffect(target, effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            DoBuff(GetBuff(target), effectModifier);

        }

        protected void DoBuff(ICharacterBasicStats buffStats, float effectModifier)
        {
            switch (buffType)
            {
                case EnumStats.Support.Heal:
                    buffStats.HealPower += effectModifier;
                    break;
                case EnumStats.Support.Buff:
                    buffStats.BuffPower += effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }
        }



        private const string SupportPrefix = " Support Stat - ";
        protected override string GetPrefix()
        {
            return SupportPrefix + buffType.ToString().ToUpper();
        }
    }
}
