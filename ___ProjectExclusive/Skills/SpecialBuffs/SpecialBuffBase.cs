using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// A buff that depends of a certain ticker
    /// </summary>
    public abstract class TickerBuff
    {
        public abstract TempoHandler.TickType GetTickType();
        public abstract void DoBuff(CombatingEntity onEntity);
    }


    /// <summary>
    /// A Buff that is not activated until certain conditions passes
    /// </summary>
    public abstract class AfterBuff : TickerBuff
    {
       
    }

    //TODO implement if necessary for UntilBuff types 
    public abstract class UntilBuff : TickerBuff
    {
    }

    public class CharacterBuffHolders : TempoTypeHandlerBase<TickerBuffHolder>
    {
        private readonly CombatingEntity _entity;
        public CharacterBuffHolders(CombatingEntity entity)
        {
            _entity = entity;
            OnBeforeSequence = new TickerBuffHolder();
            OnAction = new TickerBuffHolder();
            OnSequence = new TickerBuffHolder();
            OnRound = new TickerBuffHolder();
            _buffPool = new StackableBuffPool();
        }

        [ShowInInspector] protected override TickerBuffHolder OnBeforeSequence { get; }
        [ShowInInspector] protected override TickerBuffHolder OnAction { get; }
        [ShowInInspector] protected override TickerBuffHolder OnSequence { get; }
        [ShowInInspector] protected override TickerBuffHolder OnRound { get; }

        private readonly StackableBuffPool _buffPool;

        /// <summary>
        /// Enqueue the <paramref name="buff"/> without checking if exist already; For that use
        /// the <seealso cref="HasStackableBuff"/> before instantiating a new buff to enqueue
        /// </summary>
        public void EnqueueBuff(TickerBuff buff, TempoHandler.TickType onTickType)
        {
            GetHandler(onTickType).Add(buff);
            if (buff is IStackableBuff stackableBuff)
            {
                Type buffType = buff.GetType();
                _buffPool.Add(buffType,stackableBuff);
            }
        }

        public bool HasStackableBuff(Type buffType)
        {
            return _buffPool.ContainsKey(buffType);
        }

        public void IncrementBuff(Type buffType, float amount = 1)
        {
            _buffPool[buffType].Increment(amount);
        }

        protected override void DoActionOn(TickerBuffHolder holder)
        {
            if (holder.Count <= 0) return;

            for (var i = 0; i < holder.Count; i++)
            {
                TickerBuff buff = holder[i];
                buff.DoBuff(_entity);
                if (buff is IPersistentBuff persistentBuff)
                {
                    if(persistentBuff.BuffHasEnded())
                        holder.RemoveAt(i);
                }
                else
                {
                    holder.RemoveAt(i);
                }
            }
        }
    }

    public class TickerBuffHolder : List<TickerBuff> 
    {
        
    }

    internal class StackableBuffPool : Dictionary<Type, IStackableBuff> 
    { }

    /// <summary>
    /// Buff that can be stacked if there's any on the proper holder
    /// </summary>
    public interface IStackableBuff
    {
        void Increment(float amount);
    }

    /// <summary>
    /// For buff that have a special condition for need to be removed;
    /// (<example>It could have a count down or a float to check for the buff be removed)</example>
    /// </summary>
    public interface IPersistentBuff
    {
        bool BuffHasEnded();
    }
}
