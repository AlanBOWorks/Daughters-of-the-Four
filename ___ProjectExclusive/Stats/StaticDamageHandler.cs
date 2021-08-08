using _CombatSystem;
using Characters;
using UnityEngine;

namespace Stats
{
    public class StaticDamageHandler : ITempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            float damage = entity.CombatStats.AccumulatedStaticDamage;
            if(damage <= 0) return;

            UtilsCombatStats.DoDamageTo(entity,damage);
        }
    }
}
