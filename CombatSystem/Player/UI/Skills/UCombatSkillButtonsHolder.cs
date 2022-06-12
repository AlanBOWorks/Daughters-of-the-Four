using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillButtonsHolder : MonoBehaviour, 
        IPlayerEntityListener, 

        ITempoTeamStatesListener, ITeamEventListener,
        ISkillUsageListener, ICombatStatesListener,
        ISkillSelectionListener
    {
        [Title("References")]
        [SerializeField] 
        private UCombatSkillButton clonableSkillButton;
        [SerializeField] private Transform onParent;

        private Stack<UCombatSkillButton> _instantiationPool;
        private Dictionary<CombatSkill,UCombatSkillButton> _activeButtons;


        [Title("Parameters")] 
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;


        [ShowInInspector]
        private CombatSkill _currentSelectedSkill;
        [ShowInInspector]
        private CombatEntity _currentControlEntity;


        public IReadOnlyDictionary<CombatSkill, UCombatSkillButton> GetDictionary() => _activeButtons;

        private void Awake()
        {
            var buttonTransform = (RectTransform) clonableSkillButton.transform;
            _buttonSizes = buttonTransform.rect.size;
            _buttonSizes.y = 0; // This is just to avoid the buttons moving upwards in ShowSkills

            _instantiationPool = new Stack<UCombatSkillButton>();
            _activeButtons = new Dictionary<CombatSkill, UCombatSkillButton>();

            //Hide the reference (is used as a visual key for UI Design)
            clonableSkillButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;

            // PROBLEM: stackOverFlow in hoverSkill;
            // SOLUTION: manual subscriptions
            playerEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            HideAll();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        private void EnableHolder()
        {
            enabled = true;
        }

        public void DisableHolder()
        {
            DisableActiveButtons();
            _currentSelectedSkill = null;
            _currentControlEntity = null;
            enabled = false;
        }


        private void DisableActiveButtons()
        {
            foreach (var activeButton in _activeButtons)
            {
                activeButton.Value.DisableButton();
            }
        }

        // Safe check
        private const int MaxSkillAmount = 12;
        private void HandlePool(in IReadOnlyList<CombatSkill> skills)
        {
            int countThreshold = Mathf.Min(skills.Count, MaxSkillAmount);
            for (var i = 0; i < countThreshold; i++)
            {
                var skill = skills[i];

                UCombatSkillButton button = _instantiationPool.Count > 0 
                    ? _instantiationPool.Pop() 
                    : InstantiateButton();

                _activeButtons.Add(skill, button);
                button.Injection(in skill);

#if UNITY_EDITOR
                button.name = skill.GetSkillName() + " [BUTTON] (Clone)";
#endif

            }
        }

        private UCombatSkillButton InstantiateButton()
        {
            var button = Instantiate(clonableSkillButton, onParent);
            button.Injection(this);

            return button;
        }

        private CoroutineHandle _animationHandle;
        private const float DelayBetweenButtons = .12f;
        private const float AnimationDuration = .2f;
        [Button,DisableInEditorMode]

        private void PoolEntitySkills(in CombatEntity entity)
        {
            if(entity == null) return;
            var entitySkills = entity.GetCurrentSkills();
            HandlePool(in entitySkills);

            ShowSkillsAnimated();

            void ShowSkillsAnimated()
            {
                Timing.KillCoroutines(_animationHandle);
                _animationHandle = Timing.RunCoroutine(_ShowAll());
                CombatSystemSingleton.LinkCoroutineToMaster(_animationHandle);
                IEnumerator<float> _ShowAll()
                {
                    int index = 0;
                    Vector2 lastPoint = Vector2.zero;

                    yield return Timing.WaitForOneFrame;

                    foreach (var button in _activeButtons)
                    {
                        var buttonHolder = button.Value;
                        yield return Timing.WaitForSeconds(DelayBetweenButtons);

                        ShowIcon(in buttonHolder);

                        index++;
                    }


                    void ShowIcon(in UCombatSkillButton buttonHolder)
                    {
                        Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);

                        var buttonTransform = (RectTransform)buttonHolder.transform;

                        buttonTransform.localPosition = lastPoint;
                        buttonTransform.DOLocalMove(targetPoint, AnimationDuration);
                        EnableButton(in buttonHolder);


                        lastPoint = targetPoint;
                    }

                }
            }
        }

        private static void EnableButton(in UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = true;
            buttonHolder.ShowButton();
        }
        private static void DisableButton(in UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = false;
            buttonHolder.HideButton();
            buttonHolder.ResetState();
        }





        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            EnableHolder();
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            DisableHolder();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            HideAll();
        }

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            ResetPoolSkillsToCurrent();
        }

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
        }
        private void ReturnSkillsToStack()
        {
            foreach (var button in _activeButtons)
            {
                var buttonHolder = button.Value;
                DisableButton(in buttonHolder);
                _instantiationPool.Push(buttonHolder);
            }
            _activeButtons.Clear();
        }

        private void HideAll()
        {
            ReturnSkillsToStack();
            _currentControlEntity = null;
        }
        private void ResetPoolSkillsToCurrent()
        {
            ReturnSkillsToStack();
            PoolEntitySkills(in _currentControlEntity);
        }
        


        public void SwitchControllingEntity(in CombatEntity targetEntity)
        {
            if (_currentSelectedSkill != null)
            {
                DeselectSkill(_currentSelectedSkill);
            }

            _currentControlEntity = targetEntity;
            ResetPoolSkillsToCurrent();
        }


        public void DoSkillSelect(CombatSkill skill)
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnSkillSelect(skill);
            DoSkillSwitch(in skill, in _currentSelectedSkill);
        }

        private void DoSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
        {
            if (!enabled)
                return;

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
        private void DeselectSkill(CombatSkill skill)
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
            DeselectSkill(skill);
        }



        public void DoSkillButtonHover(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonHover(in skill);
        }

        public void DoSkillButtonExit(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonExit(in skill);
        }

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var usedSkill = values.UsedSkill;
            if (!_activeButtons.ContainsKey(usedSkill)) return;

            _activeButtons[usedSkill].UpdateCostReal();
            DeselectSkill(usedSkill);
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            if(performer == null) return;
            SwitchControllingEntity(in performer);
        }

    }
}
