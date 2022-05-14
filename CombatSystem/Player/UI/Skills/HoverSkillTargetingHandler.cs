using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class HoverSkillTargetingHandler : SkillTargetingHandler, 
        IPlayerEntityListener,
        ITargetPointerListener, ISkillSelectionListener
    {
        private CombatEntity _performer;
        private CombatSkill _selectedSkill;

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            _performer = performer;
        }

        public void OnTargetButtonHover(in CombatEntity target)
        {
            HandleSkill(in _performer, in _selectedSkill, in target);
        }

        public void OnTargetButtonExit(in CombatEntity target)
        {
        }



        public void OnSkillSelect(in CombatSkill skill)
        {
        }

        public void OnSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
        {
            _selectedSkill = skill;
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
        }
    }
}
