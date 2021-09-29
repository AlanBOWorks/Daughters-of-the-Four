using System;
using System.Collections.Generic;
using CombatEntity;
using Sirenix.OdinInspector;
using CombatTeam;
using UnityEngine;

namespace CombatSystem
{
    public class CombatPreparationHandler
    {
        public CombatPreparationHandler()
        {
            _preparationListeners =new List<ICombatPreparationListener>();
            _combatFinishListeners = new List<ICombatFinishListener>();

#if UNITY_EDITOR
            var debugElement = new CombatPreparationDebugger();
            Subscribe(debugElement);
#endif

        }

        [ShowInInspector]
        private readonly List<ICombatPreparationListener> _preparationListeners;
        [ShowInInspector]
        private readonly List<ICombatFinishListener> _combatFinishListeners;

        public bool IsCombatActive { get; private set; }

        public void Subscribe(ICombatPreparationListener listener)
        {
            _preparationListeners.Add(listener);
            if(listener is ICombatFinishListener finishListener)
                _combatFinishListeners.Add(finishListener);
        }

        public void StartCombat(ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider)
        {
            if (IsCombatActive)
            {
#if UNITY_EDITOR
                throw new AccessViolationException("Trying to start Combat while being in combat.");
#else
             return;
#endif
            }

            IsCombatActive = true;
            CombatingTeam playerTeam = new CombatingTeam(playerTeamProvider);
            CombatingTeam enemyTeam = new CombatingTeam(enemyTeamProvider);
            playerTeam.EnemyTeam = enemyTeam;
            enemyTeam.EnemyTeam = playerTeam;

            CombatSystemSingleton.VolatilePlayerTeam = playerTeam;
            CombatSystemSingleton.VolatileEnemyTeam = enemyTeam;


            // ---- PREPARATION ----
            foreach (var listener in _preparationListeners)
            {
                listener.OnPreparationCombat(playerTeam, enemyTeam);
            }

            // ---- OON START (Before first Tick) ----
            foreach (var listener in _preparationListeners)
            {
                listener.OnStartAndBeforeFirstTick();
            }
        }

        [Button, ShowIf("IsCombatActive")]
        public void RequestFinishCombat(CombatingTeam wonTeam)
        {
            foreach (ICombatFinishListener listener in _combatFinishListeners)
            {
                listener.OnFinish(wonTeam);
            }

            CombatSystemSingleton.VolatilePlayerTeam = null;
            CombatSystemSingleton.VolatileEnemyTeam = null;
            IsCombatActive = false;
        }


#if UNITY_EDITOR
        private class CombatPreparationDebugger : ICombatPreparationListener, ICombatFinishListener
        {
            [ShowInInspector,TextArea,DisableIf("_behaviourExplanation")]
            private string _behaviourExplanation = "It shows Debug.Log in the console for each preparation event"; 
            

            public void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
            {
                //this is just to remove Unity's warning messages
                if(_behaviourExplanation == null) return;
                    
                Debug.Log("xxxxxxxx---- Starting Combat -----xxxxxxxx");
                Debug.Log($"Player entities: {playerTeam.Count}");
                Debug.Log($"Enemy entities: {enemyTeam.Count}");
            }

            public void OnStartAndBeforeFirstTick()
            {
                Debug.Log("---- [Before] First Tick Combat -----");
            }

            public void OnFinish(CombatingTeam wonTeam)
            {
                Debug.Log("xxxxxxxx---- Combat Finish ----xxxxxxxx");
                Debug.Log($"Winner Team: {wonTeam}");
            }
        }
#endif
    }

    public interface ICombatPreparationListener
    {
        void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam);

        void OnStartAndBeforeFirstTick();
    }
    public interface ICombatFinishListener
    {
        void OnFinish(CombatingTeam wonTeam);
    }
    // TODO make the provider/s and its listeners
    

}
