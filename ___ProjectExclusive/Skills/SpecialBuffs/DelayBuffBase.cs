using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Buffs that depends of a certain ticker/tempo ticks.
    /// </summary>
    public interface IDelayBuff
    {
        TempoHandler.TickType GetTickType();
        void DoBuff(CombatingEntity user, float modifier);
        int MaxStack { get; }
    }



    /// <summary>
    /// Buff than remains waiting until the selected tick [<seealso cref="TempoHandler.TickType "/>]
    /// is called
    /// </summary>
    public class DelayBuffHandler : TempoTypeHandlerBase<StackableBuffPool>
    {
        private readonly CombatingEntity _entity;
        public DelayBuffHandler(CombatingEntity entity)
        {
            _entity = entity;
            OnBeforeSequence = new StackableBuffPool();
            OnAction = new StackableBuffPool();
            OnSequence = new StackableBuffPool();
            OnRound = new StackableBuffPool();
        }

        [ShowInInspector] protected override StackableBuffPool OnBeforeSequence { get; }
        [ShowInInspector] protected override StackableBuffPool OnAction { get; }
        [ShowInInspector] protected override StackableBuffPool OnSequence { get; }
        [ShowInInspector] protected override StackableBuffPool OnRound { get; }

        public void EnqueueBuff(IDelayBuff buff)
        {
            var handler = GetHandler(buff.GetTickType());
            if (handler.ContainsKey(buff))
            {
                float stackAmount = handler[buff];
                stackAmount++;
                if (stackAmount <= buff.MaxStack)
                    handler[buff] = stackAmount;
            }
            else
            {
                handler.Add(buff,1);
            }

        }

        protected override void DoActionOn(StackableBuffPool holder)
        {
            if (holder.Count <= 0) return;

            foreach (KeyValuePair<IDelayBuff, float> pair in holder)
            {
                pair.Key.DoBuff(_entity,pair.Value);
            }
            holder.Clear();
        }
    }

    /// <summary>
    /// <see cref="float"/> is stack amount
    /// </summary>
    public class StackableBuffPool : Dictionary<IDelayBuff,float> 
    { }

    

    /// <summary>
    /// For buff that have a special condition for need to be removed;
    /// (<example>It could have a count down or a float to check for the buff be removed)</example>
    /// </summary>
    public interface IPersistentBuff
    {
        bool BuffHasEnded();
    }
}
