using UnityEngine.SceneManagement;

namespace Utils_Project
{
    public static class UtilsScene
    {
        public const string MainMenuSceneName = "MainMenu_Scene";
        public const string MainCharacterSelectionSceneName = "MainCharacterSelection_Scene";

       

        public static void LoadMainMenuScene()
        {
            SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

        public static void LoadMainCharacterSelectionScene()
        {
            SceneManager.LoadSceneAsync(MainCharacterSelectionSceneName);
        }



    }
}
