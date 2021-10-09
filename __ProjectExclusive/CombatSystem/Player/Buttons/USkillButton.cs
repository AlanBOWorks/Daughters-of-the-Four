using CombatSkills;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class USkillButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillName;
        private CombatingSkill _currentSkill;

        public void ForceUpdate()
        {
            skillName.text = _currentSkill.GetSkillName();
        }

        public void Injection(CombatingSkill skill)
        {
            _currentSkill = skill;
            ForceUpdate();
        }

        public void ResetState()
        {
            _currentSkill = null;
        }
    }
}
