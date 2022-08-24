using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExplorationSystem
{
    public class UWorldExplorationDataHolder : MonoBehaviour, ISceneChangeListener
    {
        [Title("References")] 
        [SerializeField, InlineEditor()] 
        private SExplorationWorldLevelsHolder sceneGroupHolder;
        
        [Title("Scene")] 
        [ShowInInspector,DisableInEditorMode]
        private IExplorationSceneDataHolder _currentExplorationSceneData;

        private int _currentWorldLevelIndex;

        private void Awake()
        {
            PlayerExplorationSingleton.EventsHolder.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerExplorationSingleton.EventsHolder.UnSubscribe(this);
        }


        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            _currentExplorationSceneData = sceneData;
        }
    }

    internal interface ISceneChangeListener : IExplorationEventListener
    {
        void OnSceneChange(IExplorationSceneDataHolder sceneData);
    }


    [Serializable]
    internal struct LevelGroupValues
    {
        [SerializeField] private SExplorationSceneDataHolder[] scenes;

        public SExplorationSceneDataHolder[] GetScenes() => scenes;
    }
}
