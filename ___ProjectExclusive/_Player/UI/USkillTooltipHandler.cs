using Sirenix.OdinInspector;
using Skills;
using TMPro;
using UnityEngine;

namespace _Player
{
    public class USkillTooltipHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillName = null;
        [SerializeField] private TextMeshProUGUI basicDescription = null;

        [Button]
        public void HandleButton(USkillButton button)
        {
            // Skill data
            skillName.text = button.CurrentSkill.SkillName;
        }
    }
}
