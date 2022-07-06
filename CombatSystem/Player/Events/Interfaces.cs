using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
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
        void OnSkillButtonHover(ICombatSkill skill);
        void OnSkillButtonExit(ICombatSkill skill);
    }

    internal interface ISkillSelectionListener : ICombatEventListener
    {
        /// <summary>
        /// Triggers always even if the selected is the same
        /// </summary>
        void OnSkillSelect(CombatSkill skill);

        /// <summary>
        /// Triggers only when the skill selected was from Null (the invert of [<seealso cref="OnSkillSwitch"/>])
        /// </summary>
        /// <param name="skill"></param>
        void OnSkillSelectFromNull(CombatSkill skill);

        /// <summary>
        /// Is the same that <see cref="OnSkillSelect"/> but triggers when the selected is only different (and not null)
        /// </summary>
        void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection);
        /// <summary>
        /// Happens only if the same skill is deselected (things should become null);<br></br><br></br>
        /// Note: <br></br>
        /// Used [<seealso cref="OnSkillSwitch"/>] for switching without deselecting (selected skill becomes null)
        /// </summary>
        void OnSkillDeselect(CombatSkill skill);

        /// <summary>
        /// A cancel the selection; could be because it selected the same skill, use ESC, time-out, target is invalid
        /// after submit (because it was killed), etc
        /// </summary>
        void OnSkillCancel(CombatSkill skill);
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
        void OnSkillSubmit(CombatSkill skill);
    }
    internal interface ISkillTooltipListener : ICombatEventListener
    {
        void OnTooltipEffect(in PerformEffectValues values);
        void OnToolTipOffensiveEffect(in PerformEffectValues values);
        void OnTooltipSupportEffect(in PerformEffectValues values);
        void OnTooltipTeamEffect(in PerformEffectValues values);
        void OnFinishPoolEffects();
    }

    internal interface ITargetPointerListener : ICombatEventListener
    {
        void OnTargetButtonHover(CombatEntity target);
        void OnTargetButtonExit(CombatEntity target);
    }

    internal interface ITargetSelectionListener : ICombatEventListener
    {

        /// <summary>
        /// Virtually this is the same than [<see cref="OnTargetSubmit"/>]; but this is invoked first
        /// </summary>
        void OnTargetSelect(CombatEntity target);
        /// <summary>
        /// Invoked only if the target was cancel (eg: targets was dead; target condition was false after previous execution, etc)
        /// </summary>
        void OnTargetCancel(CombatEntity target);
        /// <summary>
        /// It's called at the same time with [<seealso cref="ISkillSelectionListener.OnSkillSubmit"/>]
        /// <br></br><br></br>
        /// </summary>
        void OnTargetSubmit(CombatEntity target);
    }

    internal interface IPlayerCombatEventListener : ICombatEventListener
    {
        /// <summary>
        /// Invoked when the main performer(focus) was switch;
        /// It's invoked once per switch 
        /// </summary>
        void OnPerformerSwitch(CombatEntity performer);

       /// <summary>
       /// Used on switching the UI skills;<br></br>
       /// For true stance switching use [<seealso cref="ITeamEventListener.OnStanceChange"/>]
       /// </summary>
        void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance);

    }

    internal interface ICameraHolderListener : ICombatEventListener
    {
        void OnSwitchMainCamera(in Camera combatCamera);
        void OnSwitchBackCamera(in Camera combatBackCamera);
        void OnSwitchFrontCamera(in Camera combatFrontCamera);
    }

}
