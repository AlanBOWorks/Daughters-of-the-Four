using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class CombatPreparationStatesHandler : ICombatPreparationListener, ICombatStatesListener
    {
        public CombatPreparationStatesHandler(SystemCombatEventsHolder eventsHolder)
        {
            _eventsHolder = eventsHolder;
        }

        private readonly SystemCombatEventsHolder _eventsHolder;


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            DestroyAliveReference();
            CreateAliveReference();
            _eventsHolder.OnCombatPrepares(allMembers, playerTeam, enemyTeam);

            Timing.RunCoroutine(_WaitForPreparesToFinish());

            IEnumerator<float> _WaitForPreparesToFinish()
            {
                yield return Timing.WaitForOneFrame; //todo true wait
                OnCombatPreStarts(playerTeam, enemyTeam);
            }
        }

        public void OnCombatStops()
        {
            DestroyAliveReference();
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _eventsHolder.OnCombatPreStarts(playerTeam, enemyTeam);
            //todo animations for starting fight && wait until is finish?
            _eventsHolder.OnCombatStart();
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            DestroyAliveReference();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
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
                CombatSystemSingleton.EventsHolder.OnCombatFinish(false);
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
        /// <summary>
        /// Is invoked after [<see cref="ICombatPreparationListener.OnCombatPrepares"/>]
        /// and before [<seealso cref="OnCombatStart"/>].
        /// <br></br>
        /// Use this to prepare thing that requires the combat itself being prepared but before the combat as such
        /// stats <example>(eg: repositions, initial animations, some text)</example>.
        /// <br></br>____<br></br>
        /// For necessary elements use [<see cref="ICombatPreparationListener.OnCombatPrepares"/>] preferably.
        /// </summary>
        /// <param name="playerTeam"></param>
        /// <param name="enemyTeam"></param>
        void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam);

        void OnCombatStart();

        /// <summary>
        /// First event invoked for ending the Combat; is triggers for both [<see cref="OnCombatFinish"/>] and
        /// [<see cref="OnCombatQuit"/>]
        /// </summary>
        void OnCombatEnd();

        /// <summary>
        /// Invoked only if the combat is finish by natural conditions
        /// </summary>
        /// <param name="isPlayerWin"></param>
        void OnCombatFinish(bool isPlayerWin);
        /// <summary>
        /// Invoked only if the combat is ended by forced means (quitting the game, by an event...)
        /// </summary>
        void OnCombatQuit();
    }
}
