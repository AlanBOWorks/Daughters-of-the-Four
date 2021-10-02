using System;
using __ProjectExclusive.Player;
using UnityEngine;

namespace CombatSystem
{
    public class UCombatSingletonInjectionHandler : MonoBehaviour
    {
        private void Awake()
        {
            if(CombatSystemSingleton.InjectionHandler != null) return;

            CombatSystemSingleton.InjectionHandler = this;
            InjectPlayer();
        }

        private void Start()
        {
            Destroy(this);
        }

        private static void InjectPlayer()
        {
            CombatSystemSingleton.EntitiesFixedEvents.SubscribePlayerEvents(PlayerCombatSingleton.Events);
        }
    }
}
