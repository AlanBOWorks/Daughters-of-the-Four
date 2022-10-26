using System;
using CombatSystem.Team;
using CombatSystem.UI;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace ExplorationSystem.UI
{
    public class UExplorationSkillsStanceHandler : MonoBehaviour
    {
        [SerializeField]
        private StanceSpawner stanceSpawner = new StanceSpawner();
        [ShowIf("_stanceElements")]
        private StanceStructure<UStanceElementHolder> _stanceElements;

        public UStanceElementHolder this[EnumTeam.Stance stance] => _stanceElements[stance];

        private void Awake()
        {
            _stanceElements = new StanceStructure<UStanceElementHolder>();
            stanceSpawner.Awake(_stanceElements);
        }

        public void SwitchStance(EnumTeam.Stance stance)
        {
            stanceSpawner.SwitchActiveStanceButton(stance);
        }


        [Serializable]
        private sealed class StanceSpawner : PrefabInstantiationHandler<UStanceElementHolder>, UStanceElementHolder.IStanceEventsHandler
        {
            [SerializeField] private RectTransform onPoolParent;

            [Title("Higher")]
            [SerializeField]
            private UExplorationSkillsWindowHandler skillsHandler;

            private Color _activeTextColor;
            private Color _activeBackgroundColor;

            private StanceStructure<UStanceElementHolder> _stancesHolder;

            public void Awake(StanceStructure<UStanceElementHolder> stanceElements)
            {
                _stancesHolder = stanceElements;

                var prefab = GetPrefab();
                DoInitializations();
                DoInstantiations();
                prefab.gameObject.SetActive(false);

                void DoInitializations()
                {
                    var prefabTextHolder = prefab.GetTextHolder();
                    var prefabBackgroundHolder = prefab.GetBackgroundHolder();

                    _activeTextColor = prefabTextHolder.color;
                    _activeBackgroundColor = prefabBackgroundHolder.color;

                    ResetVisualState(prefab);
                }
                void DoInstantiations()
                {
                    var enumerable = EnumTeam.GetStancesEnumerable();


                    var prefabTransform = (RectTransform) prefab.transform;
                    var prefabWidth = prefabTransform.sizeDelta.x;
                    float i = 0;
                    foreach (var stance in enumerable)
                    {
                        var element = SpawnElement();
                        stanceElements[stance] = element;

                        var elementTransform = (RectTransform) element.transform;
                        elementTransform.SetParent(onPoolParent);
                        var elementPosition = elementTransform.anchoredPosition;
                        elementPosition.x = i * prefabWidth;
                        elementTransform.anchoredPosition = elementPosition;

                        element.Injection(stance);
                        element.EventsHandler = this;

                        i++;
                    }
                }

            }

            public void SwitchActiveStanceButton(EnumTeam.Stance stance)
            {
                var holder = UtilsTeam.GetElement(stance, _stancesHolder);
                OnPointerClick(holder, stance);

            }

            private UStanceElementHolder _currentActiveElement;
            public void OnPointerEnter(UStanceElementHolder holder, EnumTeam.Stance stance)
            {
                if(holder == _currentActiveElement) return;
                AnimateHover(holder);
            }

            public void OnPointerExit(UStanceElementHolder holder, EnumTeam.Stance stance)
            {
                if(holder == _currentActiveElement) return;
                ResetVisualState(holder);
            }

            public void OnPointerClick(UStanceElementHolder holder, EnumTeam.Stance stance)
            {
                if(_currentActiveElement == holder) return;

                if (_currentActiveElement != null)
                    ResetVisualState(_currentActiveElement);

                AnimateElement();
                _currentActiveElement = holder;
                AnimateClick(holder);

                skillsHandler.SwitchStance(stance);

                void AnimateElement()
                {
                    if (_currentActiveElement != null)
                    {
                        var currentElement = _currentActiveElement.GetAnimationTarget();
                        DOTween.Kill(currentElement);
                        currentElement.localScale = Vector3.one;
                    }

                    var targetElement = holder.GetAnimationTarget();
                    Vector3 punch = new Vector3(-.2f,-.2f, 0);
                    const float animationDuration = .2f;
                    targetElement.DOPunchScale(punch, animationDuration,4);
                }

            }

            private const float IconActiveAlpha = .4f;
            private void AnimateClick(UStanceElementHolder element)
            {
                var icon = element.GetIconHolder();
                var iconColor = icon.color;
                iconColor.a = IconActiveAlpha;
                icon.color = iconColor;

                AnimateHover(element);
            }

            private void AnimateHover(UStanceElementHolder element)
            {
                element.GetBackgroundHolder().color = _activeBackgroundColor;
                element.GetTextHolder().color = _activeTextColor;
            }


            private void ResetVisualState(UStanceElementHolder element)
            {
                var background = element.GetBackgroundHolder();
                var backgroundColor = background.color;
                backgroundColor.a = 0;
                background.color = backgroundColor;

                var textHolder = element.GetTextHolder();
                textHolder.color = _activeBackgroundColor;

                var iconHolder = element.GetIconHolder();
                var iconColor = iconHolder.color;
                iconColor.a = 0;
                iconHolder.color = iconColor;
            }
        }

    }
}
