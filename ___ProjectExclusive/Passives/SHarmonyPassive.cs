
using System;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace Passives
{
    public abstract class SHarmonyPassive : SInjectionPassiveBase
    {
        private const float FirstTier = 30;
        private const float SecondTier = 60;
        private const float FinalTier = 90;
        public override void InjectPassive(CombatingEntity target)
        {
            float currentHarmony = target.CombatStats.HarmonyAmount;
            bool isNegative = currentHarmony < 0;
            Action<float> invokeHarmony;
            if (isNegative)
            {
                currentHarmony = -currentHarmony;
                // if is negative, convert to positive so the tier check is the same,
                // but keep the boolean for later
                invokeHarmony = OnNegativeHarmony;
            }
            else
            {
                invokeHarmony = OnPositiveHarmony;
            }

            if (currentHarmony < FinalTier)
            {
                return;
            }
            if (currentHarmony < SecondTier)
            {
                invokeHarmony(1);
                return;
            }
            if (currentHarmony < FinalTier)
            {
                invokeHarmony(2);
                return;
            }

            invokeHarmony(3);

        }

        public abstract void OnPositiveHarmony(float modifier);
        public abstract void OnNegativeHarmony(float modifier);
    }

    public class HarmonyBuffInvoker
    {
        private readonly CombatingEntity _entity;
        private readonly SHarmonyPassive _passive;

        public HarmonyBuffInvoker(CombatingEntity user,SHarmonyPassive passive)
        {
            _entity = user;
            _passive = passive;
        }

        public void DoHarmonyCheck()
        {
            _passive.InjectPassive(_entity);
        }
    }
}
