using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    public interface IStatDriven<T> : IStatDrivenData<T>, IStatDrivenInjection<T>
    {
    }

    public interface IStatDrivenData<out T>
    {

        T Health { get; }
        T Buff { get; }
        T Harmony { get; }
        T Tempo { get; }
        T Control { get; }
        T Stance { get; }
        T Area { get; }
    }
    public interface IStatDrivenInjection<in T>
    {

        T Health { set; }
        T Buff { set; }
        T Harmony { set; }
        T Tempo { set; }
        T Control { set; }
        T Stance { set; }
        T Area { set; }
    }

    public class StatDrivenData<T> : IStatDriven<T>
    {
        public StatDrivenData()
        {}

        public StatDrivenData(IStatDrivenData<T> copyFrom)
        {
            Health = copyFrom.Health;
            Buff = copyFrom.Buff;
            Harmony = copyFrom.Harmony;
            Tempo = copyFrom.Tempo;
            Control = copyFrom.Control;
            Stance = copyFrom.Stance;
            Area = copyFrom.Area;
        }

        public T Health { get; set; }
        public T Buff { get; set; }
        public T Harmony { get; set; }
        public T Tempo { get; set; }
        public T Control { get; set; }
        public T Stance { get; set; }
        public T Area { get; set; }
        public T BackUpElement { get; set; }

        public T GetElement(EnumSkills.StatDriven statType)
            => UtilsSkill.GetElement(this, statType);
    }

    [Serializable]
    public class SerializableStatDrivenData<T> : IStatDriven<T>
    {
        [SerializeField] protected T health;
        [SerializeField] protected T buff;
        [SerializeField] protected T harmony;
        [SerializeField] protected T tempo;
        [SerializeField] protected T control;
        [SerializeField] protected T stance;
        [SerializeField] protected T area;

        public T Health
        {
            get => health;
            set => health = value;
        }
        public T Buff
        {
            get => buff;
            set => buff = value;
        }
        public T Harmony
        {
            get => harmony;
            set => harmony = value;
        }
        public T Tempo
        {
            get => tempo;
            set => tempo = value;
        }
        public T Control
        {
            get => control;
            set => control = value;
        }
        public T Stance
        {
            get => stance;
            set => stance = value;
        }
        public T Area
        {
            get => area;
            set => area = value;
        }

        public T GetElement(EnumSkills.StatDriven statType)
            => UtilsSkill.GetElement(this, statType);
    }


    public interface ITargetDriven<T> : ITargetDrivenData<T>, ITargetDrivenInjection<T>
    { }

    public interface ITargetDrivenData<out T>
    {
        T SelfOnly { get; }
        T Offensive { get; }
        T Support { get; }
    }
    public interface ITargetDrivenInjection<in T>
    {
        T SelfOnly { set; }
        T Offensive { set; }
        T Support { set; }
    }

    public class TargetDrivenData<T> : ITargetDriven<T>
    {
        public TargetDrivenData()
        {}

        public TargetDrivenData(ITargetDrivenData<T> copyFrom)
        {
            SelfOnly = copyFrom.SelfOnly;
            Offensive = copyFrom.Offensive;
            Support = copyFrom.Support;
        }

        public T SelfOnly { get; set; }
        public T Offensive { get; set; }
        public T Support { get; set; }

        public T GetElement(EnumSkills.TargetingType targetingType)
            => UtilsSkill.GetElement(this, targetingType);
    }

    [Serializable]
    public class SerializableTargetDrivenData<T> : ITargetDriven<T>
    {
        [SerializeField] protected T selfOnly;
        [SerializeField] protected T offensive;
        [SerializeField] protected T support;

        public T SelfOnly
        {
            get => selfOnly;
            set => selfOnly = value;
        }

        public T Offensive
        {
            get => offensive;
            set => offensive = value;
        }

        public T Support
        {
            get => support;
            set => support = value;
        }

        public T GetElement(EnumSkills.TargetingType targetingType)
            => UtilsSkill.GetElement(this, targetingType);
    }


    /// <summary>
    /// An entity that contains group of [<see cref="IStatDrivenData{T}"/>] wrapped inside a
    /// [<see cref="ITargetDrivenData{T}"/>].<br></br><br></br>
    /// <example>
    /// In simpler words:<br></br>
    /// a bunch typeof(<see cref="T"/>) will be store inside a [<see cref="IStatDrivenData{T}"/>];
    /// those [<see cref="IStatDrivenData{T}"/>] will be also store in other wrapper container
    /// of [<see cref="ITargetDrivenData{T}"/>](this the bigger one in the highest level).
    /// </example>
    /// </summary>
    /// <typeparam name="T">The minimal type of use</typeparam>
    public interface IStatDrivenEntity<T> : ITargetDrivenData<IStatDriven<T>>
    {
        // Is done in this order because the higher level (ITargetDrivenData) will be always
        // instantiated(not Null) while the element could be Null perfectly.
        //
        // e.g:
        // some attacking animations could be null so its generic one could be used as a substitute,
        // while Self/Offensive/Support animation's wrappers are not only acceded always for
        // different purposes, but also different conceptually and need to be separated from each other.

        IStatDriven<T> BackUpElement { get; }
    }


    public abstract class StatDrivenEntity<T> : TargetDrivenData<IStatDriven<T>>, IStatDrivenEntity<T>
    {
        protected IStatDriven<T> backUpElement;

        /// <param name="instantiationCopy">
        /// -True:<br></br>
        /// Create new objects of [typeof(<see cref="T"/>)] and copies
        /// all the values of [<paramref name="injection"/>]<br></br>
        /// -False:<br></br>
        /// Only takes the references
        /// </param>
        protected StatDrivenEntity(IStatDrivenEntity<T> injection, bool instantiationCopy)
        {
            if (instantiationCopy)
                InstantiateCopy(injection);
            else
                InjectReferences(injection);


            void InstantiateCopy(IStatDrivenEntity<T> copyFrom)
            {
                SelfOnly = InstantiateNewStats(copyFrom.SelfOnly);
                Offensive = InstantiateNewStats(copyFrom.Offensive);
                Support = InstantiateNewStats(copyFrom.Support);
                backUpElement = InstantiateNewStats(copyFrom.BackUpElement);
            }
            void InjectReferences(IStatDrivenEntity<T> copyFrom)
            {
                SelfOnly = copyFrom.SelfOnly;
                Offensive = copyFrom.Offensive;
                Support = copyFrom.Support;
                backUpElement = copyFrom.BackUpElement;
            }
        }

        protected abstract IStatDriven<T> InstantiateNewStats(IStatDriven<T> copyFrom);
        public IStatDriven<T> BackUpElement => backUpElement;

        public T GetElement(EnumSkills.TargetingType targetingType, EnumSkills.StatDriven statType)
            => UtilsSkill.GetElement(this, targetingType, statType);
    }

    // Doesn't inherit from [TargetDrivenData<IStatDriven<T>>] since Unity can't Serialize interfaces
    [Serializable]
    public class SerializableStatDrivenEntity<T> : IStatDrivenEntity<T>
    {
        [TitleGroup("Elements")]
        [SerializeField] protected SerializableStatDrivenData<T> selfOnly;
        [TitleGroup("Elements")]
        [SerializeField] protected SerializableStatDrivenData<T> offensive;
        [TitleGroup("Elements")]
        [SerializeField] protected SerializableStatDrivenData<T> support;
        [TitleGroup("BackUp Elements")]
        [SerializeField] protected SerializableStatDrivenData<T> backUpElement;

        public IStatDriven<T> SelfOnly => selfOnly;
        public IStatDriven<T> Offensive => offensive;
        public IStatDriven<T> Support => support;

        public IStatDriven<T> BackUpElement => backUpElement;

        public T GetElement(EnumSkills.TargetingType targetingType, EnumSkills.StatDriven statType)
            => UtilsSkill.GetElement(this, targetingType, statType);


#if UNITY_EDITOR
        [Button]
        private void TestGetter(EnumSkills.TargetingType targetingType, EnumSkills.StatDriven statType)
        {
            var element = GetElement(targetingType, statType);
            Debug.Log($"Get: {element.GetType()}");
        } 
#endif
    }
}
