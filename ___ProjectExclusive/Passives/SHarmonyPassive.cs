
using System;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{

    [CreateAssetMenu(fileName = "N - Harmony PASSIVE - [Preset]",
        menuName = "Combat/Passive/Preset/Harmony Passive")]
    public class SHarmonyPassive : ScriptableObject
    {
        [SerializeField] 
        private string passiveName = "NULL";

        [SerializeField] private PassiveParams[] onPositiveHarmony;
        [SerializeField] private PassiveParams[] onNegative;

        public void OnPositiveHarmony(CombatingEntity target, float modifier)
            => DoPassives(onPositiveHarmony, target, modifier);

        public void OnNegativeHarmony(CombatingEntity target, float modifier)
            => DoPassives(onNegative, target, modifier);

        private static void DoPassives(PassiveParams[] passives, CombatingEntity target, float modifier)
        {
            if(passives.Length < 1) return;
            foreach (PassiveParams passive in passives)
            {
                passive.InjectPassive(target,modifier);
            }
        }

        private const string HarmonyPassivePrefix = " - Harmony PASSIVE - [Preset]";
        [Button]
        private void UpdateAssetName()
        {
            name = passiveName + HarmonyPassivePrefix;
            UtilsGame.UpdateAssetName(this);
        }

        [Serializable]
        private class PassiveParams
        {
            public SPassiveEffectInjection passive;
            [SuffixLabel("%00"), Range(0,10)]
            public float passiveValue = 1;

            public void InjectPassive(CombatingEntity entity, float modifier)
            {
                passive.InjectPassive(entity,modifier * passiveValue);
            }
        }
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

        private const float FirstTier = .30f;
        private const float SecondTier = .60f;
        private const float FinalTier = .90f;
        public void DoHarmonyCheck()
        {
            var target = _entity;
            float currentHarmony = target.CombatStats.HarmonyAmount;
            Action<CombatingEntity, float> invokeHarmony;
            if (currentHarmony > 0)
            {
                invokeHarmony = _passive.OnPositiveHarmony;
            }
            else
            {
                invokeHarmony = _passive.OnNegativeHarmony;
                currentHarmony = -currentHarmony;
                // if is negative, convert to positive so the tier check is the same,
                // but keep the boolean for later
            }

            if (currentHarmony < FirstTier)
            {
                return;
            }
            if (currentHarmony < SecondTier)
            {
                invokeHarmony(target, 1);
                return;
            }
            if (currentHarmony < FinalTier)
            {
                invokeHarmony(target, 2);
                return;
            }

            invokeHarmony(target, 3);
        }
    }
}
