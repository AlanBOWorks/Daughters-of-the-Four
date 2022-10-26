using System;
using System.Collections.Generic;
using CombatSystem;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using CombatSystem.Team;
using CombatSystem.UI;
using DG.Tweening;
using Localization.Characters;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utils;
using Image = UnityEngine.UI.Image;

namespace ExplorationSystem.UI
{
    public class UExplorationSkillsWindowHandler : MonoBehaviour, USkillElementHolder.ISkillElementEventsHandler
    {
        [Title("Curves")]
        [SerializeField, InlineEditor()]
        private SCurve animationCurve;
        [SerializeField, InlineEditor()]
        private SCurve clickAnimationCurve;

        [Title("Windows values")] 
        private RectTransform _root;
        [SerializeField] 
        private GraphicRaycaster raycaster;


        [Title("Skills values")]
        [SerializeField] 
        private TextMeshProUGUI characterNameHolder;
        [SerializeField] 
        private TextMeshProUGUI roleNameHolder;
        [SerializeField] 
        private Image stanceIconHolder;
        [SerializeField]
        private UExplorationSkillsStanceHandler stanceHandler;

        [SerializeReference]
        private Spawner skillListSpawner = new Spawner();
        [SerializeReference]
        private Spawner divineSkillsSpawner = new Spawner();


        private Vector2 _rootInitialPosition;
        private Vector2 _rootHiddenPosition;

        private static Color OnInactiveIconColor = new Color(.094f,.094f,.094f);
        private static Color OnActiveIconColor = new Color(.86f,.86f,.86f);

        private void Awake()
        {
            _root = (RectTransform) transform;
            _rootInitialPosition = _root.anchoredPosition;
            UpdateHiddenPosition();


            skillListSpawner.Awake();
            skillListSpawner.EventsHandler = this;

            _stancesRecord = new Dictionary<ICombatEntityProvider, EnumTeam.Stance>();
            InstantHide();

            var skillElementPrefab = skillListSpawner.GetPrefab();
            var skillIcon = skillElementPrefab.GetIconHolder().rectTransform;
            _skillElementInitialSize = skillIcon.sizeDelta;
        }

        private void OnEnable()
        {
            UpdateHiddenPosition();
        }

        private void UpdateHiddenPosition()
        {
            _rootHiddenPosition = _rootInitialPosition;
            _rootHiddenPosition.y = -_root.sizeDelta.y * 1.5f;
        }

        private void OnDisable()
        {
            CurrentEntity = null;
            skillListSpawner.Clear();
            _stancesRecord = new Dictionary<ICombatEntityProvider, EnumTeam.Stance>();
            Timing.KillCoroutines(_mainAnimationHandle);
            Timing.KillCoroutines(_elementAnimationHandle);

            skillListSpawner.OnDisable();
            divineSkillsSpawner.OnDisable();
        }

        private Dictionary<ICombatEntityProvider, EnumTeam.Stance> _stancesRecord;

        public ICombatEntityProvider CurrentEntity { get; private set; }

        private EnumTeam.Stance GetTargetStanceOnEntity(ICombatEntityProvider entity)
        {
            if (_stancesRecord.ContainsKey(entity)) return _stancesRecord[entity];
            var targetStance = UtilsTeam.ParseStance(entity.GetAreaData().RoleType);
            _stancesRecord.Add(entity,targetStance);
            return targetStance;
        }

        private void UpdateEntityInfo()
        {
            var entityName =
                LocalizationPlayerCharacters.LocalizeCharactersName(CurrentEntity.GetProviderEntityName());
            characterNameHolder.text = entityName;

            var roleType = CurrentEntity.GetAreaData().RoleType;
            var roleName = LocalizeTeam.Localize(roleType);
            roleNameHolder.text = roleName;
        }

