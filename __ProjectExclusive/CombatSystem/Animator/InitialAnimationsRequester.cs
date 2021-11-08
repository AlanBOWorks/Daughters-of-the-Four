using CombatTeam;
using UnityEngine;

namespace CombatSystem.Animator
{
    public class InitialAnimationsRequester : ICombatPreparationListener
    {
        private CombatingTeam _playerTeam;
        private CombatingTeam _enemyTeam;

        public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
        }

        public void OnAfterLoads()
        {
            InvokeInitialAnimations(_playerTeam);
            InvokeInitialAnimations(_enemyTeam);
        }

        private static void InvokeInitialAnimations(CombatingTeam team)
        {
            foreach (var member in team)
            {
                member.InstantiatedHolder.AnimationHandler?.DoIntroductionAnimation(member);
            }
        }
    }
}
