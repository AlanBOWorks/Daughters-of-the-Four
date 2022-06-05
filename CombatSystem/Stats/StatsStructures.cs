using System;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    [Serializable]
    public class MonoStatsStructure<T> : TeamRolesStructure<T>, IStatsRead<T> where T : UnityEngine.Object
    {
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






        public T AttackType
        {
            get => attackType ? attackType : attackerType;
            set => attackType = value;
        }
        public T OverTimeType
        {
            get => overTimeType ? overTimeType : attackerType;
            set => overTimeType = value;
        }
        public T DeBuffType
        {
            get => deBuffType ? deBuffType : attackerType;
            set => deBuffType = value;
        }
        public T FollowUpType
        {
            get => followUpType ? followUpType : attackerType;
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
            get => healthType ? healthType : vanguardType;
            set => healthType = value;
        }
        public T MortalityType
        {
            get => mortalityType ? mortalityType : vanguardType;
            set => mortalityType = value;
        }
        public T DamageReductionType
        {
            get => damageReductionType ? damageReductionType : vanguardType;
            set => damageReductionType = value;
        }
        public T DeBuffResistanceType
        {
            get => deBuffResistanceType ? deBuffResistanceType : vanguardType;
            set => deBuffResistanceType = value;
        }


        public T ActionsType
        {
            get => actionsType ? actionsType : flexType;
            set => actionsType = value;
        }
        public T SpeedType
        {
            get => speedType ? speedType : flexType;
            set => speedType = value;
        }
        public T ControlType
        {
            get => controlType ? controlType : flexType;
            set => controlType = value;
        }
        public T CriticalType
        {
            get => criticalType ? criticalType : flexType;
            set => criticalType = value;
        }
    }

    /// <summary>
    /// Same as [<seealso cref="MonoStatsStructure{T}"/>] but with a [<see cref="PreviewFieldAttribute"/>] for
    /// Editors purposes
    /// </summary>
    [Serializable]
    public class PreviewMonoStatsStructure<T> : PreviewTeamRolesStructure<T>, IStatsRead<T> where T : UnityEngine.Object
    {

        [HorizontalGroup("Top")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Offensive"), LabelWidth(100)] private T attackType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Offensive"), LabelWidth(100)] private T overTimeType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Offensive"), LabelWidth(100)] private T deBuffType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Offensive"), LabelWidth(100)] private T followUpType;

        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Support"), LabelWidth(100)] private T healType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Support"), LabelWidth(100)] private T shieldingType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Support"), LabelWidth(100)] private T buffType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Top/Support"), LabelWidth(100)] private T receiveBuffType;

        [HorizontalGroup("Bottom")]
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T healthType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T mortalityType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T damageReductionType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Vitality"), LabelWidth(100)] private T deBuffResistanceType;

        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T actionsType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T speedType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T controlType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f), BoxGroup("Bottom/Concentration"), LabelWidth(100)] private T criticalType;



        public T AttackType
        {
            get => attackType ? attackType : attackerType;
            set => attackType = value;
        }
        public T OverTimeType
        {
            get => overTimeType ? overTimeType : attackerType;
            set => overTimeType = value;
        }
        public T DeBuffType
        {
            get => deBuffType ? deBuffType : attackerType;
            set => deBuffType = value;
        }
        public T FollowUpType
        {
            get => followUpType ? followUpType : attackerType;
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
            get => healthType ? healthType : vanguardType;
            set => healthType = value;
        }
        public T MortalityType
        {
            get => mortalityType ? mortalityType : vanguardType;
            set => mortalityType = value;
        }
        public T DamageReductionType
        {
            get => damageReductionType ? damageReductionType : vanguardType;
            set => damageReductionType = value;
        }
        public T DeBuffResistanceType
        {
            get => deBuffResistanceType ? deBuffResistanceType : vanguardType;
            set => deBuffResistanceType = value;
        }


        public T ActionsType
        {
            get => actionsType ? actionsType : flexType;
            set => actionsType = value;
        }
        public T SpeedType
        {
            get => speedType ? speedType : flexType;
            set => speedType = value;
        }
        public T ControlType
        {
            get => controlType ? controlType : flexType;
            set => controlType = value;
        }
        public T CriticalType
        {
            get => criticalType ? criticalType : flexType;
            set => criticalType = value;
        }
    }



    /// <summary>
    /// Generic for direct referencing.<br></br>
    /// [<seealso cref="SerializeReference"/>] can't be used by generics, so it's better to implement this
    /// directly by its own class
    /// </summary>
    public class SStatsHolder<T> : ScriptableObject where T : UnityEngine.Object
    {
        [SerializeField]
        private ReferenceHolder holder = new ReferenceHolder();

        public IStatsRead<T> GetHolder() => holder;
        internal ReferenceHolder GetReferences() => holder;


        [Serializable]
        internal sealed class ReferenceHolder : MonoStatsStructure<T>
        {

        }
    }
}