        public void SwitchEntity(ICombatEntityProvider entityProvider)
        {
            if (CurrentEntity == entityProvider) return;
            skillListSpawner.Clear();
            divineSkillsSpawner.Clear();

            CurrentEntity = entityProvider;
            var stance = GetTargetStanceOnEntity(entityProvider);
            HandleStance(stance);
            HandleDivineSkillsHolderPosition();

            stanceHandler.SwitchStance(stance);
            
            UpdateEntityInfo();
            ResetCurrentSkillElements();
            Show();
        }

        private void HandleDivineSkillsHolderPosition()
        {
            var skillsListSeparation = skillListSpawner.TotalSeparation;
            var divineSkillsRoot = (RectTransform)divineSkillsSpawner.ElementsHolder.parent;
            var position = divineSkillsRoot.anchoredPosition;
            position.x = skillsListSeparation;
            divineSkillsRoot.anchoredPosition = position;
        }

        public void SwitchStance(EnumTeam.Stance stance)
        {
            skillListSpawner.Clear();
            HandleStance(stance);
            _stancesRecord[CurrentEntity] = stance;
            HandleDivineSkillsHolderPosition();
            ResetCurrentSkillElements();
        }

        private void HandleStance(EnumTeam.Stance stance)
        {
            var skills = UtilsTeam.GetElement(stance, CurrentEntity.GetPresetSkills());
            skillListSpawner.HandleSkills(skills);
            var theme = CombatThemeSingleton.RolesThemeHolder;
            var stanceTheme = UtilsTeam.GetElement(stance, theme);
            stanceIconHolder.sprite = stanceTheme.GetThemeIcon();
        }


        private CoroutineHandle _mainAnimationHandle;
        private const float DeltaSpeed = 4f;
        private float _animationLerp;
        private void Show()
        {
            var rootGameObject = _root.gameObject;
            bool isActive = rootGameObject.activeSelf;
            rootGameObject.SetActive(true);
            raycaster.enabled = true;

            if (_mainAnimationHandle.IsRunning)
            {
                Timing.KillCoroutines(_mainAnimationHandle);
            }
            else
            {
                const float onActiveLerp = 0;
                const float onSwitchEntityLerp = .7f;
                _animationLerp = isActive ? onSwitchEntityLerp: onActiveLerp;
            }
            _mainAnimationHandle = Timing.RunCoroutine(_PopUpAnimation());


            IEnumerator<float> _PopUpAnimation()
            {
                yield return Timing.WaitForOneFrame;
                while (_animationLerp < 1)
                {
                    _animationLerp += DeltaSpeed * Timing.DeltaTime;
                    LerpPosition();
                    yield return Timing.WaitForOneFrame;
                }

                _animationLerp = 1;
                LerpPosition();

            }
        }

        private void LerpPosition()
        {
            float lerp = animationCurve.Evaluate(_animationLerp);
            _root.anchoredPosition
                = Vector2.LerpUnclamped(_rootHiddenPosition, _rootInitialPosition, lerp);
        }

        public void Hide()
        {
            raycaster.enabled = false;
            if (_mainAnimationHandle.IsRunning)
            {
                Timing.KillCoroutines(_mainAnimationHandle);
            }
            else
            {
                _animationLerp = 1;
            }
            _mainAnimationHandle = Timing.RunCoroutine(_HideAnimation());

            IEnumerator<float> _HideAnimation()
            {
                yield return Timing.WaitForOneFrame;
                while (_animationLerp > 0)
                {
                    _animationLerp -= DeltaSpeed * Timing.DeltaTime;
                    LerpPosition();
                    yield return Timing.WaitForOneFrame;
                }

                InstantHide();
                ResetCurrentSkillElements();
            }
        }

        private void InstantHide()
        {
            _animationLerp = 0;
            LerpPosition();
            raycaster.enabled = false;
            _root.gameObject.SetActive(false);
        }





        // ISkillElementEventsHandler
        private CoroutineHandle _elementAnimationHandle;
        private Vector2 _skillElementInitialSize;
        public void OnPointerEnter(USkillElementHolder element, IFullSkill skill)
        {
            ElementHoverAnimation(element);
        }

