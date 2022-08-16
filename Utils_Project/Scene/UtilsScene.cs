using System;
using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
        private static void DoSceneTransition(LoadSceneParameters parameters, 
            Action onSceneLoad = null, Action onSceneLoadLast = null)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;

            sceneManager.DoSceneTransition(parameters, onSceneLoad, onSceneLoadLast);
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

        public static void LoadExplorationScene(bool showLoadScreenFromLeft, bool keepSceneAlive, float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                SceneStructures.MainExplorationSceneName, showLoadScreenFromLeft,keepSceneAlive, deltaModifier);
            DoSceneTransition(parameters);
        }

        /// <param name="onLoadFinish">Action fired after loading the scene</param>
        public static void LoadBattleScene(string sceneName, Action onLoadFinish, float deltaModifier = 1)
        {
            LoadSceneParameters parameters = new LoadSceneParameters(
                sceneName,true, true, deltaModifier);
            DoSceneTransition(parameters, onLoadFinish);
        }

        /// <param name="screenShowsFeedback">An Action callback that is fired twice. Note: <br></br>
        /// > true: means the screen is shown (exactly after the screen is shown and the timer is ended)<br></br>
        /// > false: means the screen was hidden (transition finish)</param>
        public static void DoJustVisualTransition(float waitUntilHide, bool fromLeft, Action<bool> screenShowsFeedback = null, float deltaModifier = 1)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;
            sceneManager.DoJustScreenTransition(waitUntilHide, fromLeft, screenShowsFeedback, deltaModifier);
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
    }
}
