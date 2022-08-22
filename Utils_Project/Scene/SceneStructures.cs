using UnityEngine;

namespace Utils_Project.Scene
{
    public static class SceneStructures
    {
        /**
         * Nomenclature NOTE:
         * > names as "------Scene" are behaviour Scenes (aren't directly meant for the GamePlay Loop but necessary)         *
         * > names as "------_Scene" are main GamePlay Scenes (Override types)
         * > anything else is considered as GamePlay Scene of [Additive] type
         */

        public const string StartGameSceneName = "StartGameScene";
        public const string NullLevelSceneName = "[BG_Level] NULL - BackUpScene";

        public const string MainMenuSceneName = "MainMenu_Scene";
        public const string CharacterSelectionSceneName = "MainCharacterSelection_Scene";
        public const string MainExplorationSceneName = "MainExploration_Scene";


    }

    internal interface ISceneStructureRead<out T>
    {
        T MainSceneType { get; }
        T CharacterSelectionType { get; }
    }
}
