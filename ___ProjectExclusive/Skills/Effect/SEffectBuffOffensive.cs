using System;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Offensive Buff Effect - N [Preset]",
        menuName = "Combat/Effects/Offensive Buff")]
    public class SEffectBuffOffensive : SEffectBuffBase
    {
        [SerializeField] private BuffType buffType = BuffType.AttackPower;

        public enum BuffType
        {
            AttackPower,
            DeBuffPower
        }

        protected override void DoBuff(CombatingEntity user, ICharacterBasicStats buffStats, float effectModifier)
        {
            effectModifier *= user.CombatStats.BuffPower;
            switch (buffType)
            {
                case BuffType.AttackPower:
                    buffStats.AttackPower += effectModifier;
                    break;
                case BuffType.DeBuffPower:
                    buffStats.DeBuffPower += effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }

        }

    }
}
