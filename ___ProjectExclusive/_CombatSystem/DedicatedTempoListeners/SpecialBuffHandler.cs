using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class SpecialBuffHandler : ITempoFullListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            entity.TickerBuffHolders.OnInitiativeTrigger();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            entity.TickerBuffHolders.OnDoMoreActions();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            entity.TickerBuffHolders.OnFinisAllActions();
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (CombatingEntity entity in allEntities)
            {
                entity.TickerBuffHolders.OnRoundCompleted();
            }
        }
    }
}
