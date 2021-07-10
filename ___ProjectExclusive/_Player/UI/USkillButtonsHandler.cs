using System;
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
            InjectBehaviour();

            void InjectBehaviour()
            {
                foreach (USkillButton uSkillButton in skillButtons)
                {
                    uSkillButton.Behaviour = this.buttonsBehaviour;
                }
            }
        }

        [TitleGroup("Params")]
        private void UpdateCharacterSkills(CombatingEntity entity)
        {
            List<CombatSkill> uniqueSkills = UtilsSkill.GetSkillsByTeamState(entity);
            _skillButtons.Clear();
            if(uniqueSkills == null) return;
            int skillsAmount = uniqueSkills.Count;

            int i;
            // Show
            for (i = 0; i < skillsAmount; i++)
            {
                var skill = uniqueSkills[i];
                var button = skillButtons[i];
                button.Injection(entity,skill);
                button.gameObject.SetActive(true);
                _skillButtons.Add(skill,button);
            }
            // Hide the rest
            for (; i < skillButtons.Count; i++)
            {
                skillButtons[i].gameObject.SetActive(false);
            }

        }

        public void EnableButtons(CombatingEntity entity)
        {
            UpdateCharacterSkills(entity);
            gameObject.SetActive(true);
            //TODO show animation
        }

        public void DisableButtons(CombatingEntity entity)
        {
            Debug.Log("Trigger disable");
            gameObject.SetActive(false);
            //TODO hide animation
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            EnableButtons(entity);
        }

        public void OnActionDone(CombatingEntity entity)
        {
            OnFinisAllActions(entity);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            DisableButtons(entity);
        }

        
    }

}
