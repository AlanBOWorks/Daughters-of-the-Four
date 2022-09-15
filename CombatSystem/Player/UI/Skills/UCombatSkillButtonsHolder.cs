using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillButtonsHolder : MonoBehaviour, 
        IPlayerCombatEventListener, 

        ITempoControlStatesListener, 
        ISkillUsageListener, ICombatTerminationListener,
        ISkillSelectionListener
    {
        private Dictionary<ICombatSkill,UCombatSkillButton> _activeButtons;

        [Title("Parameters")] 
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;


        [ShowInInspector, HideInEditorMode,DisableInPlayMode]
        private CombatSkill _currentSelectedSkill;
        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        private CombatEntity _currentControlEntity;

        [Title("Skills")] 
        [SerializeField] private SkillElementsSpawner skillElements = new SkillElementsSpawner();

        public ISkillShortcutCommandStructureRead<UCombatSkillButton> GetSkillsShortcutElements() => skillElements;

        [Serializable]
        private sealed class SkillElementsSpawner : ShortCutSkillElementsSpawner<UCombatSkillButton>
        {

        }

        public IReadOnlyDictionary<ICombatSkill, UCombatSkillButton> GetDictionary() => _activeButtons;

        private void Awake()
        {
            _activeButtons = new Dictionary<ICombatSkill, UCombatSkillButton>();

            PreparePrefab();
            PrepareShortCutElements();
        }

        private void PreparePrefab()
        {
            var skillButtonPrefab = skillElements.GetSkillPrefab();
            var buttonTransform = (RectTransform) skillButtonPrefab.transform;
            _buttonSizes = buttonTransform.rect.size;
            _buttonSizes.y = 0; // This is just to avoid the buttons moving upwards in ShowSkills


            //Hide the reference (is used as a visual key for UI Design)
            skillButtonPrefab.gameObject.SetActive(false);
        }

        private void PrepareShortCutElements()
        {
            var shortcutCommandsHolder = CombatShortcutsSingleton.InputActions;
            var shortCutInputActionReferences = shortcutCommandsHolder.SkillShortCuts;


            skillElements.DoInstantiations(OnInstantiateSkillButton);
            void OnInstantiateSkillButton(UCombatSkillButton button, int shortcutIndex)
            {
                var actionReference = shortCutInputActionReferences[shortcutIndex];
                var inputAction = actionReference.action;
                var bidingName = inputAction.GetBindingDisplayString(0);

                button.Injection(this);
                button.SetBindingName(bidingName);

                inputAction.performed += button.OnInputPerformer;
            }
        }


        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;

            
            playerEvents.SubscribeAsPlayerEvent(this);
            playerEvents.ManualSubscribe(this as ICombatTerminationListener);
            playerEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(this);
            playerEvents.DiscriminationEventsHolder.UnSubscribe(this);

            UnSubscribeFromInput();
            void UnSubscribeFromInput()
            {
                var shortcutCommandsHolder = CombatShortcutsSingleton.InputActions;
                var shortCutInputActionReferences = shortcutCommandsHolder.SkillShortCuts;
                var skillButtons = skillElements.SkillShortCuts;
                for (int i = 0; i < skillButtons.Count; i++)
                {
                    var inputReference = shortCutInputActionReferences[i];
                    var button = skillButtons[i];
                    inputReference.action.performed -= button.OnInputPerformer;
                }
            }
        }


        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            HideAll();
        }


        private void EnableHolder()
        {
            enabled = true;
        }

        //safe disable
        public void DisableHolder()
        {
            DisableActiveButtons(); 
            _currentSelectedSkill = null;
            enabled = false;
        }


        private void DisableActiveButtons()
        {
            foreach (var activeButton in _activeButtons)
            {
                activeButton.Value.DoDisabledButton(AnimationDuration);
            }
        }

        // Safe check
        private const int MaxSkillAmount = PrimarySkillAmount + 3;
        private const int PrimarySkillAmount = 4;
        private void HandlePool(IReadOnlyList<CombatSkill> skills)
        {
            int countThreshold = Mathf.Min(skills.Count, MaxSkillAmount);
            var skillShortCutsElements = this.skillElements.SkillShortCuts;
            int i = 0;
            for (; i < countThreshold; i++)
            {
                var skill = skills[i];

                UCombatSkillButton button = skillShortCutsElements[i];

                _activeButtons.Add(skill, button);
                button.Injection(skill);

#if UNITY_EDITOR
                button.name = skill.GetSkillName() + " [BUTTON] (Clone)";
#endif
            }

            for (; i < skillShortCutsElements.Count; i++)
            {
                skillShortCutsElements[i].gameObject.SetActive(false);
            }
        }

        private CoroutineHandle _animationHandle;
        private const float DelayBetweenButtons = .12f;
        private const float AnimationDuration = .24f;

        private EnumTeam.StanceFull _currentStance;
        [Button,DisableInEditorMode]

        private void PoolEntitySkills(CombatEntity entity)
        {
            if(entity == null) return;
            var entitySkills = entity.GetStanceSkills(_currentStance);
            var canAct = UtilsCombatStats.IsControlActive(entity) && CanStanceSwitch();
            HandlePool(entitySkills);

            ShowSkillsAnimated(canAct, true);

            bool CanStanceSwitch()
            {
                var entityTeamValues = entity.Team.DataValues;
                return entityTeamValues.CurrentStance == _currentStance ||
                    entityTeamValues.CurrentControl >= 1;
            }
        }

        private const float OnOffPrimarySkillAmountPassedHeightOffset = 24f;
        private void ShowSkillsAnimated(bool canAct, bool doMoveAnimation)
        {
            Timing.KillCoroutines(_animationHandle);
            _animationHandle = Timing.RunCoroutine(_ShowAll());
            CombatSystemSingleton.LinkCoroutineToMaster(_animationHandle);
            IEnumerator<float> _ShowAll()
            {
                int index = 0;
                yield return Timing.WaitForOneFrame;

                foreach (var button in _activeButtons)
                {
                    var buttonHolder = button.Value;
                    yield return Timing.WaitForSeconds(DelayBetweenButtons);

                    ShowIcon(buttonHolder);

                    index++;
                }


                void ShowIcon(UCombatSkillButton buttonHolder)
                {
                    Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);
                    if (index >= PrimarySkillAmount)
                    {
                        targetPoint.y += OnOffPrimarySkillAmountPassedHeightOffset;
                    }

                    var buttonTransform = buttonHolder.transform;

                    if (doMoveAnimation)
                    {
                        DOTween.Kill(buttonTransform);
                        buttonTransform.localPosition = Vector3.zero;
                        buttonTransform.DOLocalMove(targetPoint, AnimationDuration);
                    }
                    else
                        buttonTransform.localPosition = targetPoint;


                    if (canAct)
                        EnableButton(buttonHolder);
                    else
                        DisableButton(buttonHolder);
                }

            }
        }

        private static void EnableButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = true;
            buttonHolder.ActivateButton(AnimationDuration);
        }

        private static void DisableButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = true;
            buttonHolder.DoDisabledButton(AnimationDuration);
        }

        private static void HideButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = false;
            buttonHolder.HideButton();
            buttonHolder.ResetState();
        }

        

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            EnableHolder();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            DisableHolder();
        }
        
        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            DisableHolder();
        }
        private void ReturnSkillsToStack()
        {
            foreach (var button in _activeButtons)
            {
                var buttonHolder = button.Value;
                HideButton(buttonHolder);
            }
            _activeButtons.Clear();
        }

        private void HideAll()
        {
            ReturnSkillsToStack();
            _currentControlEntity = null;
        }
      
        


        public void SwitchControllingEntity(CombatEntity targetEntity)
        {
            if (targetEntity == _currentControlEntity)
            {
                var canAct = UtilsCombatStats.IsControlActive(targetEntity);
                ShowSkillsAnimated(canAct, false);
                return;
            }
           
            UpdateToEntity(targetEntity);
        }

        private void UpdateToEntity(CombatEntity targetEntity)
        {
            if (_currentSelectedSkill != null)
            {
                DeselectSkill(_currentSelectedSkill);
            }

            _currentControlEntity = targetEntity;
            ReturnSkillsToStack();
            PoolEntitySkills(_currentControlEntity);
        }


        public void DoSkillSelect(CombatSkill skill)
        {
            if (!enabled) return;
            if(PlayerCombatSingleton.IsInPauseMenu) return;


            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnSkillSelect(skill);
            DoSkillSwitch(skill, _currentSelectedSkill);
        }

        private void DoSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            if (skill == null)
                return; //this prevents null skills and (_currentSelectedSkill = null) == skill check

            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;

            if (previousSelection == skill)
            {
                playerEvents.OnSkillDeselect(skill);
            }
            else
            {
                if (_currentSelectedSkill == null)
                {
                    playerEvents.OnSkillSelectFromNull(skill);
                }

                _currentSelectedSkill = skill;
                var button = GetButton(in skill);
                button.SelectButton();
                playerEvents.OnSkillSwitch(skill, previousSelection);
            }
        }
        private void DeselectSkill(ICombatSkill skill)
        {
            if (skill == null || skill != _currentSelectedSkill) return;

            var button = GetButton(in _currentSelectedSkill);
            if (button == null) return;

            button.DeSelectButton();

            _currentSelectedSkill = null;
        }
        private UCombatSkillButton GetButton(in CombatSkill skill)
        {
            return _activeButtons.ContainsKey(skill)
                ? _activeButtons[skill]
                : null;
        }

        public void OnSkillSelect(CombatSkill skill)
        {}
        public void OnSkillSelectFromNull(CombatSkill skill)
        {}
        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {}

        public void OnSkillDeselect(CombatSkill skill)
        {
            DeselectSkill(skill);
        }
        public void OnSkillCancel(CombatSkill skill)
        {
            DeselectSkill(skill);
        }
        public void OnSkillSubmit(CombatSkill skill)
        {
        }



        public void DoSkillButtonHover(CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonHover(skill);
        }

        public void DoSkillButtonExit(CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonExit(skill);
        }

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var usedSkill = values.UsedSkill;
            if (!_activeButtons.ContainsKey(usedSkill)) return;

            _activeButtons[usedSkill].UpdateCostReal();
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            if(performer == null) return;
            SwitchControllingEntity(performer);
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            _currentStance = targetStance;
            UpdateToEntity(_currentControlEntity);
        }
    }
}
