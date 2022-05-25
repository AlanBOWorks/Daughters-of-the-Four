using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public static class CombatFinishHandler 
    {
        public static void FinishCombat(bool isWinCombat)
        {
            var combatCoroutine = CombatSystemSingleton.MasterCoroutineHandle;
            if(!combatCoroutine.IsRunning) return;

            Timing.KillCoroutines(combatCoroutine); //safe kill
            Timing.KillCoroutines(CombatSystemSingleton.CombatCoroutineLayer);

            CombatSystemSingleton.EventsHolder.OnCombatFinish(isWinCombat);
        }
    }
}
