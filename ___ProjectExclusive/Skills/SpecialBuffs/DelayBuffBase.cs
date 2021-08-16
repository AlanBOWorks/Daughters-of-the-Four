﻿using System;
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
            holder.InvokeAndClear(_entity);
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

                this[i] = new StackableBuffValues(buffValues);
                return;
            }
            Add(new StackableBuffValues(buff,buffer));
        }

        public void Add(ref StackableBuffValues values)
        {
            for (var i = 0; i < Count; i++)
            {
                StackableBuffValues buff = this[i];
                if (buff.Buff != values.Buff || buff.Buffer != values.Buffer) continue;

                this[i] = new StackableBuffValues(buff);
                return;
            }

            Add(values);
        }

        public void InvokeAndClear(CombatingEntity target)
        {
            if (Count <= 0) return;

            for (int i = Count - 1; i >= 0; i--)
            {
                var buff = this[i];
                RemoveAt(i);
                buff.Buff.DoBuff(buff.Buffer, target,  buff.StackAmount);

            }
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

        public StackableBuffValues(IDelayBuff buff, CombatingEntity buffer)
        : this(buff,buffer,1)
        {}

        public StackableBuffValues(StackableBuffValues copyFrom, float stackAddition = 1)
        {
            Buff = copyFrom.Buff;
            Buffer = copyFrom.Buffer;
            StackAmount = copyFrom.StackAmount + stackAddition;
        }
    }
}