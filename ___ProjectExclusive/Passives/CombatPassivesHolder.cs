using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Passives
{
    public class CombatPassivesHolder
    {
        public CombatPassivesHolder(CombatingEntity user, IEnumerable<IPassiveBase> passives)
        {
            AllPassives = new List<IPassiveBase>(passives);
            DoPassives(user,passives);
        }

        public CombatPassivesHolder(CombatingEntity user, ICollection<IPassiveBase> passives)
        {
            AllPassives = new List<IPassiveBase>(passives.Count);
            foreach (IPassiveBase passive in passives)
            {
                AllPassives.Add(passive);
                passive.InjectPassive(user);
            }
        }
        
        public readonly List<IPassiveBase> AllPassives;

        private static void DoPassives(CombatingEntity user, IEnumerable<IPassiveBase> passives)
        {
            foreach (IPassiveBase passive in passives)
            {
                passive.InjectPassive(user);
            }
        }
    }




    [Serializable]
    public abstract class PassiveInjectorsHolder<T>
    {
        [SerializeField] protected T[] passives = new T[0];
        protected abstract void DoInjection(CombatingEntity entity, T element);
        public void InjectPassive(CombatingEntity entity)
        {
            for (var i = 0; i < passives.Length; i++)
            {
                T passive = passives[i];
                DoInjection(entity, passive);
            }
        }
    }


    public abstract class PassiveInjector<T> : ScriptableObject, IPassiveBase
    {
        public void InjectPassive(CombatingEntity entity)
        {
            GetHolder().InjectPassive(entity);
        }

        protected abstract PassiveInjectorsHolder<T> GetHolder();
    }

    public abstract class SPassiveInjector<T> : ScriptableObject, IPassiveBase
    {
        public void InjectPassive(CombatingEntity entity)
        {
            GetHolder().InjectPassive(entity);
        }

        protected abstract PassiveInjectorsHolder<T> GetHolder();
    }

}