        public void OnPointerExit(USkillElementHolder element, IFullSkill skill)
        {
            ResetHoverElementState(element);
        }

        private void ResetCurrentSkillElements()
        {
            ResetHoverElementState(_currentFocusElement);
            ResetActiveSkillElementState(_currentActiveElement);

            _currentFocusElement = null;
            _currentActiveElement = null;
        }

        private USkillElementHolder _currentFocusElement;
        private const float ElementAnimationDeltaSpeed = 12f;
        private void ElementHoverAnimation(USkillElementHolder element)
        {
            Timing.KillCoroutines(_elementAnimationHandle);
            if (_currentFocusElement != null)
            {
                ResetHoverElementState(_currentFocusElement);
            }

            _elementAnimationHandle = Timing.RunCoroutine(_Animation());
            _currentFocusElement = element;

            IEnumerator<float> _Animation()
            {
                float lerp = 0;
                var elementIcon = element.GetIconHolder().rectTransform;
                Vector2 targetSize = CalculateHoverSize();
                while (lerp < 1)
                {
                    yield return Timing.WaitForOneFrame;
                    lerp += Timing.DeltaTime * ElementAnimationDeltaSpeed;
                    DoLerp();
                }

                lerp = 1;
                DoLerp();

                void DoLerp()
                {
                    var targetLerp = animationCurve.Evaluate(lerp);
                    elementIcon.sizeDelta = Vector2.LerpUnclamped(_skillElementInitialSize, targetSize, targetLerp);
                }
            }
        }

        private Vector2 CalculateHoverSize() => _skillElementInitialSize * .6f;

        private void ResetHoverElementState(USkillElementHolder element)
        {
            if(element == null) return;

            var resetSizeElement = element.GetIconHolder().rectTransform;
            resetSizeElement.sizeDelta = _skillElementInitialSize;

        }

        private void SnapHoverElementState(USkillElementHolder element)
        {
            var snapElement = element.GetIconHolder().rectTransform;
            snapElement.sizeDelta = CalculateHoverSize();
        }

        private static void SetActiveSkillElement(USkillElementHolder element)
        {
            var elementBackground = element.GetIconBackgroundColor();
            elementBackground.gameObject.SetActive(true);

            var elementIcon = element.GetIconHolder();
            elementIcon.color = OnActiveIconColor;
        }
        private static void ResetActiveSkillElementState(USkillElementHolder element)
        {
            if(element == null) return;

            var elementBackground = element.GetIconBackgroundColor();
            elementBackground.gameObject.SetActive(false);

            var elementIcon = element.GetIconHolder();
            elementIcon.color = OnInactiveIconColor;
        }



        public void OnPointerClick(USkillElementHolder element, IFullSkill skill)
        {
            if (_currentActiveElement != null)
            {
                ResetActiveSkillElementState(_currentActiveElement);
                if (element == _currentActiveElement)
                {
                    _currentActiveElement = null;
                    return;
                }
            }

            _currentActiveElement = element;
            ElementClickAnimation();
        }
        private USkillElementHolder _currentActiveElement;
        private void ElementClickAnimation()
        {
            if(_currentActiveElement == null) return;
            SetActiveSkillElement(_currentActiveElement);

            Timing.KillCoroutines(_elementAnimationHandle);
            SnapHoverElementState(_currentActiveElement);

            _elementAnimationHandle = Timing.RunCoroutine(_Animation());

            IEnumerator<float> _Animation()
            {
                float lerp = 0;
                var elementBackground = _currentActiveElement.GetIconBackgroundColor();
                elementBackground.gameObject.SetActive(true);

                Vector3 targetScale = new Vector3(.6f,.6f, .6f);

                while (lerp < 1)
                {
                    yield return Timing.WaitForOneFrame;
                    lerp += Timing.DeltaTime * ElementAnimationDeltaSpeed;
                    DoLerp();
                }

                lerp = 1;
                DoLerp();

                void DoLerp()
                {
                    var targetLerp = clickAnimationCurve.Evaluate(lerp);
                    elementBackground.localScale = Vector3.LerpUnclamped(Vector3.one, targetScale, targetLerp);
                }
            }
        }

