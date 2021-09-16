using System;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace _Player
{
    public class CombatSkillSelector : ITempoListener
    {
        public CombatSkill SelectedSkill { get; private set; }
        private CombatingEntity _currentEntity;
        


        public void OnSkillSelect(CombatSkill selectedSkill)
        {
            if(selectedSkill.IsInCooldown()) return;

            if (selectedSkill != SelectedSkill && SelectedSkill != null)
            {
                SelectedSkill.SwitchState(CombatSkill.State.Idle);
                ToggleIcon(false);
                PlayerEntitySingleton.PlayerCombatEvents.OnSkillDeselect(SelectedSkill);
            }


            SelectedSkill = selectedSkill;
            ToggleIcon(true);

            PlayerEntitySingleton.PlayerCombatEvents.OnSkillSelect(SelectedSkill);

        }

        private void ToggleIcon(bool showSelected)
        {
            var button = PlayerEntitySingleton.SkillButtonsDictionary[SelectedSkill];
            button.ToggleSelectedIcon(showSelected);
        }

        public void SubmitTarget(CombatingEntity target)
        {
            if (SelectedSkill == null || _currentEntity == null || target == null)
            {
                string nullLog = "_____\nNULL:\n";
                if (SelectedSkill == null)
                    nullLog += "Selected Skill\n";
                if (_currentEntity == null)
                    nullLog += "User Entity\n";
                if (target == null)
                    nullLog += "Target entity\n";



                throw new MethodAccessException($"Submitting a Target[{target.CharacterName}] while Skill parameters",
                    new NullReferenceException(nullLog));
            }

            ToggleIcon(false);

            SelectedSkill.OnSkillUsage();
            PlayerEntitySingleton.PlayerCombatEvents.OnSubmitSkill(SelectedSkill);
            CombatSystemSingleton.PerformSkillHandler.InjectSkill(SelectedSkill, _currentEntity, target);
        }


        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _currentEntity = entity;
            SelectedSkill = null;
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            SelectedSkill = null;
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            _currentEntity = null;
        }
    }
}
