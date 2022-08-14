using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    [CreateAssetMenu(fileName = "N " + AssetPrefix, menuName = "Editor/Scenes/Global ExplorationScenes", order = -4)]
    public class SExplorationLevelGroupsHolder : ScriptableObject
    {
        private const string AssetPrefix = "[Global ExplorationScenes]";

        [SerializeField]
        private LevelGroupValues[] worldGroups = new LevelGroupValues[0];

#if UNITY_EDITOR
        [Button]
        private void TestNotNull()
        {
            var scenes = worldGroups[0].GetScenes();
            if (scenes == null)
            {
                Debug.LogError("NULL scenes array");
                return;
            }
            Debug.Log("All fine");
        } 
#endif


        [Serializable]
        internal struct LevelGroupValues
        {
            [SerializeField] private SExplorationSceneDataHolder[] scenes;

            public SExplorationSceneDataHolder[] GetScenes() => scenes;
        }
    }
}
