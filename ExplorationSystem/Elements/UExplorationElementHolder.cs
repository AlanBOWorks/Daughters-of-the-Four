using System;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils_Extended.UI;
using Utils_Project.UI;

namespace ExplorationSystem.Elements
{
    [RequireComponent(typeof(UButtonVisualEventsHolder))]
    public class UExplorationElementHolder : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private IconColorChanger iconHolder = new IconColorChanger();

        private static readonly Color DefaultColor = new Color(.1f,.1f,.1f);
        private Color _dedicatedColor;

        [HideInEditorMode,ShowInInspector]
        private EnumExploration.ExplorationType _currentBehaviour;


        private void OnDisable()
        {
            _currentBehaviour = EnumExploration.ExplorationType.Undefined;
            _elementSelected = false;
            HandleVisuals();
            Timing.PauseCoroutines(iconHolder.CurrentHandle);
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(iconHolder.CurrentHandle);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(iconHolder.CurrentHandle);
        }

        public void Injection(EnumExploration.ExplorationType type)
        {
            _currentBehaviour = type;
        }

        public void Injection(Sprite elementIcon)
        {
            iconHolder.Injection(elementIcon);
        }

        public void Injection(Color dedicatedColor)
        {
            _dedicatedColor = dedicatedColor;
        }

        private bool _elementSelected;
        public void OnPointerClick(PointerEventData eventData)
        {
            _elementSelected = !_elementSelected;
            HandleVisuals();
        }

        private void HandleVisuals()
        {
            if (_elementSelected)
            {
                iconHolder.Animate(_dedicatedColor);
            }
            else
            {
                iconHolder.InstantColorChange(DefaultColor);
            }
        }
    }
}
