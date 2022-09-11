using System;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils_Extended.UI;
using Utils_Project.UI;

namespace ExplorationSystem
{
    [RequireComponent(typeof(UButtonVisualEventsHolder))]
    public class UExplorationElementHolder : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconHolder;
        [HideInEditorMode,ShowInInspector]
        private EnumExploration.ExplorationType _currentBehaviour;


        private void OnDisable()
        {
            _currentBehaviour = EnumExploration.ExplorationType.Undefined;
            _elementSelected = false;
        }


        public void Injection(EnumExploration.ExplorationType type)
        {
            _currentBehaviour = type;
        }

        public void Injection(Sprite elementIcon)
        {
            iconHolder.sprite = elementIcon;
        }


        private bool _elementSelected;
        public void OnPointerClick(PointerEventData eventData)
        {
            _elementSelected = !_elementSelected;
            ExplorationSingleton.EventsHolder.OnExplorationRequest(_currentBehaviour);
        }
    }

}
