using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class CombatStatesHandler : ICombatPreparationListener
    {
        public CombatStatesHandler(SystemCombatEventsHolder eventsHolder)
        {
            _eventsHolder = eventsHolder;
        }

        private readonly SystemCombatEventsHolder _eventsHolder;


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            DestroyAliveReference();
            CreateAliveReference();
            _eventsHolder.OnCombatPrepares(allMembers, playerTeam, enemyTeam);

            //todo animations for starting fight

            _eventsHolder.OnCombatStart();
        }

        public void OnCombatStops()
        {
            DestroyAliveReference();
        }


        private static void CreateAliveReference()
        {
            var aliveReference = new GameObject("-- Combat System Alive Reference --");
            CombatSystemSingleton.AliveGameObjectReference = aliveReference;
            CombatSystemSingleton.MasterCoroutineHandle = Timing.RunCoroutine(_TrackAlive(), Segment.RealtimeUpdate);


            IEnumerator<float> _TrackAlive()
            {
                while (aliveReference)
                {
                    yield return Timing.WaitForOneFrame;
                }
                CombatSystemSingleton.EventsHolder.OnCombatFinish();
            }
        }

        private static void DestroyAliveReference()
        {
            var aliveReference = CombatSystemSingleton.AliveGameObjectReference;
            if (aliveReference)
            {
                Object.Destroy(aliveReference);
            }



        }
    }

    public interface ICombatPreparationListener : ICombatEventListener
    {
        void OnCombatPrepares(
            IReadOnlyCollection<CombatEntity> allMembers,
            CombatTeam playerTeam,
            CombatTeam enemyTeam);
    }

    public interface ICombatStatesListener : ICombatEventListener
    {
        void OnCombatStart();
        void OnCombatFinish();
        void OnCombatQuit();
    }
}
