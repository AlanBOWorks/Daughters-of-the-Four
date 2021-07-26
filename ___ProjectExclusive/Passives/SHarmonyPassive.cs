
using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
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

        [SerializeField] private EffectParams[] onPositiveHarmony;
        [SerializeField] private EffectParams[] onNegative;

        public void OnPositiveHarmony(CombatingEntity user,CombatingEntity target, float modifier)
            => DoPassives(onPositiveHarmony,user, target, modifier);

        public void OnNegativeHarmony(CombatingEntity user, CombatingEntity target, float modifier)
            => DoPassives(onNegative, user, target, modifier);

        private static void DoPassives(EffectParams[] passives, CombatingEntity user, CombatingEntity target, float modifier)
        {
            if(passives.Length < 1) return;
            foreach (EffectParams passive in passives)
            {
                passive.DoEffect(user,target,modifier);
            }
        }

        private const string HarmonyPassivePrefix = " - Harmony PASSIVE - [Preset]";
        [Button]
        private void UpdateAssetName()
        {
            name = passiveName + HarmonyPassivePrefix;
            UtilsGame.UpdateAssetName(this);
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
            Action<CombatingEntity, CombatingEntity, float> invokeHarmony;
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
                invokeHarmony(_entity,target, 1);
                return;
            }
            if (currentHarmony < FinalTier)
            {
                invokeHarmony(_entity, target, 2);
                return;
            }

            invokeHarmony(_entity, target, 3);
        }
    }
}
