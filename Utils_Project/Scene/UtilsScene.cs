using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
        private static void DoSceneTransition(string sceneName, bool showLoadScreenFromLeft, bool keepAlive = false, float deltaModifier = 1)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;
            sceneManager.DoSceneTransition(sceneName, showLoadScreenFromLeft, keepAlive, deltaModifier);
        }

        public static void LoadMainMenuScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            DoSceneTransition(SceneStructures.MainMenuSceneName, showLoadScreenFromLeft,false, deltaModifier);
        }

        public static void LoadMainCharacterSelectionScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            DoSceneTransition(SceneStructures.CharacterSelectionSceneName, showLoadScreenFromLeft, false, deltaModifier);
        }

        public static void LoadExplorationScene(bool showLoadScreenFromLeft, bool keepSceneAlive, float deltaModifier = 1)
        {
            DoSceneTransition(SceneStructures.MainExplorationSceneName,showLoadScreenFromLeft,keepSceneAlive, deltaModifier);
        }

    }
}
