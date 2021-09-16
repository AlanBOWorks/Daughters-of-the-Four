using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace _Player
{
    public class USkillButtonsHandler : MonoBehaviour, ISharedSpecialSkills<USkillButton>, ITempoListener
    {
        [TitleGroup("Params")] 
        [SerializeField]
        private SkillButtonBehaviour buttonsBehaviour = new SkillButtonBehaviour();

        [Title("Common")]
        [SerializeField] private USkillButton ultimateButton = null;
        [SerializeField] private USkillButton commonFirst = null;
        [SerializeField] private USkillButton commonSecondary = null;
        [SerializeField] private USkillButton waitButton = null;
        [Title("Unique")]
        [SerializeField] private List<USkillButton> skillButtons = new List<USkillButton>();

        public USkillButton UltimateSkill => ultimateButton;
        public USkillButton CommonSkillFirst => commonFirst;
        public USkillButton CommonSkillSecondary => commonSecondary;
        public USkillButton WaitSkill => waitButton;
        public List<USkillButton> AllSkills => skillButtons;


        [ShowInInspector]
        private Dictionary<CombatSkill, USkillButton> _skillButtons;

        [Button ("Serialize Children"),HideInPlayMode]
        private void SerializeButtons()
        {
            skillButtons.AddRange(GetComponentsInChildren<USkillButton>());
        }

        private void Start()
        {
            buttonsBehaviour.Handler = this;
            var combatEvents = PlayerEntitySingleton.PlayerCombatEvents;
            combatEvents.Subscribe(this);
            
            int predictedAmountOfButtons = UtilsCombatStats.PredictedAmountOfSkillsPerState;
            _skillButtons 
                = new Dictionary<CombatSkill, USkillButton>(predictedAmountOfButtons);

            PlayerEntitySingleton.SkillButtonsDictionary = _skillButtons;
            PlayerEntitySingleton.PlayerCombatEvents.Subscribe(this);

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
                waitButton.Behaviour = this.buttonsBehaviour;
            }
        }


        private void InjectUniqueSkills(CombatingEntity entity)
        {
            List<CombatSkill> uniqueSkills = UtilsSkill.GetUniqueByStance(entity);
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
        private void InjectSharedSkills(CombatingEntity entity)
        {
            if (_sharedParsingAction == null) 
                _sharedParsingAction = DoParsingInjection;

            ISharedSkills<CombatSkill> skillShared = entity.CombatSkills.GetCurrentSharedSkills();
            UtilsSkill.DoParse(skillShared,this, _sharedParsingAction);

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

        private void InjectSpecialSkills(CombatingEntity entity)
        {
            ISpecialSkills<CombatSkill> skills = entity.CombatSkills;
            InjectSkillOnButton(entity,skills.WaitSkill,waitButton);

            var ultimateSkill = skills.UltimateSkill;
            if(ultimateSkill != null)
                InjectSkillOnButton(entity,ultimateSkill,ultimateButton);
            else
            {
                ultimateButton.Hide();
            }

        }

        private void InjectSkillOnButton(CombatingEntity entity,CombatSkill skill, USkillButton button)
        {
            button.UpdateSkill(entity, skill);
            _skillButtons.Add(skill, button);
            button.UpdateCooldown();
            button.Show();
        }

        private void ShowButtons()
        {
            gameObject.SetActive(true);
            foreach (KeyValuePair<CombatSkill, USkillButton> pair in _skillButtons)
            {
                var button = pair.Value;
                button.Show();
            }
        }

        private void ShowAndUpdateButtons()
        {
            gameObject.SetActive(true);
            foreach (KeyValuePair<CombatSkill, USkillButton> pair in _skillButtons)
            {
                var button = pair.Value;
                button.UpdateCooldown();
                button.Show();
            }
        }

        private void HideButtons()
        {
            gameObject.SetActive(false);

            foreach (KeyValuePair<CombatSkill, USkillButton> button in _skillButtons)
            {
                button.Value.Hide();
            }
            //TODO hide animation
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _skillButtons.Clear();
            InjectUniqueSkills(entity);
            InjectSharedSkills(entity);
            InjectSpecialSkills(entity);
            ShowButtons();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            ShowAndUpdateButtons();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            HideButtons();
        }
    }

}
