using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Stats
{
    
    // TODO change name without DATA
    public class StatDrivenData<T> : IStatDriven<T>
    {
        public StatDrivenData()
        {}

        public StatDrivenData(IStatDrivenData<T> copyFrom)
        {
            Health = copyFrom.Health;
            Static = copyFrom.Static;
            Buff = copyFrom.Buff;
            Harmony = copyFrom.Harmony;
            Tempo = copyFrom.Tempo;
            Control = copyFrom.Control;
            Stance = copyFrom.Stance;
            Area = copyFrom.Area;
        }

        public T Health { get; set; }
        public T Static { get; set; }
        public T Buff { get; set; }
        public T Harmony { get; set; }
        public T Tempo { get; set; }
        public T Control { get; set; }
        public T Stance { get; set; }
        public T Area { get; set; }
        public T BackUpElement { get; set; }

        public T GetElement(EnumSkills.StatDriven statType)
            => UtilsEnumStats.GetElement(this, statType);
    }

    [Serializable]
    public class SerializableStatDrivenData<T> : IStatDriven<T>
    {
        [Title("Elements")]
        [SerializeField] protected T health;
        [SerializeField] protected T _static;
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

        public T Static
        {
            get => _static;
            set => _static = value;
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
            => UtilsEnumStats.GetElement(this, statType);
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
            => UtilsEnumStats.GetElement(this, targetingType);
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
            => UtilsEnumStats.GetElement(this, targetingType);
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

    [Serializable]
    public class SerializableFullDrivenData<T> : ITargetDrivenData<IStatDrivenFull<T>>, ITargetDrivenExtension<T>
    {
        [SerializeField] private StatDrivenHolder selfOnlyHolder = new StatDrivenHolder();
        [SerializeField] private StatDrivenHolder offensiveHolder = new StatDrivenHolder();
        [SerializeField] private StatDrivenHolder supportHolder = new StatDrivenHolder();
        
        public IStatDrivenFull<T> SelfOnly => selfOnlyHolder;
        public IStatDrivenFull<T> Offensive => offensiveHolder;
        public IStatDrivenFull<T> Support => supportHolder;

        [Serializable]
        private class StatDrivenHolder : SerializableStatDrivenData<T>, IStatDrivenFull<T>
        {
            [SerializeField, Title("Holder"), PropertyOrder(-10)]
            public T elementHolder;

            public T ElementHolder => elementHolder;
        }

        public T SelfOnlyElement
        {
            get => selfOnlyHolder.elementHolder;
            set => selfOnlyHolder.elementHolder = value;
        }
        public T OffensiveElement
        {
            get => offensiveHolder.elementHolder;
            set => offensiveHolder.elementHolder = value;
        }
        public T SupportElement
        {
            get => supportHolder.elementHolder;
            set => supportHolder.elementHolder = value;
        }

        public T GetElement(EnumSkills.TargetingType targetingType, EnumSkills.StatDriven stat)
        {
            var holder = UtilsEnumStats.GetElement(this, targetingType);
            return GetElement();

            T GetElement()
            {
                var element = UtilsEnumStats.GetElement(holder, stat);
                return element ?? holder.ElementHolder;
            }
        }

        public T GetHolderElement(EnumSkills.TargetingType targetingType)
        {
            var holder = UtilsEnumStats.GetElement(this, targetingType);
            return holder.ElementHolder;
        }
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
            => UtilsEnumStats.GetElement(this, targetingType, statType);


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
