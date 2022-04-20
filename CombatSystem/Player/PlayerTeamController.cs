using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerTeamController : CombatTeamControllerBase,
        ISkillUsageListener,
        ISkillSelectionListener,
        ITargetSelectionListener
    {
        [ShowInInspector]
        private CombatSkill _selectedSkill;
        [ShowInInspector]
        private CombatEntity _skillOnTarget;
        public void PerformRequestAction(CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target)
        {
            usedSkill = _selectedSkill;
            target = _skillOnTarget;

            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnSkillSubmit(in usedSkill);
            playerEvents.OnTargetSubmit(in target);
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

        public void OnTargetSelect(in CombatEntity target)
        {
            _skillOnTarget = target;
        }

        public void OnTargetCancel(in CombatEntity target)
        {
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            OnSkillFinish();
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish()
        {
            _skillOnTarget = null;
            _selectedSkill = null;
        }

    }

}
