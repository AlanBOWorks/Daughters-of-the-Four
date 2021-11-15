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

        public void OnAfterLoadsCombat()
        {
            InvokeInitialAnimations(_playerTeam);
            InvokeInitialAnimations(_enemyTeam);
        }

        private static void InvokeInitialAnimations(CombatingTeam team)
        {
            foreach (var member in team)
            {
                var holder = member.InstantiatedHolder;
                if(holder == null) continue;

                holder.AnimationHandler?.DoIntroductionAnimation(member);
            }
        }
    }
}
