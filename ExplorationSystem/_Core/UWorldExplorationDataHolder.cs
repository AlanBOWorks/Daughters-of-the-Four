using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExplorationSystem
{
    public class UWorldExplorationDataHolder : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField, InlineEditor()] 
        private SExplorationWorldLevelsHolder sceneGroupHolder;
        
        [Title("Scene")] 
        [ShowInInspector,DisableInEditorMode]
        private IExplorationSceneDataHolder _currentExplorationSceneData;

        public SExplorationWorldLevelsHolder GetWorldDataHolder() => sceneGroupHolder;

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
            if(_currentExplorationSceneData == dataHolder) return;

            _currentExplorationSceneData = dataHolder;
            PlayerExplorationSingleton.EventsHolder.OnSceneChange(dataHolder);
        }

    }

    internal interface ISceneChangeListener : IExplorationEventListener
    {
        void OnSceneChange(IExplorationSceneDataHolder sceneData);
    }
}
