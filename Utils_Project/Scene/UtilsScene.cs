using UnityEngine.SceneManagement;
using Utils_Project.Scene;

namespace Utils_Project
{
    public static class UtilsScene
    {
        private static void DoSceneTransition(string sceneName, bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            var sceneManager = LoadSceneManagerSingleton.ManagerInstance;
            sceneManager.DoSceneTransition(sceneName, showLoadScreenFromLeft, deltaModifier);
        }

        public static void LoadMainMenuScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            DoSceneTransition(SceneStructures.MainMenuSceneName, showLoadScreenFromLeft, deltaModifier);
        }

        public static void LoadMainCharacterSelectionScene(bool showLoadScreenFromLeft, float deltaModifier = 1)
        {
            DoSceneTransition(SceneStructures.CharacterSelectionSceneName, showLoadScreenFromLeft, deltaModifier);
        }



    }
}
