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
            _eliteListeners = new List<ICombatPreparationEliteListener>();

#if UNITY_EDITOR
            var debugElement = new CombatPreparationDebugger();
            Subscribe(debugElement);
#endif

        }

        [ShowInInspector,HorizontalGroup("Main",Title ="_______ Main Listeners ____________")]
        private readonly List<ICombatPreparationListener> _preparationListeners;
        [ShowInInspector, HorizontalGroup("Main")]
        private readonly List<ICombatOnBeforeTickPreparation> _beforeTickListeners;
        [ShowInInspector]
        private readonly List<ICombatFinishListener> _combatFinishListeners;


        [ShowInInspector] 
        private readonly List<ICombatPreparationEliteListener> _eliteListeners;

        public bool IsCombatActive { get; private set; }

        public void Subscribe(ICombatPreparationListener listener)
        {
            if(listener is ICombatFinishListener finishListener)
                _combatFinishListeners.Add(finishListener);
            if(listener is ICombatPreparationEliteListener eliteListener)
                _eliteListeners.Add(eliteListener);
            if(listener is ICombatOnBeforeTickPreparation onBeforeTickListener)
                _beforeTickListeners.Add(onBeforeTickListener);

            _preparationListeners.Add(listener);
        }

        public void StartCombat(string sceneAssetPath, ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider, bool isElite)
        {
            if (IsCombatActive)
            {
                throw new AccessViolationException("Trying to start Combat while being in combat.");
            }

            Timing.RunCoroutine(_LoadCombat(sceneAssetPath, playerTeamProvider, enemyTeamProvider, isElite));
        }

        private IEnumerator<float> _LoadCombat(string sceneAssetPath,
            ITeamProvider playerTeamProvider, ITeamProvider enemyTeamProvider,
            bool isElite)
        {
            var sceneLoadStatus =
                SceneManager.LoadSceneAsync(sceneAssetPath, LoadSceneMode.Additive);

            IsCombatActive = true;
            CombatingTeam playerTeam = new CombatingTeam(playerTeamProvider);
            CombatingTeam enemyTeam = new CombatingTeam(enemyTeamProvider);
            playerTeam.EnemyTeam = enemyTeam;
            enemyTeam.EnemyTeam = playerTeam;

            CombatSystemSingleton.VolatilePlayerTeam = playerTeam;
            CombatSystemSingleton.VolatileEnemyTeam = enemyTeam;

            // ---- PREPARATION ----
            RequestPreparation(_preparationListeners);


            yield return Timing.WaitUntilDone(sceneLoadStatus);

            // ---- OON START (Before first Tick) ----
            RequestOnStart(_preparationListeners);


            void RequestPreparation(List<ICombatPreparationListener> listeners)
            {
                foreach (var listener in listeners)
                {
                    listener.OnPreparationCombat(playerTeam, enemyTeam);
                }
            }


            void RequestOnStart(List<ICombatPreparationListener> listeners)
            {
                foreach (var listener in listeners)
                {
                    listener.OnAfterLoadScene();
                }
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

    public interface ICombatOnBeforeTickPreparation
    {
        void OnBeforeTicking();
        void OnFirstTick();
    }

    /// <summary>
    /// Could be special music, cinematics, events, etc
    /// </summary>
    public interface ICombatPreparationEliteListener
    {
        void PrepareEliteCombat();
    }

    public interface ICombatFinishListener
    {
        void OnFinish(CombatingTeam wonTeam);
    }
    // TODO make the provider/s and its listeners
    

}
