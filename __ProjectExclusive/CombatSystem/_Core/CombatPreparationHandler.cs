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
        public CombatPreparationHandler(int listAlloc = 2)
        {
            _preparationListeners =new List<ICombatPreparationListener>(listAlloc);
            _combatFinishListeners = new List<ICombatFinishListener>(listAlloc);

#if UNITY_EDITOR
            var debugElement = new CombatPreparationDebugger();
            Subscribe(debugElement);
#endif

        }

        [ShowInInspector,HorizontalGroup("Main",Title ="_______ Main Listeners ____________")]
        private readonly List<ICombatPreparationListener> _preparationListeners;
        [ShowInInspector]
        private readonly List<ICombatFinishListener> _combatFinishListeners;


        public bool IsCombatActive { get; private set; }

        public void Subscribe(ICombatPreparationListener listener)
        {
            if(listener is ICombatFinishListener finishListener)
                _combatFinishListeners.Add(finishListener);

            _preparationListeners.Add(listener);
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
            CombatingTeam playerTeam = new CombatingTeam(playerTeamProvider);
            CombatingTeam enemyTeam = new CombatingTeam(enemyTeamProvider);
            playerTeam.EnemyTeam = enemyTeam;
            enemyTeam.EnemyTeam = playerTeam;

            CombatSystemSingleton.VolatilePlayerTeam = playerTeam;
            CombatSystemSingleton.VolatileEnemyTeam = enemyTeam;
        }
        private static void RequestOnStart(IEnumerable<ICombatPreparationListener> listeners)
        {
            foreach (var listener in listeners)
            {
                listener.OnAfterLoadScene();
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

            public void OnAfterLoadScene()
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
        /// Events after the load (not necessary show the screen)
        /// </summary>
        void OnAfterLoadScene();
    }

    public interface ICombatFinishListener
    {
        void OnFinish(CombatingTeam wonTeam);
    }
    // TODO make the provider/s and its listeners
    

}
