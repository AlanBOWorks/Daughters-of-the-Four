using System.Collections.Generic;
using CombatSystem.Entity;
using DataHolders;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    public sealed class CombatEntityVitalityTracker : IDamageHandler
    {
        public CombatEntityVitalityTracker()
        {
            _perAction = new VitalityTrackerValues();
            _perSequence = new VitalityTrackerValues();

        }

        [ShowInInspector]
        private readonly VitalityTrackerValues _perAction;
        [ShowInInspector]
        private readonly VitalityTrackerValues _perSequence;


        public void DoShields(CombatEntity entity, float amount)
        {
            _perAction.DoShields(entity, amount);
            _perSequence.DoShields(entity,amount);
        }

        public void DoHealth(CombatEntity entity, float amount)
        {
            _perAction.DoHealth(entity, amount);
            _perSequence.DoHealth(entity, amount);
        }

        public void DoMortality(in CombatEntity entity, in float amount)
        {
            _perAction.DoMortality(in entity, in amount);
            _perSequence.DoMortality(in entity, in amount);
        }


        public void ResetOnActionValues() => _perAction.Reset();
        public void ResetOnSequenceValues() => _perSequence.Reset();
    }

    public sealed class VitalityTrackerValues : IDamageHandler
    {
        [ShowInInspector]
        private Dictionary<CombatEntity, VitalityValuesTracker> _dictionary;
        


        private void HandleKey(in CombatEntity entity)
        {
            if (_dictionary == null)
                _dictionary = new Dictionary<CombatEntity, VitalityValuesTracker>();


            if (_dictionary.ContainsKey(entity)) return;
            _dictionary.Add(entity, new VitalityValuesTracker());
        }

        public void DoShields(CombatEntity entity, float amount)
        {
            HandleKey(in entity);
            var currentValues = _dictionary[entity];
            currentValues.InteractShields(amount);
        }
        public void DoHealth(CombatEntity entity, float amount)
        {
            HandleKey(in entity);
            var currentValues = _dictionary[entity];
            currentValues.InteractHealth(amount);
        }

        public void DoMortality(in CombatEntity entity, in float amount)
        {
            HandleKey(in entity);
            var currentValues = _dictionary[entity];
            currentValues.InteractMortality(amount);
        }


        public VitalityValues<float> GetValues(in CombatEntity entity)
        {
            return _dictionary.ContainsKey(entity) 
                ? _dictionary[entity].GenerateValues()
                : new VitalityValues<float>();
        }

        public IVitalityValues<float> GetReference(in CombatEntity entity)
        {
            return _dictionary.ContainsKey(entity)
                ? _dictionary[entity]
                : null;
        }

        public VitalityValues<float> GetCurrentAccumulation()
        {
            var accumulation = new VitalityValues<float>();
            foreach (var values in _dictionary)
            {
                IVitalityValues<float> dealtDamage = values.Value;
                accumulation.CurrentShields += dealtDamage.ShieldsValue;
                accumulation.CurrentHealth += dealtDamage.HealthValue;
                accumulation.CurrentMortality += dealtDamage.MortalityValue;
            }

            return accumulation;
        }

        public void Reset()
        {
            if(_dictionary == null) return;

            foreach (var pair in _dictionary)
            {
                pair.Value.Reset();
            }
        }

        public void Clear()
        {
            _dictionary?.Clear();
        }

        [Button]
        private void DebugLog()
        {
            foreach (var pair in _dictionary)
            {
               Debug.Log($"{pair.Key.GetProviderEntityName()} > HD: {pair.Value.HealthValue} "); 
            }
        }

        private sealed class VitalityValuesTracker : IVitalityValues<float>
        {

            public VitalityValuesTracker()
            {
                _countableValuesHolder = new ValuesHolder();
            }

            [ShowInInspector] 
            private readonly IVitalityValues<CountableValue> _countableValuesHolder;

            public float ShieldsValue => _countableValuesHolder.ShieldsValue.Value;
            public float HealthValue => _countableValuesHolder.HealthValue.Value;
            public float MortalityValue => _countableValuesHolder.ShieldsValue.Value;

            public void InteractShields(float shieldsVariation)
            {
                _countableValuesHolder.ShieldsValue.Interact(in shieldsVariation);
            }
            public void InteractHealth(float healthVariation)
            {
                _countableValuesHolder.HealthValue.Interact(in healthVariation);
            }

            public void InteractMortality(float mortalityVariation)
            {
                _countableValuesHolder.MortalityValue.Interact(in mortalityVariation);
            }


            public VitalityValues<float> GenerateValues() => new VitalityValues<float>(this);



            public void Reset()
            {
                _countableValuesHolder.HealthValue.Reset();
                _countableValuesHolder.ShieldsValue.Reset();
                _countableValuesHolder.MortalityValue.Reset();
            }

            private sealed class ValuesHolder : IVitalityValues<CountableValue>
            {
                public ValuesHolder()
                {
                    ShieldsValue = new CountableValue();
                    HealthValue = new CountableValue();
                    MortalityValue = new CountableValue();
                }
                [ShowInInspector]
                public CountableValue ShieldsValue { get; }
                [ShowInInspector]
                public CountableValue HealthValue { get; }
                [ShowInInspector]
                public CountableValue MortalityValue { get; }
            }
        }
    }

    public struct VitalityValues<T> : IDamageableStats<T>, IVitalityValues<T>
    {
        public T Shields;
        public T Health;
        public T Mortality;

        public VitalityValues(T shields, T health, T mortality)
        {
            Shields = shields;
            Health = health;
            Mortality = mortality;
        }

        public VitalityValues(IVitalityValues<T> values) 
            : this(values.ShieldsValue, values.HealthValue, values.MortalityValue)
        {
            
        }

        public T CurrentHealth
        {
            get => Health;
            set => Health = value;
        }

        public T CurrentMortality
        {
            get => Mortality;
            set => Mortality = value;
        }

        public T CurrentShields
        {
            get => Shields;
            set => Shields = value;
        }


        public T ShieldsValue => Shields;
        public T HealthValue => Health;
        public T MortalityValue => Mortality;
    }



    public interface IVitalityValues<out T>
    {
        T ShieldsValue { get; }
        T HealthValue { get; }
        T MortalityValue { get; }
    }


    internal interface IDamageHandler
    {
        void DoShields(CombatEntity entity, float amount);
        void DoHealth(CombatEntity entity, float amount);
        void DoMortality(in CombatEntity entity, in float amount);
    }
}
