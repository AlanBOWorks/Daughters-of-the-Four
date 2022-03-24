using System;
using UnityEngine;

namespace CombatSystem._Core
{
    public abstract class UCombatEventsSubscriber : MonoBehaviour, ICombatEventListener
    {
        protected abstract ICombatEventsHolder GetEventsHolder();

        protected virtual void Start()
        {
            var holder = GetEventsHolder();
            holder.Subscribe(this);
        }

        protected void OnDestroy()
        {
            var holder = GetEventsHolder();
            holder.UnSubscribe(this);
        }
    }
}
