using CombatEntity;
using CombatSystem.Events;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class USkillButtonsHolder : MonoBehaviour, ITempoListener<CombatingEntity>
    {
        [SerializeField] private USkillButton[] skillButtons = new USkillButton[0];

        private void Awake()
        {
            PlayerCombatSingleton.PlayerEvents.Subscribe(this);
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            var entitySkills = entity.SkillsHolder.GetCurrentSkills().MainSkillTypes;

            int maxIteration = Mathf.Min(entitySkills.Count, skillButtons.Length);
            int i;
            for (i = 0; i < maxIteration; i++)
            {
                var skill = entitySkills[i];
                var button = skillButtons[i];

                button.Injection(skill);
                button.gameObject.SetActive(true);
            }

            for (;i < skillButtons.Length; i++)
            {
                var button = skillButtons[i];
                button.gameObject.SetActive(false);
            }
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            foreach (var skillButton in skillButtons)
            {
                if(skillButton.gameObject.activeSelf)
                skillButton.ForceUpdate();
            }
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            foreach (var skillButton in skillButtons)
            {
                skillButton.ResetState();
            }
        }

        public void OnSkipActions(CombatingEntity element)
        {
        }
    }
}
