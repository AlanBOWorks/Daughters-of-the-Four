
using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{

    [CreateAssetMenu(fileName = "N - Harmony PASSIVE - [Preset]",
        menuName = "Combat/Passive/Preset/Harmony Passive")]
    public class SHarmonyPassive : ScriptableObject
    {
        [SerializeField] 
        private string passiveName = "NULL";

        [InfoBox("Effects apply in tiers; +- > x0,x1,x2,x3")]
        [SerializeField] 
        private EffectParams[] onPositiveHarmony;
        [SerializeField] 
        private EffectParams[] onNegative;
        [SerializeField] 
        private FilterPassivesHolder onPositivePassives = new FilterPassivesHolder();

        [SerializeField]
        private FilterPassivesHolder onNegativePassives = new FilterPassivesHolder();

        public void DoOnPositiveHarmonyEffects(CombatingEntity user,CombatingEntity target, float modifier)
            => DoPassives(onPositiveHarmony,user, target, modifier);

        public void DoOnNegativeHarmonyEffects(CombatingEntity user, CombatingEntity target, float modifier)
            => DoPassives(onNegative, user, target, modifier);

        public FilterPassivesHolder OnPositivePassives => onPositivePassives;
        public FilterPassivesHolder OnNegativePassives => onNegativePassives;


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
    
    public class HarmonyBuffInvoker : IPassivesFilterHandler, ITemporalStatsChangeListener
    {
        private readonly CombatingEntity _entity;
        public readonly SHarmonyPassive Passive;
        public readonly FilterPassivesHolder OnPositivePassives;
        public readonly FilterPassivesHolder OnNegativePassives;

        public HarmonyBuffInvoker(CombatingEntity user,SHarmonyPassive passive)
        {
            Passive = passive;
            _entity = user;
            OnPositivePassives = passive.OnPositivePassives;
            OnNegativePassives = passive.OnNegativePassives;

            _entity.Events.Subscribe(this);
        }

        private HarmonyTiers _currentTier;
        private float _currentTierModifier;
        private bool _isPositiveTier;
        public enum HarmonyTiers
        {
            Neutral = 0,
            FirstPositive = FirstTierIndex,
            SecondPositive = SecondTierIndex,
            FinalPositive = FinalTierIndex
        }


        private const int FirstTierIndex = 1;
        private const float FirstTier = .30f;
        private const int SecondTierIndex = FirstTierIndex + 1;
        private const float SecondTier = .60f;
        private const int FinalTierIndex = SecondTierIndex + 1;
        private const float FinalTier = .90f;

        
        public void InvokeBurstStats()
        {
            if(_currentTier == HarmonyTiers.Neutral) return;

            Action<CombatingEntity, CombatingEntity, float> invokeHarmony;
            if (_isPositiveTier)
            {
                invokeHarmony = Passive.DoOnPositiveHarmonyEffects;
            }
            else
            {
                invokeHarmony = Passive.DoOnNegativeHarmonyEffects;
            }
            invokeHarmony(_entity, _entity, _currentTierModifier);
        }

        public void DoActionPassiveFilter(ref EffectArguments arguments, ref float currentValue, float originalValue)
        {
            if(_currentTier == HarmonyTiers.Neutral) return;

            var passives = (_isPositiveTier)
                ? OnPositivePassives.ActionFilterPassives
                : OnNegativePassives.ActionFilterPassives;
            foreach (SActionPassiveFilterPreset passive in passives)
            {
                passive.DoPassiveFilter(ref arguments,ref originalValue,originalValue, _currentTierModifier);
            }
        }

        public void DoReActionPassiveFilter(ref EffectArguments arguments, ref float currentValue, float originalValue)
        {
            if(_currentTier == HarmonyTiers.Neutral) return;
            var passives = (_isPositiveTier)
                ? OnPositivePassives.ReactionFilterPassives
                : OnNegativePassives.ReactionFilterPassives;
            foreach (SReactionPassiveFilterPreset passive in passives)
            {
                passive.DoPassiveFilter(ref arguments, ref originalValue, originalValue, -_currentTierModifier);
            }
        }

        public void OnTemporalStatsChange(ICombatTemporalStats currentStats)
        {
            DoCheck();
            _currentTierModifier = (float)_currentTier;

            void DoCheck()
            {
                float currentHarmony = currentStats.HarmonyAmount;
                if (currentHarmony > 0)
                {
                }
                else
                {
                    currentHarmony = -currentHarmony;
                    _isPositiveTier = false;
                    // if is negative, convert to positive so the tier check is the same,
                    // but keep the boolean for later
                }

                if (currentHarmony < FirstTier)
                {
                    _currentTier = HarmonyTiers.Neutral;
                    return;
                }

                if (currentHarmony < SecondTier)
                {
                    _currentTier = HarmonyTiers.FirstPositive;
                    return;
                }

                if (currentHarmony < FinalTier)
                {
                    _currentTier = HarmonyTiers.SecondPositive;
                    return;
                }

                _currentTier = HarmonyTiers.FinalPositive;
            }
        }
    }
}
