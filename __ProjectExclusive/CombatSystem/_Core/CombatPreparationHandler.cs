using System;
using System.Collections.Generic;
using CombatEntity;
using Sirenix.OdinInspector;
using CombatTeam;
using MEC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatSystem
{
    public class CombatPreparationHandler
    {
        public CombatPreparationHandler()
        {
            _preparationListeners = new HashSet<ICombatPreparationListener>();
            _combatFinishListeners = new HashSet<ICombatFinishListener>();
            _combatDisruptionListeners = new HashSet<ICombatDisruptionListener>();

#if UNITY_EDITOR
            var debugElement = new CombatPreparationDebugger();
            Subscribe((ICombatPreparationListener) debugElement);
            Subscribe(debugElement as ICombatFinishListener);
#endif

        }

        [ShowInInspector,HorizontalGroup("Main",Title ="_______ Main Listeners ____________")]
        private readonly HashSet<ICombatPreparationListener> _preparationListeners;

        [ShowInInspector,HorizontalGroup("Disruption Listeners")]
        private readonly HashSet<ICombatFinishListener> _combatFinishListeners;
        [ShowInInspector, HorizontalGroup("Disruption Listeners")]
        private readonly HashSet<ICombatDisruptionListener> _combatDisruptionListeners;

        public bool IsCombatActive { get; private set; }

        public void Subscribe(ICombatPreparationListener listener)
        {
            _preparationListeners.Add(listener);
        }
        public void Subscribe(ICombatFinishListener finishListener)
        {
            _combatFinishListeners.Add(finishListener);
        }
        public void Subscribe(ICombatDisruptionListener disruptionListener)
        {
            _combatDisruptionListeners.Add(disruptionListener);
        }

        public void StartCombat(string sceneAssetPath, ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider)
        {
            Timing.RunCoroutine(_LoadCombat(sceneAssetPath, playerTeamProvider, enemyTeamProvider));
        }
        
        private IEnumerator<float> _LoadCombat(string sceneAssetPath,
            ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider)
        {
            var sceneLoadStatus =
                SceneManager.LoadSceneAsync(sceneAssetPath, LoadSceneMode.Additive);

            IsCombatActive = true;
            PrepareCombatTeams(playerTeamProvider,enemyTeamProvider);

            // ---- PREPARATION ----
            RequestPreparation(_preparationListeners);

            yield return Timing.WaitUntilDone(sceneLoadStatus);

            // ---- OON START (Before first Tick) ----
            RequestOnStart(_preparationListeners);
        }

        public void RequestLocalCombat(ITeamProvider playerTeamProvider,
            ITeamProvider enemyTeamProvider)
        {
            IsCombatActive = true;
            PrepareCombatTeams(playerTeamProvider, enemyTeamProvider);

            // ---- PREPARATION ----
            RequestPreparation(_preparationListeners);


            // ---- OON START (Before first Tick) ----
            RequestOnStart(_preparationListeners);
        }

        private static void PrepareCombatTeams(ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider)
        {
            var allEntities = CombatSystemSingleton.AllEntities;
            allEntities.Clear();

            CombatingTeam playerTeam = new CombatingTeam(playerTeamProvider);
            CombatingTeam enemyTeam = new CombatingTeam(enemyTeamProvider);
            playerTeam.Injection(enemyTeam);
            enemyTeam.Injection(playerTeam);

            CombatSystemSingleton.VolatilePlayerTeam = playerTeam;
            CombatSystemSingleton.VolatileEnemyTeam = enemyTeam;
        }
        private static void RequestOnStart(IEnumerable<ICombatPreparationListener> listeners)
        {
            foreach (var listener in listeners)
            {
                listener.OnAfterLoads();
            }
        }
        private static void RequestPreparation(IEnumerable<ICombatPreparationListener> listeners)
        {
            var playerTeam = CombatSystemSingleton.VolatilePlayerTeam;
            var enemyTeam = CombatSystemSingleton.VolatileEnemyTeam;
            foreach (var listener in listeners)
            {
                listener.OnPreparationCombat(playerTeam, enemyTeam);
            }
        }

        public void RequestPause()
        {
            foreach (var listener in _combatDisruptionListeners)
            {
                listener.OnCombatPause();
            }
        }

        public void RequestResume()
        {
            foreach (var listener in _combatDisruptionListeners)
            {
                listener.OnCombatResume();
            }
        }

        public void RequestExitCombat()
        {
            foreach (var listener in _combatDisruptionListeners)
            {
                listener.OnCombatExit();
            }


            CombatSystemSingleton.VolatilePlayerTeam = null;
            CombatSystemSingleton.VolatileEnemyTeam = null;
            IsCombatActive = false;
        }

        [Button, ShowIf("IsCombatActive")]
        public void RequestFinishCombat(CombatingTeam wonTeam)
        {
            foreach (ICombatFinishListener listener in _combatFinishListeners)
            {
                listener.OnFinish(wonTeam);
            }
            RequestExitCombat();
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

            public void OnAfterLoads()
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
        /// <summary>
        /// Events of preparation before or during load
        /// </summary>
        void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam);
        /// <summary>
        /// Events after all loads from [<see cref="OnPreparationCombat"/>] are done.
        /// </summary>
        void OnAfterLoads();
    }

    public interface ICombatFinishListener
    {
        void OnFinish(CombatingTeam wonTeam);
    }

    public interface ICombatDisruptionListener
    {
        void OnCombatPause();
        void OnCombatResume();
        /// <summary>
        /// Forced or natural, the combat exits its running state
        /// </summary>
        void OnCombatExit();
    }
    
}
