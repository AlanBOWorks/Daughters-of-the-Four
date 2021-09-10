using Skills;
using UnityEngine;

namespace _Player
{
    public class PlayerSkillsTracker : IPlayerSkillListener
    {
        public CombatSkill CurrentSelectedSkill { get; private set; }

        public void OnSkillSelect(CombatSkill selectedSkill)
        {
            CurrentSelectedSkill = selectedSkill;
        }

        public void OnSkillDeselect(CombatSkill deselectSkill)
        {
            CurrentSelectedSkill = null;
        }

        public void OnSubmitSkill(CombatSkill submitSkill)
        {
        }
    }
}
