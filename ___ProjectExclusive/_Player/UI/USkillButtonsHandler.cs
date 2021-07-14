﻿using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using _Player;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Player
{
    public class USkillButtonsHandler : MonoBehaviour, IEquipSkill<USkillButton>, IPlayerTempoListener
    {
        [TitleGroup("Params")] 
        [SerializeField]
        private SkillButtonBehaviour buttonsBehaviour = new SkillButtonBehaviour();

        [Title("Common")]
        [SerializeField] private USkillButton ultimateButton = null;
        [SerializeField] private USkillButton commonFirst = null;
        [SerializeField] private USkillButton commonSecondary = null;
        [Title("Unique")]
        [SerializeField] private List<USkillButton> skillButtons = new List<USkillButton>();

        public USkillButton UltimateSkill => ultimateButton;
        public USkillButton CommonSkillFirst => commonFirst;
        public USkillButton CommonSkillSecondary => commonSecondary;
        public List<USkillButton> UniqueSkills => skillButtons;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private USkillButton _currentSelectedButton;
        public USkillButton CurrentSelectedButton
        {
            get => _currentSelectedButton;
            set
            {
                _currentSelectedButton = value;
                if (_currentSelectedButton == null)
                {
                    PlayerEntitySingleton.TargetsHandler.HideSkillTargets();
                }
                else
                {
                    PlayerEntitySingleton.TargetsHandler.ShowSkillTargets(value.CurrentSkill);
                }
            }
        }

        [ShowInInspector]
        private Dictionary<CombatSkill, USkillButton> _skillButtons;
        public USkillButton GetButton(CombatSkill skill) => _skillButtons[skill];

        [Button ("Serialize Children"),HideInPlayMode]
        private void SerializeButtons()
        {
            skillButtons.AddRange(GetComponentsInChildren<USkillButton>());
        }



        private void Awake()
        {
            buttonsBehaviour.Handler = this;
            PlayerEntitySingleton.SkillButtonsHandler = this;
            CombatSystemSingleton.TempoHandler.Subscribe(this);
            int predictedAmountOfButtons = UtilsSkill.PredictedAmountOfSkillsPerState;
            _skillButtons 
                = new Dictionary<CombatSkill, USkillButton>(predictedAmountOfButtons);

            gameObject.SetActive(false);
            InjectBehaviourUnique();
            InjectBehaviourShared();

            void InjectBehaviourUnique()
            {
                foreach (USkillButton uSkillButton in skillButtons)
                {
                    uSkillButton.Behaviour = this.buttonsBehaviour;
                }
            }

            void InjectBehaviourShared()
            { 
                ultimateButton.Behaviour = this.buttonsBehaviour;
                commonFirst.Behaviour = this.buttonsBehaviour;
                commonSecondary.Behaviour = this.buttonsBehaviour;
            }
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            UpdateUniqueSkills(entity);
            UpdateSharedSkills(entity);
            ShowButtons();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            UpdateUniqueSkills(entity);
            UpdateSharedSkills(entity);
            ShowButtons();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            HideButtons();
        }

        private void UpdateUniqueSkills(CombatingEntity entity)
        {
            List<CombatSkill> uniqueSkills = UtilsSkill.GetSkillsByTeamState(entity);
            _skillButtons.Clear();
            if(uniqueSkills == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Character without Skills is added to the pool");
#endif

                HideFrom(0);
                return;
            }
            int skillsAmount = uniqueSkills.Count;

            int i = 0;
            // Show
            for (; i < skillsAmount; i++)
            {
                var skill = uniqueSkills[i];
                var button = skillButtons[i];
                InjectSkillOnButton(entity,skill,button);
            }
            // Hide the rest
            HideFrom(i);

            void HideFrom(int hideIndex)
            {
                for (; hideIndex < skillButtons.Count; hideIndex++)
                {
                    skillButtons[hideIndex].Hide();
                }
            }
        }

        private Action<CombatSkill, USkillButton> _sharedParsingAction;
        private void UpdateSharedSkills(CombatingEntity entity)
        {
            if (_sharedParsingAction == null) 
                _sharedParsingAction = DoParsingInjection;

            ISkillShared<CombatSkill> skillShared = entity.SharedSkills;
            SharedCombatSkills.DoParse(skillShared,this, _sharedParsingAction);

            void DoParsingInjection(CombatSkill skill, USkillButton button)
            {
                if (skill == null || skill.Preset == null)
                {
                    button.Hide();
                    return;
                }
                InjectSkillOnButton(entity,skill,button);
            }
        }

        private void InjectSkillOnButton(CombatingEntity entity,CombatSkill skill, USkillButton button)
        {
            button.Injection(entity, skill);
            _skillButtons.Add(skill, button);
            button.Show();

        }

        public void ShowButtons()
        {
            gameObject.SetActive(true);
            //TODO show animation
        }

        public void HideButtons()
        {
            gameObject.SetActive(false);
            //TODO hide animation
        }


        
    }

}
