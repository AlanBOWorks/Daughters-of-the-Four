using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class TempoFixedEvents : ITempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            entity.HarmonyBuffInvoker?.DoHarmonyCheck();
            entity.Events.OnInitiativeTrigger();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            entity.Events.OnDoMoreActions();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            entity.Events.OnFinisAllActions();
        }
    }
}
