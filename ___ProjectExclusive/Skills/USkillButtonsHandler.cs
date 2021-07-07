using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using _Player;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace Skills
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

        [NonSerialized,ShowInInspector,DisableInEditorMode,DisableInPlayMode] 
        public USkillButton currentSelectedButton;

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
            if(uniqueSkills == null) return;
            int skillsAmount = uniqueSkills.Count;

            int i;
            // Show
            for (i = 0; i < skillsAmount; i++)
            {
                skillButtons[i].Injection(entity,uniqueSkills[i]);
                skillButtons[i].gameObject.SetActive(true);
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

    public class CharacterSkillButtonsHandler { }
}
