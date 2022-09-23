using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using MEC;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class CombatPreparationStatesHandler : ICombatPreparationListener, ICombatStartListener
    {
        public CombatPreparationStatesHandler(SystemCombatEventsHolder eventsHolder)
        {
            _eventsHolder = eventsHolder;
        }

        private readonly SystemCombatEventsHolder _eventsHolder;


        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _eventsHolder.OnCombatPrepares(allMembers, playerTeam, enemyTeam);

            Timing.RunCoroutine(_WaitForPreparesToFinish());

            IEnumerator<float> _WaitForPreparesToFinish()
            {
                yield return Timing.WaitForOneFrame; //todo true wait
                OnCombatPreStarts(playerTeam, enemyTeam);
            }
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

        
    }

    /// <summary>
    /// The very first event call on combat preparation;<br></br>
    /// [<seealso cref="ICombatStartListener"/>] is called after this
    /// </summary>
    public interface ICombatPreparationListener : ICombatEventListener
    {
        void OnCombatPrepares(
            IReadOnlyCollection<CombatEntity> allMembers,
            CombatTeam playerTeam,
            CombatTeam enemyTeam);
    }

    /// <summary>
    /// The second step of combat preparation;<br></br>
    /// [<seealso cref="ICombatPreparationListener"/>] is called before this
    /// </summary>
    public interface ICombatStartListener : ICombatEventListener
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
    }

}
