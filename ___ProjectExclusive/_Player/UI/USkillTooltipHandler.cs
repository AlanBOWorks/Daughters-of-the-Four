using Sirenix.OdinInspector;
using Skills;
using TMPro;
using UnityEngine;

namespace _Player
{
    public class USkillTooltipHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillName = null;
        [SerializeField] private TextMeshProUGUI cooldown = null;
        [SerializeField] private TextMeshProUGUI basicDescription = null;

        [Button]
        public void HandleButton(USkillButton button)
        {
            // Skill data
            var skill = button.CurrentSkill;
            HandleName(skill);
            HandleCooldown(skill);
            HandleDescription(skill);
        }

        private void HandleName(CombatSkill skill)
        {
            skillName.text = skill.SkillName;
        }

        private void HandleCooldown(CombatSkill skill)
        {
            int cooldownAmount = skill.CurrentCooldown;
            if (cooldownAmount > 0)
            {
                cooldown.transform.parent.gameObject.SetActive(true);
                cooldown.text = cooldownAmount.ToString();
            }
            else
            {
                if (skill.IsInCooldown())
                {
                    cooldown.text = "X";
                }
                else
                {
                    cooldown.transform.parent.gameObject.SetActive(false);
                }
            }
        }

        private void HandleDescription(CombatSkill skill)
        {
            string description;
            string cooldownCost = $"Cooldown: {skill.CooldownCost}\n";
            description = cooldownCost;

            string type = $"Type: {UtilsSkill.GetType(skill)}\n";
            description += type;

            basicDescription.text = description;
        }
    }
}
