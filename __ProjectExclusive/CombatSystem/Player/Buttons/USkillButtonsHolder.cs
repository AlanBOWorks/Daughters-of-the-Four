using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class USkillButtonsHolder : MonoBehaviour, IVirtualSkillInjectionListener, IVirtualSkillInteraction,
        ISkillGroupTypesRead<List<USkillButton>>,
        ITempoListener<CombatingEntity>
    {
        [SerializeField, HorizontalGroup()] private List<USkillButton> mainSkillButtons = new List<USkillButton>();
        [SerializeField, HorizontalGroup()] private List<USkillButton> sharedSkillButtons = new List<USkillButton>();
        public List<USkillButton> SharedSkillTypes => sharedSkillButtons;
        public List<USkillButton> MainSkillTypes => mainSkillButtons;

        private SelectionHandler _hoverHandler;
        private SelectionHandler _selectionHandler;
        private USkillButton _currentHoverButton;
        private USkillButton _currentSelectedButton;

#if UNITY_EDITOR
        [SerializeField] private bool debugSkillSelection = true;
#endif


        private void Awake()
        {
            _hoverHandler = new SelectionHandler();
            _selectionHandler = new SelectionHandler();

            var playerEvents = PlayerCombatSingleton.PlayerEvents;

            playerEvents.Subscribe(this as ITempoListener<CombatingEntity>);
            playerEvents.Subscribe(this as IVirtualSkillInjectionListener);
            playerEvents.Subscribe(this as IVirtualSkillInteraction);

            InjectIntoButtons(mainSkillButtons);
            InjectIntoButtons(sharedSkillButtons);
            void InjectIntoButtons(List<USkillButton> buttons)
            {
                foreach (var button in buttons)
                {
                    button.Injection(this);
                }
            }
        }
        

        public void OnInjectionSkills(CombatingEntity entity, ISkillGroupTypesRead<List<PlayerVirtualSkill>> skillGroup)
        {
            UtilSkills.DoActionOn(this,skillGroup,UpdateButtons);

            void UpdateButtons(List<USkillButton> buttons, List<PlayerVirtualSkill> skills)
            {
                int maxIteration = Mathf.Min(buttons.Count, skills.Count);
                int i;
                for (i = 0; i < maxIteration; i++)
                {
                    var skill = skills[i];
                    var button = buttons[i];

                    button.Injection(skill);
                    button.gameObject.SetActive(true);
                }

                for (; i < mainSkillButtons.Count; i++)
                {
                    var button = mainSkillButtons[i];
                    button.gameObject.SetActive(false);
                }
            }
        }

        public void OnHover(USkillButton button)
        {
            // This is a safety check (hover only happens once, but something could go wrong with Unity)
            if (button == _currentHoverButton)
                return;

            if (_currentHoverButton != null)
            {
                ExitHoverCurrent();
            }

            _currentHoverButton = button;
            _hoverHandler.InjectButton(button);

            PlayerCombatSingleton.PlayerEvents.OnHover(_hoverHandler);
        }
        private void ExitHoverCurrent()
        {
            PlayerCombatSingleton.PlayerEvents.OnHoverExit(_hoverHandler);
        }
        public void OnHoverExit(USkillButton button)
        {
            // This is a safety check (hover only happens once and should be the same button,
            // but something could go wrong with Unity)
            if(button != _currentHoverButton) return;

            ExitHoverCurrent();
        }

        public void OnSelect(USkillButton button)
        {
            if (button == _currentSelectedButton)
            {
                DeselectCurrent();
                _currentSelectedButton = null;
                return;
            }

            if (_currentSelectedButton != null)
            {
                DeselectCurrent();
            }

            _currentSelectedButton = button;
            _selectionHandler.InjectButton(button);

            PlayerCombatSingleton.PlayerEvents.OnSelect(_selectionHandler);

#if UNITY_EDITOR
            if (debugSkillSelection)
                Debug.Log($"Select: {_currentSelectedButton.GetSkill().CurrentSkill.Preset.GetSkillName()}");
#endif


            void DeselectCurrent()
            {
                PlayerCombatSingleton.PlayerEvents.OnDeselect(_selectionHandler);

#if UNITY_EDITOR
                if (debugSkillSelection)
                    Debug.Log($"DeSelect: {_currentSelectedButton.GetSkill().CurrentSkill.Preset.GetSkillName()}");
#endif
            }
        }



        public void OnInitiativeTrigger(CombatingEntity element)
        {
            _selectionHandler.User = element;
            _hoverHandler.User = element;
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            _selectionHandler.User = null;
            _hoverHandler.User = null;

        }

        public void OnSkipActions(CombatingEntity element)
        {
            _selectionHandler.User = null;
            _hoverHandler.User = null;
        }

        public void OnSelect(IVirtualSkillSelection selection)
        {
            _currentSelectedButton.OnSelect();
        }

        public void OnDeselect(IVirtualSkillSelection selection)
        {
            _currentSelectedButton.OnDeselect();
        }

        public void OnSubmit(IVirtualSkillSelection selection)
        {
            _currentSelectedButton.OnSubmit();
            _selectionHandler.ResetSkillValues();
        }

        public void OnHover(IVirtualSkillSelection selection)
        {
        }

        public void OnHoverExit(IVirtualSkillSelection selection)
        {
            
        }


        private class SelectionHandler : IVirtualSkillSelection
        {
            public CombatingEntity User { get; set; }
            public PlayerVirtualSkill PrioritySkill { get; private set; }
            public List<CombatingEntity> PossibleTargets { get; private set; }


            public void InjectButton(USkillButton button)
            {
                PrioritySkill = button.GetSkill();
                PossibleTargets = UtilsTarget.GetPossibleTargets(User, PrioritySkill.CurrentSkill.GetTargetType());
            }

            public void ResetSkillValues()
            {
                PrioritySkill = null;
                PossibleTargets = null;
            }
        }
    }
}
