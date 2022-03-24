using System;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UPlayerEventsDebug : MonoBehaviour, 
        ISkillPointerListener, ISkillSelectionListener
    {
        // Start is called before the first frame update
        void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        [Title("Pointer State")]
        public EnumsEvent.SkillPointerStates lastSkillPointerState;
        [ShowInInspector]
        public CombatSkill lastPointerSkill;
        [Title("Selection State")]
        public EnumsEvent.SkillSelectionStates lastSkillSelectionState;
        [ShowInInspector]
        public CombatSkill lastSelectionSkill;

        private void HandlePointerState(EnumsEvent.SkillPointerStates state, CombatSkill skill)
        {
            lastSkillPointerState = state;
            lastPointerSkill = skill;
        }

        public void OnSkillButtonHover(in CombatSkill skill)
        {
            HandlePointerState(EnumsEvent.SkillPointerStates.Hover,skill);
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            HandlePointerState(EnumsEvent.SkillPointerStates.Exit, skill);
        }



        private void HandleSelectionState(EnumsEvent.SkillSelectionStates state, CombatSkill skill)
        {
            lastSkillSelectionState = state;
            lastSelectionSkill = skill;
        }

        public void OnSkillSelect(in CombatSkill skill)
        {
            HandleSelectionState(EnumsEvent.SkillSelectionStates.FirstSelect, skill);
        }

        public void OnSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
        {
            HandleSelectionState(EnumsEvent.SkillSelectionStates.Switch,skill);
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            HandleSelectionState(EnumsEvent.SkillSelectionStates.Deselect, skill);
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            HandleSelectionState(EnumsEvent.SkillSelectionStates.Cancel, skill);
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            HandleSelectionState(EnumsEvent.SkillSelectionStates.Submit, skill);
        }
    }
}