        [Button,DisableInEditorMode]
        private void TestShowSkill(SPlayerPreparationEntity entity)
        {
            skillListSpawner.Clear();
            SwitchEntity(entity);
        }

        [Serializable]
        private sealed class Spawner : DictionaryPool<IFullSkill, USkillElementHolder>
        {
            [TitleGroup("References")]
            [SerializeField] private RectTransform onPoolParent;

            [TitleGroup("References")] 
            [SerializeField]
            private RectTransform onReleaseParent;

            [TitleGroup("Params")] 
            [SerializeField, SuffixLabel("px"),DisableInPlayMode]
            private float lateralSeparation;



            public RectTransform ElementsHolder => onPoolParent;

            private float _finalElementSeparation;
            public USkillElementHolder.ISkillElementEventsHandler EventsHandler { private get; set; }
            public override void Awake()
            {
                base.Awake();
                onReleaseParent.gameObject.SetActive(false);
                var prefabTransform =  (RectTransform) GetPrefab().transform;
                _finalElementSeparation = prefabTransform.sizeDelta.x + lateralSeparation;
                for (int i = 0; i < _lateralPositions.Length; i++)
                {
                    _lateralPositions[i] = _finalElementSeparation * i;
                }
            }

            public void OnDisable()
            {
                Timing.KillCoroutines(_animationHandle);
            }

            public float TotalSeparation { get; private set; }
            private CoroutineHandle _animationHandle;

            private float[] _lateralPositions = new float[PerStanceSkillAmount];
            private RectTransform[] _skillsTransforms = new RectTransform[PerStanceSkillAmount];
            private const int PerStanceSkillAmount = EnumTeam.PerStanceSkillAmount;

            public void HandleSkills(IEnumerable<IFullSkill> skills)
            {
                int i = 0;
                foreach (var skill in skills)
                {
                    var skillElement = Pop(skill);

                    if(i > _skillsTransforms.Length -1) continue;
                    _skillsTransforms[i] = (RectTransform) skillElement.transform;
                    i++;
                }

                TotalSeparation = (Dictionary.Count + 1) * _finalElementSeparation;
                DoAnimations();
            }

            private void DoAnimations()
            {
                Timing.KillCoroutines(_animationHandle);
                _animationHandle = Timing.RunCoroutine(_Animations());

                IEnumerator<float> _Animations()
                {
                    yield return Timing.WaitForOneFrame;

                    float lerp = 0;
                    const float deltaSpeed = 4;
                    int activeAmount = Mathf.Min(Dictionary.Count, PerStanceSkillAmount);
                    while (lerp < 1)
                    {
                        yield return Timing.WaitForOneFrame;
                        lerp += Timing.DeltaTime * deltaSpeed;
                        LerpAllElements();
                    }

                    lerp = 1;
                    LerpAllElements();

                    void LerpAllElements()
                    {
                        for (int i = 0; i < activeAmount; i++)
                        {
                            var element = _skillsTransforms[i];
                            Vector2 desiredPosition = new Vector2(_lateralPositions[i],0);
                            Vector2 targetPosition =
                                Vector2.LerpUnclamped(element.anchoredPosition, desiredPosition, lerp);
                            element.anchoredPosition = targetPosition;
                        }
                    }
                }
            }


            public override USkillElementHolder Pop(IFullSkill key)
            {
                var element = base.Pop(key);
                var elementTransform = (RectTransform) element.transform;
                elementTransform.SetParent(onPoolParent);
                element.Injection(key);
                element.Injection(EventsHandler);

                elementTransform.anchoredPosition = new Vector2(-_finalElementSeparation * .5f, 0);

                return element;
            }

            protected override void OnRelease(IFullSkill key, USkillElementHolder element)
            {
                base.OnRelease(key, element);
                element.transform.SetParent(onReleaseParent);
            }
        }

    }
}
