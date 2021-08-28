using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Passives
{
    /// <summary>
    /// Used be [<see cref="CombatingEntity"/>] to hold all <see cref="IPassiveInjector"/>
    /// </summary>
    public class CombatPassivesHolder
    {
        public CombatPassivesHolder(CombatingEntity user, ICollection<IPassiveInjector> passives)
        {
            AllPassives = new List<IPassiveInjector>(passives.Count);
            foreach (IPassiveInjector passive in passives)
            {
                AllPassives.Add(passive);
                passive.InjectPassive(user);
            }
        }
        
        public readonly List<IPassiveInjector> AllPassives;

    }

    /// <summary>
    /// <inheritdoc cref="SPassiveInjector{T}"/>
    /// <br></br><br></br>
    /// Base for implementing [<see cref="IPassiveInjector"/>]; It's prefer to use
    /// [<see cref="SPassiveInjector{T}"/>] unless there's a more specific type of implementation;
    /// </summary>
    public abstract class SPassiveInjectorBase : ScriptableObject, IPassiveInjector
    {
        public abstract void InjectPassive(CombatingEntity entity);
    }

    /// <summary>
    /// Does the job of injecting the passive into the [<seealso cref="CombatingEntity"/>].
    /// </summary>
    public abstract class SPassiveInjector<T> : SPassiveInjectorBase
    {
        [SerializeField] protected T[] passives = new T[0];
        protected abstract void DoInjection(CombatingEntity entity, T element);
        public override void InjectPassive(CombatingEntity entity)
        {
            for (var i = 0; i < passives.Length; i++)
            {
                T passive = passives[i];
                DoInjection(entity, passive);
            }
        }
    }
}
