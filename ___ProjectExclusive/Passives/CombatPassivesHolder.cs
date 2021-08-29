using System;
using System.Collections;
using System.Collections.Generic;
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
           ConditionalStats = new StatPassives(user);
        }

        [ShowInInspector]
        public readonly IBuffHolder<ConditionalStats> ConditionalStats;

        private class StatPassives : BuffTypeHolder<ConditionalStats>
        {
            public StatPassives(CombatingEntity user)
            {
                buffType = new ConditionalStats(user);
                burstType = new ConditionalStats(user);

                user.Injection(buffType);
            }
        }
    }
    
    public abstract class SPassiveInjector : ScriptableObject, IPassiveInjector
    {
        public abstract void InjectPassive(CombatingEntity entity);
    }

}
