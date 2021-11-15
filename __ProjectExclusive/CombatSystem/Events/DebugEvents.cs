using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem.Events
{
#if UNITY_EDITOR
    internal sealed class DebugEvents : ITempoListener<CombatingEntity>, ITempoTickListener, ISkillEventListener
    {
        public bool LogTempoEvents = true;
        public bool LogTickStep = true;
        public bool LogSkillEvents = true;

        public void OnFirstAction(CombatingEntity element)
        {
            if(LogTempoEvents)
                Debug.Log($"------- ENTITY (STARTS) --------- \n N: {element.GetEntityName()}");
        }

        public void OnFinishAction(CombatingEntity element)
        {
            if(LogTempoEvents)
                Debug.Log(">>> Finish Action");
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            if(LogTempoEvents)
                Debug.Log($"------- ENTITY (FINISH) --------- \n N: {element.GetEntityName()}");
        }

        public void OnTickStep(float seconds)
        {
            if(LogTickStep)
                Debug.Log($"xxx- TICKING: {seconds}sec");
        }

        public void OnSkillUse(SkillValuesHolders values)
        {
            if(LogSkillEvents)
                Debug.Log($"Used Skill: {values.UsedSkill.GetSkillName()}____ \n" +
                      $"- Performer >>> {values.Performer.GetEntityName()} \n" +
                      $"- Target >>> {values.Target.GetEntityName()}");
        }

        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
            if(LogSkillEvents)
                Debug.Log($"Skill - {values.UsedSkill.GetSkillName()} cost: {values.UsedSkill.GetUseCost()}");
        }
    }


    internal class DebugEventsWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private DebugEvents _debugEvents;

        [HideIf("_debugEvents"), Button]
        private void AddToSystem()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var tempo = CombatSystemSingleton.TempoTicker;

            DebugEvents events = new DebugEvents();
            _debugEvents = events;

            eventsHolder.Subscribe(events);
            tempo.TickListeners.Add(events);
        }

        [ShowIf("_debugEvents"), Button]
        private void RemoveFromSystem()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var tempo = CombatSystemSingleton.TempoTicker;

            eventsHolder.UnSubscribe(_debugEvents);
            tempo.TickListeners.Add(_debugEvents);

            _debugEvents = null;
        }


        [MenuItem("Debug/Events Logger")]
        private static void OpenWindow()
        {
            var window = GetWindow<DebugEventsWindow>();
            window.Show();
        }
    }
#endif
}
