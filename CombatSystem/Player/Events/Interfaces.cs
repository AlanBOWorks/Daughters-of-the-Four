using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Player.Events
{

    interface ISkillButtonListener : ISkillPointerListener, ISkillSelectionListener { }

    internal interface ISkillPointerListener : ICombatEventListener
    {
        void OnSkillButtonHover(in CombatSkill skill);
        void OnSkillButtonExit(in CombatSkill skill);
    }

    internal interface ISkillSelectionListener : ICombatEventListener
    {
        /// <summary>
        /// Triggers always even if the selected is the same
        /// </summary>
        void OnSkillSelect(in CombatSkill skill);
        /// <summary>
        /// Is the same that <see cref="OnSkillSelect"/> but triggers when the selected is only different (and not null)
        /// </summary>
        void OnSkillSwitch(in CombatSkill skill);
        /// <summary>
        /// A cancel the selection; could be because it selected the same skill, use ESC, time-out, etc
        /// </summary>
        void OnSkillCancel(in CombatSkill skill);
        void OnSkillSubmit(in CombatSkill skill);
    }
    internal interface ITargetPointerListener : ICombatEventListener
    {
        void OnTargetButtonHover(in CombatEntity target);
        void OnTargetButtonExit(in CombatEntity target);
    }

    internal interface ITargetSelectionListener : ICombatEventListener
    {
        void OnTargetSelect(in CombatEntity target);
        void OnTargetCancel(in CombatEntity target);
        void OnTargetSubmit(in CombatEntity target);
    }
}
