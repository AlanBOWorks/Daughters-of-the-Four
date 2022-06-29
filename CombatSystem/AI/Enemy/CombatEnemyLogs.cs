using CombatSystem.Entity;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.AI.Enemy
{
    public sealed class CombatEnemyLogs : IEnemyControllerListener
    {
        public bool ShowControlLogs = false;
        [ShowInInspector]
        private ControlLogs _controlLogs = new ControlLogs();

        private sealed class ControlLogs
        {
            public bool OnEntitySelect = true;
            public bool OnSkillSelect = true;
            public bool OnTargetSelect = true;
        }



        public void OnControlEntitySelect(CombatEntity selection)
        {
            if(!ShowControlLogs || !_controlLogs.OnEntitySelect) return;
            Debug.Log($"Enemy Control > Performer: {selection.CombatCharacterName}");
        }

        public void OnControlSkillSelect(in CombatSkill skill)
        {
            if(!ShowControlLogs || !_controlLogs.OnSkillSelect) return;
            Debug.Log($"Enemy Control > Skill: {skill.Preset}");
        }

        public void OnTargetSelect(in CombatEntity target)
        {
            if(!ShowControlLogs || !_controlLogs.OnTargetSelect) return;
            Debug.Log($"Enemy Control > Target: {target.CombatCharacterName}");
        }
    }
}
