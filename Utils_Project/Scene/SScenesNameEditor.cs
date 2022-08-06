using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Utils_Project.Scene
{
    [CreateAssetMenu(fileName = "Scene Name", menuName = "Editor/Scene Name Editor")]
    internal class SScenesNameEditor : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset mainMenuSceneAsset;
        [SerializeField] private SceneAsset characterSelectionAsset;
        [SerializeField] private SceneAsset explorationSceneAsset;


        private static void UpdateName(SceneAsset asset, string targetName)
        {
            asset.name = targetName;
            UtilsAssets.UpdateAssetName(asset);
        }

        [Button]
        private void UpdateNames()
        {
            UpdateName(mainMenuSceneAsset, SceneStructures.MainMenuSceneName);
            UpdateName(characterSelectionAsset, SceneStructures.CharacterSelectionSceneName);
            UpdateName(explorationSceneAsset, SceneStructures.MainExplorationSceneName);
        }
#endif
    }

    public class ScenesHolderEditorWindow : OdinEditorWindow
    {
        [ShowInInspector, InlineEditor()] private SScenesNameEditor _assetHolder;

        [MenuItem("Game/Editor/Scene Holder [WINDOW]", priority = -1)]
        private static void OpenWindow()
        {
            var window = GetWindow<ScenesHolderEditorWindow>();
            window._assetHolder = AssetDatabase.LoadAssetAtPath<SScenesNameEditor>("Assets/Scenes/InGame/Scene Name.Asset");
        }

    }
}
