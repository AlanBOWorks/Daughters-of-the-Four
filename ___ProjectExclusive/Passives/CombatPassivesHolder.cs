using System;
using System.Collections;
using System.Collections.Generic;
using ___ProjectExclusive;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    /// <summary>
    /// Used be [<see cref="CombatingEntity"/>] to hold all <see cref="IPassiveInjector"/>
    /// </summary>
    public class CombatPassivesHolder
    {
        public CombatPassivesHolder(CombatingEntity user)
        {
            _conditionalStats = new StatPassives(user);
            EffectFilters = new EffectFiltersHolder(user);
        }

        [ShowInInspector]
        private readonly StatPassives _conditionalStats;

        public IBuffHolder<ConditionalStats> ConditionalStats
            => _conditionalStats;

        [ShowInInspector] 
        public readonly EffectFiltersHolder EffectFilters;

        public void ResetOnFinish()
        {
            _conditionalStats.GetBurst().Clear();
        }

        private class StatPassives : BuffTypeHolder<ConditionalStats>
        {
            public StatPassives(CombatingEntity user)
            {
                buffType = new ConditionalStats(user);
                burstType = new ConditionalStats(user);


                var formulatedStats = user.CombatStats.GetFormulatedStats();
                formulatedStats.Add(this);
            }
        }


    }
    
    public abstract class SPassiveInjector : ScriptableObject, IPassiveInjector
    {
        [SerializeField, PropertyOrder(-100)] 
        private string statName = "NULL";
        public string StatName => statName;

        public abstract void InjectPassive(CombatingEntity entity);


        protected virtual string AssetPrefix()
        {
            return "Passive - ";
        }
        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = AssetPrefix() + $"{statName} [Passive]";
            UtilsGame.UpdateAssetName(this);
        }
    }

}
