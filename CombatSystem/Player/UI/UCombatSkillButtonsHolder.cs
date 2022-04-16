using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.Skills;
using CombatSystem.Skills;
using CombatSystem.Team;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillButtonsHolder : MonoBehaviour, 
        ITempoEntityStatesListener, ITeamEventListener,
        ISkillButtonListener, ISkillUsageListener, ICombatStatesListener
    {
        [Title("References")]
        [SerializeField] 
        private UCombatSkillButton clonableSkillButton;
        private Stack<UCombatSkillButton> _instantiationPool;
        private Dictionary<CombatSkill,UCombatSkillButton> _activeButtons;
        [SerializeField] 
        private UCombatWaitSkill waitSkill;

        [Title("Parameters")]
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;

        private CombatEntity _currentControlEntity;


        internal void AddToDictionary(in CombatSkill skill, in UCombatSkillButton button)
        {
            if(_activeButtons.ContainsKey(skill)) return;
            _activeButtons.Add(skill,button);
        }


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
            playerEvents.ManualSubscribe(this as ITempoEntityStatesListener);
            playerEvents.ManualSubscribe(this as ISkillUsageListener);
            playerEvents.ManualSubscribe(this as ITeamEventListener);
            playerEvents.ManualSubscribe(this as ICombatStatesListener);
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            HideAll();
        }

        public void OnCombatQuit()
        {
            HideAll();
        }


        // Safe check
        private const int MaxSkillAmount = 12;
        private void HandlePool(in IReadOnlyList<CombatSkill> skills)
        {
            int countThreshold = Mathf.Min(skills.Count, MaxSkillAmount);
            for (var i = 0; i < countThreshold; i++)
            {
                var skill = skills[i];

                UCombatSkillButton button;
                if (_instantiationPool.Count > 0)
                    button = _instantiationPool.Pop();
                else
                    button = InstantiateButton();

                _activeButtons.Add(skill, button);
                button.Injection(in skill);

#if UNITY_EDITOR
                button.name = skill.GetSkillName() + " [BUTTON] (Clone)";
#endif

            }
        }

        private UCombatSkillButton InstantiateButton()
        {
            var button = Instantiate(clonableSkillButton, transform);
            button.Injection(this);

            return button;
        }


        private const float DelayBetweenButtons = .12f;
        private const float AnimationDuration = .2f;
        [Button,DisableInEditorMode]
        private void ShowSkillsAnimated()
        {
            CombatSystemSingleton.LinkCoroutineToMaster(_ShowAll());
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
                HandleWaitSkill();
                yield return Timing.WaitForSeconds(DelayBetweenButtons);

                void ShowIcon(in UCombatSkillButton buttonHolder)
                {
                    Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);

                    var buttonTransform = (RectTransform)buttonHolder.transform;

                    buttonTransform.localPosition = lastPoint;
                    buttonTransform.DOLocalMove(targetPoint, AnimationDuration);
                    EnableButton(in buttonHolder);


                    lastPoint = targetPoint;
                }

                void HandleWaitSkill()
                {
                    var waitButton = waitSkill.GetButton();
                    waitSkill.InjectIntoButton();
                    waitSkill.ResetCost();
                    ShowIcon(in waitButton);

                }
            }
        }

        private void PoolEntitySkills(in CombatEntity entity)
        {
            var entitySkills = entity.GetCurrentSkills();
            HandlePool(in entitySkills);

            ShowSkillsAnimated();
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
        
        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            _currentControlEntity = entity;
            PoolEntitySkills(in entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            _currentControlEntity = entity;
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            if(entity != _currentControlEntity) return;
            
            HideAll();
        }

        public void OnEntityWaitSequence(CombatEntity entity)
        {
        }

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            ReturnSkillsToStack();
            PoolEntitySkills(in _currentControlEntity);
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
            _currentControlEntity = null;
            ReturnSkillsToStack();
            DisableButton(waitSkill.GetButton());
        }


        [ShowInInspector]
        private CombatSkill _currentSelectedSkill;
        public void OnSkillSelect(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillSelect(in skill);
            OnSkillSwitch(in skill, in _currentSelectedSkill);
        }

        private UCombatSkillButton GetButton(in CombatSkill skill)
        {
            if (_activeButtons.ContainsKey(skill)) return _activeButtons[skill];

            var waitCombatSkill = waitSkill.GetSkill();
            return skill == waitCombatSkill 
                ? waitSkill.GetButton() 
                : null;
        }

        public void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection)
        {
            if(skill == null) return; //this prevents null skills and (_currentSelectedSkill = null) == skill check

            if (previousSelection == skill)
            {
                _currentSelectedSkill = null;
                OnSkillDeselect(in skill);
            }
            else
            {
                _currentSelectedSkill = skill;
                var button = GetButton(in skill);
                button.SelectButton();
                PlayerCombatSingleton.PlayerCombatEvents.OnSkillSwitch(in skill, previousSelection);
            }
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            var button = GetButton(in skill);
            button.DeSelectButton();
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillDeselect(in skill);
        }
        public void OnSkillCancel(in CombatSkill skill)
        {
            DeselectSkill(in skill);
        }
        public void OnSkillSubmit(in CombatSkill skill)
        {
            DeselectSkill(in skill);
        }

        private void DeselectSkill(in CombatSkill skill)
        {
            var button = GetButton(in skill);
            button.DeSelectButton();

            if(skill == _currentSelectedSkill)
                _currentSelectedSkill = null;
        }


        public void OnSkillButtonHover(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonHover(in skill);
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonExit(in skill);
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            _currentSelectedSkill = null;
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            if(_activeButtons.ContainsKey(usedSkill))
                _activeButtons[usedSkill].UpdateCostReal();
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish()
        {
        }

    }
}