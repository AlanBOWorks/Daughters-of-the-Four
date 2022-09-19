using System;
using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
       

        private static ULoadSceneManager GetManager() => LoadSceneManagerSingleton.ManagerInstance;

        private static LoadSceneParameters.LoadType GetMainLoadType(bool fromLeft)
        {
            return LoadSceneParameters.GetMainLoadType(fromLeft);
        }

        public static void LoadMainMenuScene(bool showLoadScreenFromLeft, LoadCallBacks callbacks)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainMenuSceneName, targetType);

            GetManager().LoadScene(parameters, callbacks, LoadSceneMode.Single);
        }

        public static void LoadMainCharacterSelectionScene(bool showLoadScreenFromLeft, LoadCallBacks callbacks)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.CharacterSelectionSceneName, targetType);

            GetManager().LoadScene(parameters, callbacks, LoadSceneMode.Single);
        }

        public static void LoadWorldMapScene(bool showLoadScreenFromLeft, LoadCallBacks callbacks)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainExplorationSceneName, targetType);

            GetManager().LoadScene(parameters, callbacks, LoadSceneMode.Single);
        }

        public static void DoTransitionExplorationScene(string explorationSceneName, bool showLoadScreenFromLeft,
            LoadCallBacks callbacks)
        {
            LoadSceneParameters.LoadType targetType = GetMainLoadType(showLoadScreenFromLeft);
            LoadSceneParameters parameters = new LoadSceneParameters(
                explorationSceneName, targetType);

            GetManager().LoadScene(parameters, callbacks, LoadSceneMode.Additive);
        }


        private const float AfterBattleLoadDelay = .4f;
        public static void LoadBattleScene(string sceneName, bool isFirstLoad,
            LoadCallBacks callbacks)
        {
            var sceneManager = GetManager();
            const LoadSceneParameters.LoadType targetType = LoadSceneParameters.LoadType.CombatLoad;

            if (isFirstLoad)
            {
                LoadSceneParameters parameters = new LoadSceneParameters(
                    sceneName, targetType, AfterBattleLoadDelay);

                sceneManager.LoadScene(parameters, callbacks, LoadSceneMode.Additive);
            }
            else
            {
                sceneManager.JustTransition(targetType,0,callbacks,AfterBattleLoadDelay);
            }
        }


    }

    public readonly struct LoadSceneParameters
    {
        /// <summary>
        /// The target Scene to load; <br></br>
        /// Note: on Null the will be just a screen transition(hide) without scene loading.
        /// </summary>
        public readonly string SceneName;
        public readonly LoadType Type;
        public readonly float OnLoadDelay;

        public LoadSceneParameters(string sceneName, LoadType type, float onLoadDelay = 0)
        {
            SceneName = sceneName;
            Type = type;
            OnLoadDelay = onLoadDelay;
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

    }

    public readonly struct LoadCallBacks
    {
        public static LoadCallBacks NullCallBacks = new LoadCallBacks(null,null,null,null);

        public readonly Action OnStartTransition;
        public readonly Action OnFinishTransition;
        public readonly Action OnStartLoading;
        public readonly Action OnLoadingFinish;

        public LoadCallBacks(Action onStartLoading, Action onLoadingFinish) 
            : this(null,null, onStartLoading, onLoadingFinish)
        {
            
        }

        public LoadCallBacks(Action onStartTransition, Action onFinishTransition, Action onStartLoading, Action onLoadingFinish)
        {
            OnStartTransition = onStartTransition;
            OnFinishTransition = onFinishTransition;
            OnStartLoading = onStartLoading;
            OnLoadingFinish = onLoadingFinish;
        }
    }
}
