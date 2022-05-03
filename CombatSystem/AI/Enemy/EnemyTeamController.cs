using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyTeamControllerRandom : CombatTeamControllerBase, ITempoTeamStatesListener
    {
        public void OnTempoStartControl(in CombatTeamControllerBase controller, in CombatEntity firstEntity)
        {
            throw new System.NotImplementedException();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            throw new System.NotImplementedException();
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            throw new System.NotImplementedException();
        }
    }
}
