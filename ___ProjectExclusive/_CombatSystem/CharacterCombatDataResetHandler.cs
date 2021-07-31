using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class CharacterCombatDataResetHandler : ITempoListener, ISkippedTempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            entity.CombatStats.BurstStats.OverrideAll(0);
            entity.ReceivedStats.ResetToZero();
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            entity.CombatStats.BurstStats.OverrideAll(0);
            entity.ReceivedStats.ResetToZero();
        }
    }
}
