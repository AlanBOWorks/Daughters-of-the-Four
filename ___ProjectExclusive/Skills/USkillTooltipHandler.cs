using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Skills
{
    public class USkillTooltipHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillName = null;
        [SerializeField] private TextMeshProUGUI basicDescription = null;

        [Button]
        public void HandleButton(USkillButton button)
        {
            transform.position = button.transform.position;

            // Skill data
            skillName.text = button.CurrentSkill.SkillName;
        }
    }
}
