using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public static class CombatFinishHandler 
    {
        public static void FinishCombat(bool isWinCombat)
        {
            if(!CombatSystemSingleton.GetIsCombatActive()) return;

            var combatCoroutine = CombatSystemSingleton.MasterCoroutineHandle;

            Timing.KillCoroutines(combatCoroutine); //safe kill
            Timing.KillCoroutines(CombatSystemSingleton.CombatCoroutineLayer);

            CombatSystemSingleton.EventsHolder.OnCombatFinish(isWinCombat);

            var aliveReference = CombatSystemSingleton.AliveGameObjectReference;
            if (aliveReference)
            {
                Object.Destroy(aliveReference);
            }

        }
    }
}
