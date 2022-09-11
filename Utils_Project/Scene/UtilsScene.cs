using System;
using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
        private static void DoSceneTransition(LoadSceneParameters parameters,
            LoadSceneParameters.ISceneLoadCallback listener = null)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;

            sceneManager.DoSceneTransition(parameters, listener);
        }

        private static LoadSceneParameters.LoadType GetMainLoadType(bool fromLeft)
        {
            return LoadSceneParameters.GetMainLoadType(fromLeft);
        }

        public static void LoadMainMenuScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainMenuSceneName, 
                targetType, false, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void LoadMainCharacterSelectionScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.CharacterSelectionSceneName, targetType, false, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void LoadWorldMapScene(bool showLoadScreenFromLeft, bool keepSceneAlive, float deltaModifier = 1)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainExplorationSceneName, targetType,keepSceneAlive, deltaModifier);
            DoSceneTransition(parameters);
        }

        public static void DoTransitionExplorationScene(string explorationSceneName, bool showLoadScreenFromLeft,
            LoadSceneParameters.ISceneLoadCallback listener = null,
            float deltaModifier = 1)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                explorationSceneName, targetType, true, deltaModifier);
            DoSceneTransition(parameters,listener);
        }


        public static void LoadBattleScene(string sceneName, bool isFirstLoad,
            LoadSceneParameters.ISceneLoadCallback loadCombatCallback,
            float deltaModifier = 1)
        {
            if(loadCombatCallback == null)
                throw new NullReferenceException("Listener was Null while loading the Combat Scene;");

            if (isFirstLoad)
            {
                LoadSceneParameters.LoadType targetType = LoadSceneParameters.LoadType.CombatLoad;
                LoadSceneParameters parameters = new LoadSceneParameters(
                    sceneName, targetType, true, deltaModifier);
                DoSceneTransition(parameters, loadCombatCallback);
            }
            else
            {
                DoJustVisualTransition(.4f, true, deltaModifier,loadCombatCallback);
            }
        }

        
        public static void DoJustVisualTransition(float waitUntilHide, bool fromLeft, float deltaModifier = 1, 
            LoadSceneParameters.ISceneLoadCallback listener = null)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;
            sceneManager.DoJustScreenTransition(waitUntilHide, fromLeft, deltaModifier, listener);
        }

    }

    public readonly struct LoadSceneParameters
    {
        public readonly string SceneName;
        public readonly LoadType Type;
        public readonly bool IsAdditive;
        public readonly float DeltaModifier;
        public readonly float OnLoadDelay;

        public LoadSceneParameters(string sceneName, LoadType type, bool isAdditive = false, float deltaModifier = 1,
            float onLoadDelay = 0)
        {
            SceneName = sceneName;
            Type = type;
            IsAdditive = isAdditive;
            DeltaModifier = deltaModifier;
            OnLoadDelay = onLoadDelay;
        }

        public void ExtractValues(
            out string sceneName,
            out LoadType type,
            out bool isAdditive,
            out float deltaModifier)
        {
            sceneName = SceneName;
            type = Type;
            isAdditive = IsAdditive;
            deltaModifier = DeltaModifier;
        }

        public static LoadType GetMainLoadType(bool isFromLeft) => isFromLeft 
            ? LoadType.MainLoadFromLeft 
            : LoadType.MainLoadFromRight;


        public enum LoadType
        {
            MainLoadFromLeft,
            MainLoadFromRight,
            CombatLoad
        }

        /// <summary>
        /// Base Interface to be inherent for receiving event callback during loads (use derivation instead of this).
        /// <br></br>
        /// <br></br>
        /// Base type for: <br></br>
        /// - <seealso cref="ISceneLoadFirstLastCallListener"/><br></br>
        /// - <seealso cref="ISceneHiddenListener"/><br></br>
        /// - <seealso cref="ISceneLoadingListener"/>
        /// </summary>
        public interface ISceneLoadCallback { }
    }


    /// <summary>
    /// The very first and last invokes on Scene Loads.
    /// </summary>
    public interface ISceneLoadFirstLastCallListener : LoadSceneParameters.ISceneLoadCallback
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
    public interface ISceneHiddenListener : LoadSceneParameters.ISceneLoadCallback
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
    public interface ISceneLoadingListener : LoadSceneParameters.ISceneLoadCallback
    {
        void OnLoadSceneTick(float currentPercent);
    }
}
