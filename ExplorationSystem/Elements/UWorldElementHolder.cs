using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExplorationSystem.Elements
{
    public class UWorldElementHolder : MonoBehaviour, IPointerClickHandler
    {
        [Title("Injections")] 
        [SerializeField] private SExplorationSceneDataHolder targetScene;

        [Title("Self References")]
        [SerializeField] private TextMeshProUGUI nameHolder;
        [SerializeField] private Image iconHolder;

        private void Awake()
        {
            if(targetScene)
                DoInjection(targetScene);
        }

        [Title("Debug")]
        [ShowInInspector,HideInEditorMode, InlineEditor()]
        private IExplorationSceneDataHolder _currentScene;

        [Button, ShowIf("_currentScene")]
        private void UpdateStateToCurrent()
        {
            DoInjection(_currentScene);
        }
        public void DoInjection(IExplorationSceneDataHolder dataHolder)
        {
            var sceneName = dataHolder.GetSceneName();
            var localizedSceneName = LocalizeMap.LocalizeName(sceneName);
            Injection(localizedSceneName);
            var sceneIcon = dataHolder.GetSceneIcon();
            Injection(sceneIcon);

            _currentScene = dataHolder;
        }

        private void Injection(string worldName)
        {
            nameHolder.text = worldName;
        }
        private void Injection(Sprite icon)
        {
            iconHolder.sprite = icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayerExplorationSingleton.WorldExplorationHandler.LoadExplorationScene(_currentScene);

        }
        

    }
}
