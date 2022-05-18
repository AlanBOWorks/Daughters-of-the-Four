using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem.Player
{
    public sealed class PlayerTeamController : CombatTeamControllerBase,
        IPlayerEntityListener, ITempoTeamStatesListener,
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

        internal CombatEntity GetPerformer() => _selectedPerformer;
        internal CombatSkill GetSkill() => _selectedSkill;


        public override void InvokeStartControl()
        {
        }

        public void PerformRequestAction()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            SkillUsageValues values = new SkillUsageValues(_selectedPerformer,_selectedTarget,_selectedSkill);
            CombatSystemSingleton.EventsHolder.OnCombatSkillSubmit(in values);
            playerEvents.OnSkillSubmit(in _selectedSkill);
            playerEvents.OnTargetSubmit(in _selectedTarget);

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
            DeselectSkill(in skill);
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            DeselectSkill(in skill);
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            DeselectSkill(in skill);
        }


        private void DeselectSkill(in CombatSkill skill)
        {
            if (_selectedSkill == skill) _selectedSkill = null;
        }


        public void OnPerformerSwitch(in CombatEntity performer)
        {
            _selectedPerformer = performer;
            HandleSkillCancel();
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

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
           
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatEffectPerform(in CombatEntity performer, in CombatEntity target, in PerformEffectValues values)
        {
        }

        public void OnCombatSkillFinish(in CombatEntity performer)
        {
           
        }


        private void OnControlFinish()
        {
            _selectedTarget = null;
            _selectedPerformer = null;
            HandleSkillCancel();
        }

        private void HandleSkillCancel()
        {
            if (_selectedSkill == null) return;

            PlayerCombatSingleton.PlayerCombatEvents.OnSkillCancel(in _selectedSkill);
            _selectedSkill = null;
        }


        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            OnControlFinish();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }

        public void FinishCurrentPerformer()
        {
            if(_selectedPerformer == null) return;
            UtilsCombatEntity.DoSequenceFinish(in _selectedPerformer);
        }

    }

}
