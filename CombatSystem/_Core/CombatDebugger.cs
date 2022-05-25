using System.Collections.Generic;
using CombatSystem.AI.Enemy;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem._Core
{

#if UNITY_EDITOR
    internal sealed class CombatDebuggerSingleton
    {
        public static readonly CombatDebuggerSingleton Instance = new CombatDebuggerSingleton();

        static CombatDebuggerSingleton()
        {
            CombatEventsLogs = new CombatEventsLogs();
            CombatPlayerEventsLogs = new CombatPlayerEventsLogs();
            CombatEnemyLogs = new CombatEnemyLogs();
        }

        public static readonly CombatEventsLogs CombatEventsLogs;
        public static readonly CombatPlayerEventsLogs CombatPlayerEventsLogs;
        public static readonly CombatEnemyLogs CombatEnemyLogs;


        private sealed class CombatDebuggerWindow : OdinEditorWindow
        {
            [ShowInInspector, TabGroup("Core Events")] 
            private CombatEventsLogs _eventsLogs;

            [ShowInInspector, TabGroup("Player Events")]
            private CombatPlayerEventsLogs _playerLogs;

            [ShowInInspector, TabGroup("Enemy Events")]
            private CombatEnemyLogs _enemyLogs;


            [Button]
            private static void Refresh()
            {
                var window = GetWindow<CombatDebuggerWindow>();
                window._eventsLogs = CombatEventsLogs;
                window._playerLogs = CombatPlayerEventsLogs;
                window._enemyLogs = CombatEnemyLogs;
            }


            [MenuItem("Combat/Debug/[DEBUGGER]", priority = -100)]
            private static void OpenWindow()
            {
                Refresh();
            }

        }
    }


    internal sealed class CombatEventsLogs : ITempoTeamStatesListener,
        ITempoDedicatedEntityStatesListener, ITempoEntityStatesListener,
        ITempoTickListener, ISkillUsageListener,
        ITempoEntityStatesExtraListener,
        IDamageDoneListener
    {
        [Title("Teams")] 
        public bool ShowTeamLogs = false;

        private sealed class TeamLogs
        {
            public bool OnStartControl = true;
            public bool OnFinishActors = true;
            public bool OnFinishControl = true;
        }
        [ShowInInspector]
        private TeamLogs _teamLogs = new TeamLogs();

        public void OnTempoPreStartControl(in CombatTeamControllerBase controller)
        {
            if(!ShowTeamLogs || !_teamLogs.OnStartControl) return;
            Debug.Log($"Start Control: {controller} | Controlling {controller.ControllingTeam.GetControllingMembers().Count}");
        }

        public void OnAllActorsNoActions(in CombatEntity lastActor)
        {
            if (!ShowTeamLogs || !_teamLogs.OnFinishActors) return;
            Debug.Log($"Pre-Finish all Actors: {lastActor.CombatCharacterName}");
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            if (!ShowTeamLogs || !_teamLogs.OnFinishActors) return;
            Debug.Log($"Finish all Actors: {lastActor.CombatCharacterName}");

        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            if(!ShowTeamLogs || !_teamLogs.OnFinishControl) return;
            Debug.Log($"Finish Control: {controller}");
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            if(!ShowTeamLogs || !_teamLogs.OnFinishControl) return;
            Debug.Log($"------------------------XXX LAST CONTROL XXX-------------------------------");

        }


        [Title("Entity Sequence")]
        public bool ShowEntitySequenceLogs = false;
        private class EntitySequenceLogs
        {
            public bool OnSequence = true;
            public bool OnAction = true;
            public bool OnFinishAction = true;
            public bool OnFinishSequence = true;
        }

        [ShowInInspector]
        private EntitySequenceLogs _entitySequenceLogs = new EntitySequenceLogs();

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnSequence) return;
            Debug.Log($"> --- TRINITY Entity [START] Sequence: {entity.GetProviderEntityName()}");
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnSequence) return;
            Debug.Log($"> --- OFF Entity [START] Sequence: {entity.GetProviderEntityName()}");
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnFinishSequence) return;
            Debug.Log($"> --- TRINITY Entity [END] Sequence: {entity.GetProviderEntityName()}");
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnFinishSequence) return;
            Debug.Log($"> --- OFF Entity [END] Sequence: {entity.GetProviderEntityName()}");
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnSequence) return;
            Debug.Log("--------- -------- -------- SEQUENCE -------- -------- --------");
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnAction) return;
            Debug.Log($"> --- Entity [START] Action: {entity.GetProviderEntityName()}");
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnFinishAction) return;
            Debug.Log($"> --- Entity [END] Action: {entity.GetProviderEntityName()}");
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnFinishSequence) return;
            Debug.Log($"Sequence Finished | Forced: {isForcedByController}");
        }

        [Title("Entity Extra")]
        public bool ShoEntitySequenceExtraLogs = false;
        private sealed class EntitySequenceExtraLogs
        {
            public bool OnRequest = true;
            public bool OnFinish = true;
            public bool OnNoActions = true;
        }
        [ShowInInspector]
        private EntitySequenceExtraLogs _entitySequenceExtraLogs = new EntitySequenceExtraLogs();

        public void OnAfterEntityRequestSequence(in CombatEntity entity)
        {
            if (!ShoEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnRequest) return;
            Debug.Log("After Request Sequence");
        }

        public void OnAfterEntitySequenceFinish(in CombatEntity entity)
        {
            if (!ShoEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnFinish) return;
            Debug.Log("--------- -------- END SEQUENCE -------- -------- --------");
        }

        public void OnNoActionsForcedFinish(in CombatEntity entity)
        {
            if (!ShoEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnNoActions) return;
            Debug.Log($"Request Sequence - NO ACTIONS: {entity.CombatCharacterName}");
        }



        [Title("Tempo")]
        public bool ShowTempoLogs = false;
        private class TempoLogs
        {
            public bool OnStartTicking = false;
            public bool OnTick = false;
            public bool OnRoundPassed = true;
            public bool OnStopTick = true;
        }

        [ShowInInspector]
        private TempoLogs _tempoLogs = new TempoLogs();

        public void OnStartTicking()
        {
            if (!ShowTempoLogs || !_tempoLogs.OnStartTicking) return;
            Debug.Log("xx - START Ticking");
        }

        public void OnTick()
        {
            if (!ShowTempoLogs || !_tempoLogs.OnTick) return;
            Debug.Log("TTTTT - Tick - TTTTT");
        }

        public void OnRoundPassed()
        {
            if (!ShowTempoLogs || !_tempoLogs.OnRoundPassed) return;
            Debug.Log("xx - ROUND Passed");
        }

        public void OnStopTicking()
        {
            if (!ShowTempoLogs || !_tempoLogs.OnStopTick) return;
            Debug.Log("xx - STOP Ticking");
        }



        [Title("Skills")]
        public bool ShowSkillLogs = false;
        private class SkillsSubmitLogs
        {
            public bool OnSubmit = true;
            public bool OnPerform = true;
            public bool OnEffect = true;
            public bool OnFinish = true;
        }

        [ShowInInspector]
        private SkillsSubmitLogs _skillsLogs = new SkillsSubmitLogs();

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            if (!ShowSkillLogs || !_skillsLogs.OnSubmit) return;
            values.Extract(out var performer, out var target, out var usedSkill);
            Debug.Log($"----------------- SKILL --------------- \n" +
                      $"Random Controller: {performer.GetProviderEntityName()} / " +
                      $"Used : {usedSkill.Preset} /" +
                      $"Target: {target.GetProviderEntityName()}");
            Debug.Log($"Performer ACTIONS: {performer.Stats.UsedActions}");
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            if (!ShowSkillLogs || !_skillsLogs.OnPerform) return;
            Debug.Log($"Performing ----- > {values.UsedSkill.GetSkillName()}");
        }

        public void OnCombatEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            if (!ShowSkillLogs || !_skillsLogs.OnEffect) return;
            Debug.Log($"Effect[{values.Effect} - ] performed  {performer.GetProviderEntityName()} / On target: {target.GetProviderEntityName()} ");
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            if (!ShowSkillLogs || !_skillsLogs.OnFinish) return;
            Debug.Log($"Performer ACTIONS: {performer.Stats.UsedActions}");
            Debug.Log("-------------- SKILL END --------------- ");
        }

        [Title("Damage")] 
        public bool ShowDamageLogs = false;
        private class DamageLogs
        {
            public bool OnShieldLost = true;
            public bool OnHealthLost = true;
            public bool OnMortalityLost = true;
            public bool OnKnockOut = true;
        }
        [ShowInInspector]
        private DamageLogs _damageLogs = new DamageLogs();

        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            if(!ShowDamageLogs || !_damageLogs.OnShieldLost) return;
            Debug.Log($"SHIELDS - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            if (!ShowDamageLogs || !_damageLogs.OnHealthLost) return;
            Debug.Log($"HEALTH - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");

        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            if (!ShowDamageLogs || !_damageLogs.OnMortalityLost) return;
            Debug.Log($"MORTALITY - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");

        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            if (!ShowDamageLogs || !_damageLogs.OnKnockOut) return;
            Debug.Log($"KNOCKOUT - P[{performer.CombatCharacterName}] > T[{target}");

        }
    }



    public sealed class CombatPlayerEventsLogs :
        ICombatPauseListener,
        ISkillPointerListener, ISkillSelectionListener,
        ITargetPointerListener, ITargetSelectionListener, IHoverInteractionTargetsListener,
        IPlayerEntityListener
    {

        public bool ShowPauseLogs = false;
        private sealed class PauseLogs
        {
            public bool OnPause = true;
            public bool OnResume = true;
        }
        [ShowInInspector]
        private PauseLogs _pauseLogs = new PauseLogs();

        public void OnCombatPause()
        {
            if(!ShowPauseLogs || !_pauseLogs.OnPause) return;
            Debug.Log("XxXxX - PAUSE Combat - XxXxX");
        }

        public void OnCombatResume()
        {
            if (!ShowPauseLogs || !_pauseLogs.OnResume) return;
            Debug.Log("XxXxX - RESUME Combat - XxXxX");
        }

        [Title("Skill Buttons")]
        public bool ShowSkillButtonLogs = false;
        private sealed class SkillPointerLogs
        {
            public bool OnButtonHover = true;
            public bool OnButtonExit = true;
        }
        [ShowInInspector]
        private SkillPointerLogs _skillPointerLogs = new SkillPointerLogs();
        
        public void OnSkillButtonHover(in CombatSkill skill)
        {
            if(!ShowSkillButtonLogs || !_skillPointerLogs.OnButtonHover) return;
            Debug.Log($"Skill-Button Hover: {skill.Preset}");
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            if(!ShowSkillButtonLogs || !_skillPointerLogs.OnButtonExit) return;
            Debug.Log($"Skill-Button Exit: {skill.Preset}");
        }

        [Title("Skill Selections")]
        public bool ShowSkillSelectionLogs = false;
        private sealed class SkillSelectionLogs
        {
            public bool OnSelect = true;
            public bool OnSelectFromNull = true;
            public bool OnSwitch = true;
            public bool OnDeselect = true;
            public bool OnCancel = true;
            public bool OnSubmit = true;
        }
        [ShowInInspector]
        private SkillSelectionLogs _skillSelectionLogs = new SkillSelectionLogs();

        public void OnSkillSelect(in CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSelect) return;
            Debug.Log($"Skill Select: {skill.Preset}");
        }

        public void OnSkillSelectFromNull(in CombatSkill skill)
        {
            if (!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSelectFromNull) return;
            Debug.Log($"Skill Select (From NULL): {skill.Preset}");
        }

        public void OnSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSwitch) return;
            Debug.Log($"Skill SWITCH: {skill.Preset} FROM {previousSelection.Preset}");
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnDeselect) return;
            Debug.Log($"Skill DESELECTED: {skill.Preset}");
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnCancel) return;
            Debug.Log($"Skill CANCEL: {skill.Preset}");
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSubmit) return;
            Debug.Log($"xxxx - Skill Submit: {skill.Preset}");
        }

        [Title("Target Buttons")]
        public bool ShowTargetPointerLogs = false;
        private sealed class TargetPointerLogs
        {
            public bool OnButtonHover = true;
            public bool OnButtonExit = true;
        }
        [ShowInInspector]
        private TargetPointerLogs _targetPointerLogs = new TargetPointerLogs();
        public void OnTargetButtonHover(in CombatEntity target)
        {
            if(!ShowTargetPointerLogs || !_targetPointerLogs.OnButtonHover) return;
            Debug.Log($"Target Hover: {target.GetProviderEntityName()}");
        }

        public void OnTargetButtonExit(in CombatEntity target)
        {
            if(!ShowTargetPointerLogs || !_targetPointerLogs.OnButtonExit) return;
            Debug.Log($"Target Exit: {target.GetProviderEntityName()}");
        }

        [Title("Target Selections")]
        public bool ShowTargetSelectionLogs = false;
        private sealed class TargetSelectionLogs
        {
            public bool OnSelect = true;
            public bool OnCancel = true;
            public bool OnSubmit = true;

        }
        [ShowInInspector]
        private TargetSelectionLogs _targetSelectionLogs = new TargetSelectionLogs();
        public void OnTargetSelect(in CombatEntity target)
        {
            if(!ShowTargetSelectionLogs || !_targetSelectionLogs.OnSelect) return;
            Debug.Log($"Target Select: {target.GetProviderEntityName()}");
        }

        public void OnTargetCancel(in CombatEntity target)
        {
            if(!ShowTargetSelectionLogs || !_targetSelectionLogs.OnCancel) return;
            Debug.Log($"Target Cancel: {target.GetProviderEntityName()}");
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
            if(!ShowTargetSelectionLogs || !_targetSelectionLogs.OnSubmit) return;
            Debug.Log($"xxxx - Target SUBMIT: {target.GetProviderEntityName()}");
        }



        [Title("Target Hover")]
        public bool ShowTargetFeedbackLogs = false;
        private sealed class TargetFeedbackLogs
        {
            public bool OnPossibleTargets = true;
            public bool OnHideTargets = true;
        }

        [ShowInInspector]
        private TargetFeedbackLogs _targetFeedbackLogs = new TargetFeedbackLogs();

        public void OnHoverTargetInteraction(in CombatEntity target)
        {
            if (!ShowTargetFeedbackLogs || !_targetFeedbackLogs.OnPossibleTargets) return;
            Debug.Log($"Possible Target: {target.CombatCharacterName}");
           
        }

        public void OnHoverTargetExit()
        {
            if (!ShowTargetFeedbackLogs || !_targetFeedbackLogs.OnHideTargets) return;
            Debug.Log($"Hiding Possible Targets");
        }


        [Title("Performer Switch")]
        public bool ShowEntitiesLog = false;

        private sealed class EntitiesLogs
        {
            public bool OnPerformerSwitch = true;
        }
        [ShowInInspector]
        private EntitiesLogs _entitiesLogs = new EntitiesLogs();

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            if(!ShowEntitiesLog || !_entitiesLogs.OnPerformerSwitch) return;
            Debug.Log($"xxxx - PERFORMER: {performer.GetProviderEntityName()}");
        }

    }

#endif


}
