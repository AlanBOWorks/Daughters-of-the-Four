using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem.Player
{
    public sealed class PlayerTeamController : CombatTeamControllerBase,
        IPlayerEntityListener,
        ISkillUsageListener,
        ISkillSelectionListener,
        ITargetSelectionListener
    {
        [ShowInInspector] 
        private CombatEntity _selectedPerformer;
        [ShowInInspector]
        private CombatSkill _selectedSkill;
        [ShowInInspector]
        private CombatEntity _selectedTarget;
        public void PerformRequestAction()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnSkillSubmit(in _selectedSkill);
            playerEvents.OnTargetSubmit(in _selectedTarget);

            CombatSystemSingleton.EventsHolder.OnSkillSubmit(in _selectedPerformer,in _selectedSkill,in _selectedTarget);
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
            _selectedSkill = null;
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
        }

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            _selectedPerformer = performer;
        }


        public void OnTargetSelect(in CombatEntity target)
        {
            _selectedTarget = target;
            PerformRequestAction();
        }

        public void OnTargetCancel(in CombatEntity target)
        {
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            OnSkillFinish();
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish()
        {
            _selectedTarget = null;
            _selectedSkill = null;
        }

    }

}
