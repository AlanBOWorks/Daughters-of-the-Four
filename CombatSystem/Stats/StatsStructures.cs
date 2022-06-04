using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    [Serializable]
    public class MonoStatsStructure<T> : IStatsRead<T> where T : UnityEngine.Object
    {
        [SerializeField] private T offensiveType;
        [SerializeField] private T supportType;
        [SerializeField] private T vitalityType;
        [SerializeField] private T concentrationType;

        [HorizontalGroup("Top")]
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] private T attackType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] private T overTimeType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] private T deBuffType;
        [SerializeField, BoxGroup("Top/Offensive"), LabelWidth(100)] private T followUpType;

        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] private T healType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] private T shieldingType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] private T buffType;
        [SerializeField, BoxGroup("Top/Support"), LabelWidth(100)] private T receiveBuffType;

        [HorizontalGroup("Bottom")]
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T healthType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T mortalityType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T damageReductionType;
        [SerializeField, BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T deBuffResistanceType;

        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T actionsType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T speedType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T controlType;
        [SerializeField, BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T criticalType;
        


        public T OffensiveType => offensiveType;
        public T SupportType => supportType;
        public T VitalityType => vitalityType;
        public T ConcentrationType => concentrationType;


        public T AttackType
        {
            get => attackType ? attackType : offensiveType;
            set => attackType = value;
        }
        public T OverTimeType
        {
            get => overTimeType ? overTimeType : offensiveType;
            set => overTimeType = value;
        }
        public T DeBuffType
        {
            get => deBuffType ? deBuffType : offensiveType;
            set => deBuffType = value;
        }
        public T FollowUpType
        {
            get => followUpType ? followUpType : offensiveType;
            set => followUpType = value;
        }


        public T HealType
        {
            get => healType ? healType : supportType;
            set => healType = value;
        }
        public T ShieldingType
        {
            get => shieldingType ? shieldingType : supportType;
            set => shieldingType = value;
        }
        public T BuffType
        {
            get => buffType ? buffType : supportType;
            set => buffType = value;
        }
        public T ReceiveBuffType
        {
            get => receiveBuffType ? receiveBuffType : supportType;
            set => receiveBuffType = value;
        }


        public T HealthType
        {
            get => healthType ? healthType : vitalityType;
            set => healthType = value;
        }
        public T MortalityType
        {
            get => mortalityType ? mortalityType : vitalityType;
            set => mortalityType = value;
        }
        public T DamageReductionType
        {
            get => damageReductionType ? damageReductionType : vitalityType;
            set => damageReductionType = value;
        }
        public T DeBuffResistanceType
        {
            get => deBuffResistanceType ? deBuffResistanceType : vitalityType;
            set => deBuffResistanceType = value;
        }


        public T ActionsType
        {
            get => actionsType ? actionsType : concentrationType;
            set => actionsType = value;
        }
        public T SpeedType
        {
            get => speedType ? speedType : concentrationType;
            set => speedType = value;
        }
        public T ControlType
        {
            get => controlType ? controlType : concentrationType;
            set => controlType = value;
        }
        public T CriticalType
        {
            get => criticalType ? criticalType : concentrationType;
            set => criticalType = value;
        }
    }
}
