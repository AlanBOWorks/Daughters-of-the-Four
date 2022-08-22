using System;
using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
        private static void DoSceneTransition(LoadSceneParameters parameters,
            LoadSceneParameters.ISceneLoadListener listener = null)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;

            sceneManager.DoSceneTransition(parameters, listener);
        }

        public static void LoadMainMenuScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainMenuSceneName, 
                showLoadScreenFromLeft, false, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void LoadMainCharacterSelectionScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.CharacterSelectionSceneName, showLoadScreenFromLeft, false, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void LoadWorldMapScene(bool showLoadScreenFromLeft, bool keepSceneAlive, float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainExplorationSceneName, showLoadScreenFromLeft,keepSceneAlive, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void DoTransitionExplorationScene(string explorationSceneName, bool showLoadScreenFromLeft,
            LoadSceneParameters.ISceneLoadListener listener = null,
            float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                explorationSceneName, showLoadScreenFromLeft, true, deltaModifier);
            DoSceneTransition(parameters,listener);
        }


        public static void LoadBattleScene(string sceneName, float deltaModifier = 1, 
            LoadSceneParameters.ISceneLoadListener listener = null)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                sceneName,true, true, deltaModifier);
            DoSceneTransition(parameters, listener);
        }

        
        public static void DoJustVisualTransition(float waitUntilHide, bool fromLeft, float deltaModifier = 1, 
            LoadSceneParameters.ISceneLoadListener listener = null)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;
            sceneManager.DoJustScreenTransition(waitUntilHide, fromLeft, deltaModifier, listener);
        }
    }

    public readonly struct LoadSceneParameters
    {
        public readonly string SceneName;
        public readonly bool ShowLoadScreenFromLeft;
        public readonly bool IsAdditive;
        public readonly float DeltaModifier;

        public LoadSceneParameters(string sceneName, bool showLoadScreenFromLeft, bool isAdditive = false, float deltaModifier = 1)
        {
            SceneName = sceneName;
            ShowLoadScreenFromLeft = showLoadScreenFromLeft;
            IsAdditive = isAdditive;
            DeltaModifier = deltaModifier;
        }

        public void ExtractValues(
            out string sceneName,
            out bool showLoadFromLeft,
            out bool isAdditive,
            out float deltaModifier)
        {
            sceneName = SceneName;
            showLoadFromLeft = ShowLoadScreenFromLeft;
            isAdditive = IsAdditive;
            deltaModifier = DeltaModifier;
        }

        public interface ISceneLoadListener { }
    }


    /// <summary>
    /// The very first and last invokes on Scene Loads.
    /// </summary>
    public interface ISceneLoadFirstLastCallListener : LoadSceneParameters.ISceneLoadListener
    {
        /// <summary>
        /// The very first invoke on load, before everything else (before the first frame of transition is done);<br></br>
        /// > For the moment in which the scene is hidden use [<seealso cref="ISceneHiddenListener.OnStartLoading"/>]
        /// instead
        /// </summary>
        void OnStartTransition();
        /// <summary>
        /// The very fast invoke on load, right after the last frame after the transition is done. <br></br>
        /// > For the moment in which the scene is shown use [<seealso cref="ISceneHiddenListener.OnLoadingFinish"/>]
        /// </summary>
        void OnFinishTransition();
    }

    /// <summary>
    /// Invokes before and after the Scene Transition's hiding Object appears in screen.
    /// </summary>
    public interface ISceneHiddenListener : LoadSceneParameters.ISceneLoadListener
    {
        /// <summary>
        /// The very first frame after the scene is hidden behind the transition object and
        /// starts the Scene load.<br></br>
        /// > For the very first loading invoke use [<seealso cref="ISceneLoadFirstLastCallListener.OnStartTransition"/>]
        /// </summary>
        void OnStartLoading();
        /// <summary>
        /// The very first frame after the scene is loaded (but its still hidden).<br></br>
        /// > For the very last loading invoke use [<seealso cref="ISceneLoadFirstLastCallListener.OnFinishTransition"/>]
        /// </summary>
        void OnLoadingFinish();
    }

    /// <summary>
    /// Listener for the async load ticks.
    /// </summary>
    public interface ISceneLoadingListener : LoadSceneParameters.ISceneLoadListener
    {
        void OnLoadSceneTick(float currentPercent);
    }
}
