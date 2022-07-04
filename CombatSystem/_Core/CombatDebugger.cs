using System.Collections.Generic;
using CombatSystem.AI.Enemy;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using CombatSystem.Skills.VanguardEffects;
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


    internal sealed class CombatEventsLogs : 
        ICombatEventsHolderBase,
        ITempoTickListener, IDamageDoneListener
    {
        [TitleGroup("Team")] 
        public bool ShowTeamLogs = false;

        private sealed class TeamLogs
        {
            public bool OnStartControl = true;
            public bool OnFinishActors = true;
            public bool OnFinishControl = true;
        }
        [TitleGroup("Team")] 
        [ShowInInspector]
        private TeamLogs _teamLogs = new TeamLogs();

        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            if (!ShowTeamLogs || !_teamLogs.OnStartControl) return;
            Debug.Log($"Start Control: {controller} | Controlling {controller.ControllingTeam.GetControllingMembers().Count}");

        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            if (!ShowTeamLogs || !_teamLogs.OnFinishActors) return;
            Debug.Log($"Pre-Finish all Actors: {lastActor.CombatCharacterName}");
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
            if (!ShowTeamLogs || !_teamLogs.OnFinishActors) return;
            Debug.Log($"Finish all Actors: {lastActor.CombatCharacterName}");

        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            if(!ShowTeamLogs || !_teamLogs.OnFinishControl) return;
            Debug.Log($"Finish Control: {controller}");
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
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
            public bool OnNoActionLeft = true;
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
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnNoActionLeft) return;
            Debug.Log($"> --- Entity 0 Action: {entity.GetProviderEntityName()}");

        }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
            if (!ShowEntitySequenceLogs || !_entitySequenceLogs.OnFinishSequence) return;
            Debug.Log($"Sequence Finished | Forced: {isForcedByController}");
        }

        [Title("Entity Extra")]
        public bool ShowEntitySequenceExtraLogs = false;
        private sealed class EntitySequenceExtraLogs
        {
            public bool OnRequest = true;
            public bool OnFinish = true;
            public bool OnNoActions = true;
        }
        [ShowInInspector]
        private EntitySequenceExtraLogs _entitySequenceExtraLogs = new EntitySequenceExtraLogs();

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            if (!ShowEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnRequest) return;
            Debug.Log("After Request Sequence");
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            if (!ShowEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnFinish) return;
            Debug.Log("--------- -------- END SEQUENCE -------- -------- --------");
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            if (!ShowEntitySequenceExtraLogs || !_entitySequenceExtraLogs.OnNoActions) return;
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



        [TitleGroup("Skills")]
        public bool ShowSkillLogs = false;
        private class SkillsSubmitLogs
        {
            public bool OnSubmit = true;
            public bool OnPerform = true;
            public bool OnFinish = true;
        }

        [TitleGroup("Skills")]
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

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            if (!ShowSkillLogs || !_skillsLogs.OnFinish) return;
            Debug.Log($"Performer ACTIONS: {performer.Stats.UsedActions}");
            Debug.Log("-------------- SKILL END --------------- ");
        }

        [TitleGroup("Skills")]
        public bool ShowEffectLogs = false;
        private class EffectsSubmitLogs
        {
            public bool OnPrimary = true;
            public bool OnSecondary = true;
            public bool OnVanguardEffects = true;
        }
        [TitleGroup("Skills")]
        [ShowInInspector]
        private EffectsSubmitLogs _effectLogs = new EffectsSubmitLogs();

        public void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            if (!ShowEffectLogs || !_effectLogs.OnPrimary) return;
            entities.Extract(out var performer, out var target);
            Debug.Log($"Primary Effect[{values.Effect} - ] performed  {performer.GetProviderEntityName()} / On target: {target.GetProviderEntityName()} ");
        }

        public void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            if (!ShowEffectLogs || !_effectLogs.OnSecondary) return;
            entities.Extract(out var performer, out var target);
            Debug.Log($"Secondary Effect[{values.Effect} - ] performed  {performer.GetProviderEntityName()} / On target: {target.GetProviderEntityName()} ");
        }

        public void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            if (!ShowEffectLogs || !_effectLogs.OnVanguardEffects) return;
            entities.Extract(out var performer, out var target);
            Debug.Log($"Secondary Effect[{values.Effect} - ] performed  {performer.GetProviderEntityName()} / On target: {target.GetProviderEntityName()} ");
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

        public void OnShieldLost(CombatEntity performer, CombatEntity target, float amount)
        {
            if(!ShowDamageLogs || !_damageLogs.OnShieldLost) return;
            Debug.Log($"SHIELDS - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");
        }

        public void OnHealthLost(CombatEntity performer, CombatEntity target, float amount)
        {
            if (!ShowDamageLogs || !_damageLogs.OnHealthLost) return;
            Debug.Log($"HEALTH - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");

        }

        public void OnMortalityLost(CombatEntity performer, CombatEntity target, float amount)
        {
            if (!ShowDamageLogs || !_damageLogs.OnMortalityLost) return;
            Debug.Log($"MORTALITY - P[{performer.CombatCharacterName}] > T[{target}] : {amount}");

        }

        public void OnDamageReceive(CombatEntity performer, CombatEntity target)
        {
        }

        public void OnKnockOut(CombatEntity performer, CombatEntity target)
        {
            if (!ShowDamageLogs || !_damageLogs.OnKnockOut) return;
            Debug.Log($"KNOCKOUT - P[{performer.CombatCharacterName}] > T[{target}");

        }


        [Title("Vanguard Effects")] 
        public bool ShowVanguardEffectsLogs = false;
        private class VanguardEffectsLogs
        {
            public bool OnEffectSubscribe = true;
            public bool OnEffectIncrement = true;
            public bool OnRevengePerform = true;
            public bool OnPunishPerform = true;
        }
        [ShowInInspector]
        private VanguardEffectsLogs _vanguardEffectsLogs = new VanguardEffectsLogs();


        public void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values)
        {
            if(!ShowVanguardEffectsLogs || !_vanguardEffectsLogs.OnEffectSubscribe) return;
            Debug.Log($"Vanguard Subscribe [{values.Type}] >>>> {values.Skill}");

        }

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
            if(!ShowVanguardEffectsLogs || !_vanguardEffectsLogs.OnEffectIncrement) return;
            Debug.Log($"Vanguard Increment [{type}] <<<< {attacker.CombatCharacterShorterName}");
        }

        public void OnVanguardEffectPerform(VanguardSkillUsageValues values)
        {
            if(!ShowVanguardEffectsLogs || !_vanguardEffectsLogs.OnRevengePerform) return;
            Debug.Log($"REVENGE Perform: {values.UsedSkill} [{values.Accumulation}]");
        }

        public void OnVanguardPunishEffectPerform(IVanguardSkill skill, int iterations)
        {
            if (!ShowVanguardEffectsLogs || !_vanguardEffectsLogs.OnPunishPerform) return;
            Debug.Log($"PUNISH Perform: {skill} [{iterations}]");
        }

        [TitleGroup("Team")] 
        public bool ShowTeamValuesChangeLogs = false;
        private class TeamValuesChangeLogs
        {
            public bool OnStanceChange = true;
            public bool OnControlChange = true;
        }
        [TitleGroup("Team")] 
        [ShowInInspector]
        private TeamValuesChangeLogs _teamValuesChangeLogs = new TeamValuesChangeLogs();

        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
            if(!ShowTeamValuesChangeLogs || !_teamValuesChangeLogs.OnStanceChange) return;
            string teamName = team.IsPlayerTeam ? "PLAYER" : "ENEMY";
            Debug.Log($"On Stance Change: {teamName} >> {switchedStance}");
        }

        public void OnControlChange(CombatTeam team, float phasedControl, bool isBurst)
        {
            if (!ShowTeamValuesChangeLogs || !_teamValuesChangeLogs.OnControlChange) return;
            string teamName = team.IsPlayerTeam ? "PLAYER" : "ENEMY";
            Debug.Log($"On Control Change: {teamName} >> {phasedControl} [Burst: {isBurst}]");

        }
    }



    public sealed class CombatPlayerEventsLogs :
        ICombatPauseListener,
        ISkillPointerListener, ISkillSelectionListener,
        ITargetPointerListener, ITargetSelectionListener, IHoverInteractionEffectTargetsListener,
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
        
        public void OnSkillButtonHover(ICombatSkill skill)
        {
            if(!ShowSkillButtonLogs || !_skillPointerLogs.OnButtonHover) return;
            Debug.Log($"Skill-Button Hover: {skill.Preset}");
        }

        public void OnSkillButtonExit(ICombatSkill skill)
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

        public void OnSkillSelect(CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSelect) return;
            Debug.Log($"Skill Select: {skill.Preset}");
        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
            if (!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSelectFromNull) return;
            Debug.Log($"Skill Select (From NULL): {skill.Preset}");
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnSwitch) return;
            Debug.Log($"Skill SWITCH: {skill.Preset} FROM {previousSelection.Preset}");
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnDeselect) return;
            Debug.Log($"Skill DESELECTED: {skill.Preset}");
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            if(!ShowSkillSelectionLogs || !_skillSelectionLogs.OnCancel) return;
            Debug.Log($"Skill CANCEL: {skill.Preset}");
        }

        public void OnSkillSubmit(CombatSkill skill)
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
        public void OnTargetButtonHover(CombatEntity target)
        {
            if(!ShowTargetPointerLogs || !_targetPointerLogs.OnButtonHover) return;
            Debug.Log($"Target Hover: {target.GetProviderEntityName()}");
        }

        public void OnTargetButtonExit(CombatEntity target)
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
        public void OnTargetSelect(CombatEntity target)
        {
            if(!ShowTargetSelectionLogs || !_targetSelectionLogs.OnSelect) return;
            Debug.Log($"Target Select: {target.GetProviderEntityName()}");
        }

        public void OnTargetCancel(CombatEntity target)
        {
            if(!ShowTargetSelectionLogs || !_targetSelectionLogs.OnCancel) return;
            Debug.Log($"Target Cancel: {target.GetProviderEntityName()}");
        }

        public void OnTargetSubmit(CombatEntity target)
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

        public void OnHoverTargetInteraction(CombatEntity target, ISkill skill)
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

        public void OnPerformerSwitch(CombatEntity performer)
        {
            if(!ShowEntitiesLog || !_entitiesLogs.OnPerformerSwitch) return;
            Debug.Log($"xxxx - PERFORMER: {performer.GetProviderEntityName()}");
        }

    }

#endif


}
