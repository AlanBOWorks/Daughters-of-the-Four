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

        private CombatingEntity _currentUser;
       
        
        private USkillButton _currentHoverButton;
        private USkillButton _currentSelectedButton;

#if UNITY_EDITOR
        [SerializeField] private bool debugSkillSelection = true;
#endif


        private void Awake()
        {
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

        private VirtualSkillSelection HandleSkillSelection(USkillButton button)
        {
            var selectedSkill = button.GetSkill();
            var possibleTargets =
                UtilsTarget.GetPossibleTargets(_currentUser, selectedSkill.CurrentSkill.GetTargetType());

            return new VirtualSkillSelection(selectedSkill,possibleTargets);
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

        private VirtualSkillSelection _hoverSelection;
        public void OnHover(USkillButton button)
        {
            // This is a safety check (hover only happens once, but something could go wrong with Unity)
            if (button == _currentHoverButton)
                return;


            if (_currentHoverButton != null)
            {
                ExitHoverCurrent();
            }

            _hoverSelection = HandleSkillSelection(button);
            _currentHoverButton = button;

            PlayerCombatSingleton.PlayerEvents.OnHover(_hoverSelection);
        }
        private void ExitHoverCurrent()
        {
            PlayerCombatSingleton.PlayerEvents.OnHoverExit(_hoverSelection);
        }
        public void OnHoverExit(USkillButton button)
        {
            // This is a safety check (hover only happens once and should be the same button,
            // but something could go wrong with Unity)
            if(button != _currentHoverButton) return;

            ExitHoverCurrent();
        }

        private VirtualSkillSelection _clickSelection;
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
            _clickSelection = HandleSkillSelection(_currentHoverButton);

            PlayerCombatSingleton.PlayerEvents.OnSelect(_clickSelection);

#if UNITY_EDITOR
            if (debugSkillSelection)
                Debug.Log($"Select: {_currentSelectedButton.GetSkill().CurrentSkill.Preset.GetSkillName()}");
#endif


            void DeselectCurrent()
            {
                PlayerCombatSingleton.PlayerEvents.OnDeselect(_clickSelection);

#if UNITY_EDITOR
                if (debugSkillSelection)
                    Debug.Log($"DeSelect: {_currentSelectedButton.GetSkill().CurrentSkill.Preset.GetSkillName()}");
#endif
            }
        }



        public void OnInitiativeTrigger(CombatingEntity element)
        {
            _currentUser = element;
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            _currentUser = null;
        }

        public void OnSkipActions(CombatingEntity element)
        {
            _currentUser = null;
        }

        public void OnSelect(VirtualSkillSelection selection)
        {
            _currentSelectedButton.OnSelect();
        }

        public void OnDeselect(VirtualSkillSelection selection)
        {
            _currentSelectedButton.OnDeselect();
        }

        public void OnSubmit(VirtualSkillSelection selection)
        {
            _currentSelectedButton.OnSubmit();
        }

        public void OnHover(VirtualSkillSelection selection)
        {

        }

        public void OnHoverExit(VirtualSkillSelection selection)
        {
            
        }


    }
}
