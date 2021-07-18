using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class SpecialBuffHandler : ITempoFullListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            entity.SpecialBuffHolders.OnInitiativeTrigger();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            entity.SpecialBuffHolders.OnDoMoreActions();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            entity.SpecialBuffHolders.OnFinisAllActions();
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (CombatingEntity entity in allEntities)
            {
                entity.SpecialBuffHolders.OnRoundCompleted();
            }
        }
    }
}
