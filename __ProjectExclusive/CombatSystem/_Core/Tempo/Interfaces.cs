
using System.Collections.Generic;
using CombatEntity;

namespace CombatSystem
{


    public interface ITempoTickListener
    {
        void OnTickStep(float seconds);
    }

    public interface IEntityTickListener
    {

        void OnTickEntity(CombatingEntity entity, float currentTickAmount);
        void RoundAmountInjection(int triggerAmount);
        void OnRoundTick(int currentTickCount);
    }
}