using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public abstract class TempoTypeHandlerBase<T> : ITempoListener, IRoundListener
    //,ITempoTypes<T> <<<< uncheck for placeHolding updates
    {
        protected abstract T OnBeforeSequence { get; }
        protected abstract T OnAction { get; }
        protected abstract T OnSequence { get; }
        protected abstract T OnRound { get; }

        public T GetHandler(TempoHandler.TickType type)
        {
            switch (type)
            {
                case TempoHandler.TickType.OnBeforeSequence:
                    return OnBeforeSequence;
                case TempoHandler.TickType.OnAction:
                    return OnAction;
                case TempoHandler.TickType.OnAfterSequence:
                    return OnSequence;
                case TempoHandler.TickType.OnRound:
                    return OnRound;

                default:
                    throw new ArgumentException($"Invalid type: {type}",
                        new NotImplementedException("Type could be not implemented or it's just an error on " +
                                                    $"the setup of the buff/holder target type"));
            }
        }
        
        protected abstract void DoActionOn(T onHandler);

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            DoActionOn(OnBeforeSequence);
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            DoActionOn(OnAction);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            DoActionOn(OnSequence);
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            DoActionOn(OnRound);
        }
    }
}
