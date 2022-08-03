using UnityEngine;

namespace Utils_Project.Scene
{
    public static class SceneStructures
    {
        public const string MainMenuSceneName = "MainMenu_Scene";
        public const string CharacterSelectionSceneName = "MainCharacterSelection_Scene";
    }

    internal interface ISceneStructureRead<out T>
    {
        T MainSceneType { get; }
        T CharacterSelectionType { get; }
    }
}
