using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;

namespace CombatSystem._Core
{
    public abstract class TempoQueuePerformer <T> : ICombatTerminationListener
    {
        protected TempoQueuePerformer(Queue<T> queue)
        {
            Queue = queue;
        }

        protected TempoQueuePerformer() : this(new Queue<T>())
        { }
        [ShowInInspector] protected readonly Queue<T> Queue;

        public Queue<T> GetQueue() => Queue;


        protected CoroutineHandle _queueCoroutineHandle;
        public void EnQueueValue(in T value)
        {
            Queue.Enqueue(value);

            if(_queueCoroutineHandle.IsRunning) return;
            _queueCoroutineHandle = Timing.RunCoroutine(_DoDeQueue());
        }

        public void EnQueueValue(T value)
        {
            Queue.Enqueue(value);

            if (_queueCoroutineHandle.IsRunning) return;
            _queueCoroutineHandle = Timing.RunCoroutine(_DoDeQueue());
        }


        protected abstract IEnumerator<float> _DoDeQueue();
       
        public bool IsActing() => _queueCoroutineHandle.IsRunning;


        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
            Timing.KillCoroutines(_queueCoroutineHandle);
            Queue.Clear();
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
        }

    }
}
