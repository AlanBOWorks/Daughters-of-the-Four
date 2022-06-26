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
        private Dictionary<ICombatSkill,UCombatSkillButton> _activeButtons;


        [Title("Parameters")] 
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;


        [ShowInInspector, DisableInEditorMode,DisableInPlayMode]
        private CombatSkill _currentSelectedSkill;
        [ShowInInspector, DisableInEditorMode,DisableInPlayMode]
        private CombatEntity _currentControlEntity;


        public IReadOnlyDictionary<ICombatSkill, UCombatSkillButton> GetDictionary() => _activeButtons;

        private void Awake()
        {
            var buttonTransform = (RectTransform) clonableSkillButton.transform;
            _buttonSizes = buttonTransform.rect.size;
            _buttonSizes.y = 0; // This is just to avoid the buttons moving upwards in ShowSkills

            _instantiationPool = new Stack<UCombatSkillButton>();
            _activeButtons = new Dictionary<ICombatSkill, UCombatSkillButton>();

            //Hide the reference (is used as a visual key for UI Design)
            clonableSkillButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;

            
            playerEvents.SubscribeAsPlayerEvent(this);
            playerEvents.ManualSubscribe(this as ICombatStatesListener);
            playerEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(this);
            playerEvents.DiscriminationEventsHolder.UnSubscribe(this);
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
                activeButton.Value.DoShowDisabledButton();
            }
        }

        // Safe check
        private const int MaxSkillAmount = 12;
        private void HandlePool(IReadOnlyList<CombatSkill> skills)
        {
            int countThreshold = Mathf.Min(skills.Count, MaxSkillAmount);
            for (var i = 0; i < countThreshold; i++)
            {
                var skill = skills[i];

                UCombatSkillButton button = _instantiationPool.Count > 0 
                    ? _instantiationPool.Pop() 
                    : InstantiateButton();

                _activeButtons.Add(skill, button);
                button.Injection(skill);

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

        private void PoolEntitySkills(CombatEntity entity)
        {
            if(entity == null) return;
            var entitySkills = entity.GetCurrentSkills();
            var canAct = UtilsCombatStats.IsControlActive(entity);
            HandlePool(entitySkills);

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

                        ShowIcon(buttonHolder);

                        index++;
                    }


                    void ShowIcon(UCombatSkillButton buttonHolder)
                    {
                        Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);

                        var buttonTransform = (RectTransform)buttonHolder.transform;

                        buttonTransform.localPosition = lastPoint;
                        buttonTransform.DOLocalMove(targetPoint, AnimationDuration);

                        if(canAct)
                            EnableButton(buttonHolder);
                        else
                            DisableButton(buttonHolder);

                        lastPoint = targetPoint;
                    }

                }
            }
        }

        private static void EnableButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = true;
            buttonHolder.DoShowActiveButton();
        }

        private static void DisableButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = true;
            buttonHolder.DoShowDisabledButton();
        }

        private static void HideButton(UCombatSkillButton buttonHolder)
        {
            buttonHolder.enabled = false;
            buttonHolder.HideButton();
            buttonHolder.ResetState();
        }

        

        public void OnTempoStartControl(CombatTeamControllerBase controller)
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

        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
            ResetPoolSkillsToCurrent();
        }

        public void OnControlChange(CombatTeam team, float phasedControl, bool isBurst)
        {
        }
        private void ReturnSkillsToStack()
        {
            foreach (var button in _activeButtons)
            {
                var buttonHolder = button.Value;
                HideButton(buttonHolder);
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
            PoolEntitySkills(_currentControlEntity);
        }
        


        public void SwitchControllingEntity(CombatEntity targetEntity)
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
            DeselectSkill(skill);
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
            DeselectSkill(usedSkill);
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

    }
}
