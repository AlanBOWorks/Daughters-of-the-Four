using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils_Project;
using Random = UnityEngine.Random;

namespace ExplorationSystem
{
    public class UExplorationLevelDataHolder : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField, InlineEditor()] 
        private SExplorationLevelGroupsHolder sceneGroupHolder;
        
        [Title("Scene")] 
        [ShowInInspector,DisableInEditorMode]
        private IExplorationSceneDataHolder _currentExplorationSceneData;

        private ISceneChangeListener[] _listeners;

        private void Awake()
        {
            _listeners = GetComponents<ISceneChangeListener>();
        }

        [Button,DisableInEditorMode]
        private void TestRandomWorld(int targetGroupIndex)
        {
            var group = sceneGroupHolder.GetWorld()[targetGroupIndex];
            var targetScenes = group.GetScenes();
            var targetScene = targetScenes[Random.Range(0, targetScenes.Length)];
            Injection(targetScene);
        }



        private void Injection(IExplorationSceneDataHolder dataHolder)
        {
            _currentExplorationSceneData = dataHolder;
            InvokeListeners();
        }

        private void InvokeListeners()
        {
            foreach (var listener in _listeners)
            {
                listener.OnSceneChange(_currentExplorationSceneData);
            }

        }

    }

    internal interface ISceneChangeListener
    {
        void OnSceneChange(IExplorationSceneDataHolder sceneData);
    }
}
