using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerTeamController : CombatTeamControllerBase,
        IOverridePauseElement,
        IPlayerCombatEventListener, 

        ITempoControlStatesExtraListener,

        ISkillUsageListener, ISkillSelectionListener,
        ITargetSelectionListener
    {
        [ShowInInspector] 
        private CombatEntity _selectedPerformer;
        [ShowInInspector]
        private IFullSkill _selectedSkill;
        [ShowInInspector]
        private CombatEntity _selectedTarget;


        internal CombatEntity GetPerformer() => _selectedPerformer;
        internal IFullSkill GetSkill() => _selectedSkill;


        public void OnPauseInputReturnState(IOverridePauseElement lastElement)
        {
            HandleSkillCancel(ref _selectedSkill);
        }


        public override void InvokeStartControl()
        {
            _selectedSkill = null;
            _selectedTarget = null;
        }

        private void PerformRequestAction()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            SkillUsageValues values = new SkillUsageValues(_selectedPerformer,_selectedTarget,_selectedSkill);
            CombatSystemSingleton.EventsHolder.OnCombatSkillSubmit(in values);
            playerEvents.OnSkillSubmit(_selectedSkill);
            playerEvents.OnTargetSubmit(_selectedTarget);

        }

        public void OnSkillSelect(IFullSkill skill)
        {
        }

        public void OnSkillSelectFromNull(IFullSkill skill)
        {
            PlayerCombatSingleton.GetCombatEscapeButtonHandler().PushOverridingAction(this);
        }

        public void OnSkillSwitch(IFullSkill skill, IFullSkill previousSelection)
        {
            _selectedSkill = skill;

        }

        public void OnSkillDeselect(IFullSkill skill)
        {
        }

        public void OnSkillCancel(CombatSkill skill)
        {
        }

        public void OnSkillSubmit(IFullSkill skill)
        {
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            _selectedPerformer = performer;
            HandleSkillCancel(ref _selectedSkill);
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            HandleSkillCancel(ref _selectedSkill);
        }

        public void OnTargetSelect(CombatEntity target)
        {
            _selectedTarget = target;
            PerformRequestAction();
        }

        public void OnTargetCancel(CombatEntity target)
        {
        }

        public void OnTargetSubmit(CombatEntity target)
        {

        }

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
           
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
           
        }

        private void OnControlFinish()
        {
            _selectedTarget = null;
            _selectedPerformer = null;
            _selectedSkill = null;
        }


        private static void HandleSkillCancel(ref IFullSkill skill)
        {
            if (skill == null) return;
            PlayerCombatSingleton.PlayerCombatEvents.OnSkillDeselect(skill);
            skill = null;
        }


        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
        }

        public void LateOnAllActorsNoActions(CombatEntity lastActor)
        {
            
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
            OnControlFinish();
        }

        public void FinishCurrentPerformer()
        {
            if(_selectedPerformer == null) return;
            UtilsCombatEntity.DoSequenceFinish(_selectedPerformer);
        }

    }

}
