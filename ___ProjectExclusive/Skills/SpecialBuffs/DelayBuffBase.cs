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
        void DoBuff(CombatingEntity user, CombatingEntity target, float stacks);
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

        public void EnqueueBuff(IDelayBuff buff, CombatingEntity buffer)
        {
            var handler = GetHandler(buff.GetTickType());
            handler.Add(buff,buffer);

        }

        protected override void DoActionOn(StackableBuffPool holder)
        {
            if (holder.Count <= 0) return;

            for (int i = holder.Count - 1; i >= 0; i--)
            {
                var values = holder[i];
                values.Buff.DoBuff(values.Buffer,_entity,values.StackAmount);
                holder.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// <see cref="float"/> is stack amount
    /// </summary>
    public class StackableBuffPool : List<StackableBuffValues>
    {
        public void Add(IDelayBuff buff,CombatingEntity buffer)
        {
            for (var i = 0; i < this.Count; i++)
            {
                StackableBuffValues buffValues = this[i];
                if (buff != buffValues.Buff || buffer != buffValues.Buffer) continue;

                this[i] = new StackableBuffValues(buff, buffer, buffValues.StackAmount+1);
                return;
            }
            Add(new StackableBuffValues(buff,buffer,1));
        }
    }

    public struct StackableBuffValues
    {
        public readonly IDelayBuff Buff;
        public readonly CombatingEntity Buffer;
        public readonly float StackAmount;

        public StackableBuffValues(IDelayBuff buff, CombatingEntity buffer, float stackAmount)
        {
            Buff = buff;
            Buffer = buffer;
            StackAmount = stackAmount;
        }
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
