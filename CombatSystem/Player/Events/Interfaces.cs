using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Player.Events
{
    internal interface ICombatPauseListener : ICombatEventListener
    {
        void OnCombatPause();
        void OnCombatResume();
    }

    internal interface ISkillButtonListener : ISkillPointerListener, ISkillSelectionListener { }

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
        /// Triggers only when the skill selected was from Null (the invert of [<seealso cref="OnSkillSwitch"/>])
        /// </summary>
        /// <param name="skill"></param>
        void OnSkillSelectFromNull(in CombatSkill skill);

        /// <summary>
        /// Is the same that <see cref="OnSkillSelect"/> but triggers when the selected is only different (and not null)
        /// </summary>
        void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection);
        /// <summary>
        /// Happens only if the same skill is deselected (things should become null);<br></br><br></br>
        /// Note: <br></br>
        /// Used [<seealso cref="OnSkillSwitch"/>] for switching without deselecting (selected skill becomes null)
        /// </summary>
        void OnSkillDeselect(in CombatSkill skill);

        /// <summary>
        /// A cancel the selection; could be because it selected the same skill, use ESC, time-out, target is invalid
        /// after submit (because it was killed), etc
        /// </summary>
        void OnSkillCancel(in CombatSkill skill);
        /// <summary>
        /// It's called at the same time with [<seealso cref="ITargetSelectionListener.OnTargetSubmit"/>]
        /// <br></br><br></br>
        /// Note:<br></br>
        /// Is not guaranteed to tick at 'Perform'
        /// [<seealso cref="ISkillUsageListener.OnCombatSkillPerform"/>].
        /// <br></br>
        /// Normally it will happens at the same time unless there's
        /// a lot of skills in the Queue to be performed, in which case the submit > perform will be out of sync
        /// because of the Wait.
        /// </summary>
        void OnSkillSubmit(in CombatSkill skill);
    }
    internal interface ITargetPointerListener : ICombatEventListener
    {
        void OnTargetButtonHover(in CombatEntity target);
        void OnTargetButtonExit(in CombatEntity target);
    }

    internal interface ITargetSelectionListener : ICombatEventListener
    {

        /// <summary>
        /// Virtually this is the same than [<see cref="OnTargetSubmit"/>]; but this is invoked first
        /// </summary>
        void OnTargetSelect(in CombatEntity target);
        /// <summary>
        /// Invoked only if the target was cancel (eg: targets was dead; target condition was false after previous execution, etc)
        /// </summary>
        void OnTargetCancel(in CombatEntity target);
        /// <summary>
        /// It's called at the same time with [<seealso cref="ISkillSelectionListener.OnSkillSubmit"/>]
        /// <br></br><br></br>
        /// </summary>
        void OnTargetSubmit(in CombatEntity target);
    }

    internal interface IPlayerEntityListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked when the main performer(focus) was switch;
        /// It's invoked once per switch 
        /// </summary>
        void OnPerformerSwitch(in CombatEntity performer);

    }

    internal interface ICameraHolderListener : ICombatEventListener
    {
        void OnSwitchMainCamera(in Camera combatCamera);
        void OnSwitchBackCamera(in Camera combatBackCamera);
    }
}
