using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class USkillButtonsHolder : MonoBehaviour, ITempoEntityStatesListener, ISkillButtonListener
    {
        [Title("References")]
        [SerializeField] 
        private UCombatSkillButton clonableSkillButton;
        private Stack<UCombatSkillButton> _instantiationPool;
        private Dictionary<CombatSkill,UCombatSkillButton> _activeButtons;

        [Title("Parameters")]
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;

        private void Awake()
        {
            var buttonTransform = (RectTransform) clonableSkillButton.transform;
            _buttonSizes = buttonTransform.rect.size;
            _buttonSizes.y = 0; // This is just to avoid the buttons moving upwards in ShowSkills

            _instantiationPool = new Stack<UCombatSkillButton>();
            _activeButtons = new Dictionary<CombatSkill, UCombatSkillButton>();
        }

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.ManualSubscribe(this as ITempoEntityStatesListener);
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
                foreach (var button in _activeButtons)
                {
                    yield return Timing.WaitForSeconds(DelayBetweenButtons);

                    var buttonHolder = button.Value;
                    var buttonTransform = (RectTransform) buttonHolder.transform;


                    Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);

                    buttonTransform.localPosition = lastPoint;
                    buttonTransform.DOLocalMove(targetPoint, AnimationDuration);
                    buttonHolder.ShowButton();


                    lastPoint = targetPoint;
                    index++;
                }
            }
        }

        private void HideSkillInstant()
        {
            foreach (var button in _activeButtons)
            {
                var buttonHolder = button.Value;
                buttonHolder.HideButton();
                buttonHolder.ResetState();
            }
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            var entitySkills = entity.GetCurrentSkills();
            HandlePool(in entitySkills);

            ShowSkillsAnimated();
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            ReturnSkillToStack();
            _activeButtons.Clear();
        }

        private void ReturnSkillToStack()
        {
            foreach (var button in _activeButtons)
            {
                var buttonHolder = button.Value;
                buttonHolder.HideButton();
                buttonHolder.ResetState();
                _instantiationPool.Push(buttonHolder);
            }
        }


        [ShowInInspector]
        private CombatSkill _currentSelectedSkill;
        public void OnSkillSelect(in CombatSkill skill)
        {
            OnSkillSwitch(in skill);
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillSelect(in skill);
        }

        public void OnSkillSwitch(in CombatSkill skill)
        {
            if(skill == null) return; //this prevents null skills and (_currentSelectedSkill = null) == skill check

            if (_currentSelectedSkill == skill)
            {
                _currentSelectedSkill = null;
                OnSkillCancel(in skill);
            }
            else
            {
                _currentSelectedSkill = skill;
                _activeButtons[skill].SelectButton();
                PlayerCombatSingleton.PlayerCombatEvents.OnSkillSwitch(in skill);
            }
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            _activeButtons[skill].DeSelectButton();
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillCancel(in skill);
        }
        public void OnSkillSubmit(in CombatSkill skill)
        {
            DeselectSkill(in _currentSelectedSkill);
            _currentSelectedSkill = null;
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillSubmit(in skill);
        }

        private void DeselectSkill(in CombatSkill skill)
        {
            _activeButtons[skill].DeSelectButton();
        }


        public void OnSkillButtonHover(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonHover(in skill);
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillButtonExit(in skill);
        }
    }
}
