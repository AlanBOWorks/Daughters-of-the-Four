using System.Collections.Generic;
using CombatSkills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Events
{
    public class SkillEvents : ISkillEventListener
    {
        public SkillEvents()
        {
            _listeners = new List<ISkillEventListener>();
        }

        [ShowInInspector]
        private readonly List<ISkillEventListener> _listeners;

        public void Subscribe(ISkillEventListener listener)
        {
            _listeners.Add(listener);
        }
        public void OnSkillUse(SkillValuesHolders values)
        {
            foreach (var listener in _listeners)
            {
                listener.OnSkillUse(values);
            }
        }
    }

    public interface ISkillEventListener
    {
        void OnSkillUse(SkillValuesHolders values);
    }

#if UNITY_EDITOR
    internal class DebugSkillEvents : ISkillEventListener
    {
        public void OnSkillUse(SkillValuesHolders values)
        {
            Debug.Log($"Used Skill: {values.UsedSkill.GetSkillName()}____ \n" +
                      $"- Performer >>> {values.Performer.GetEntityName()} \n" +
                      $"- Target >>> {values.Target.GetEntityName()}");
        }
    }


#endif
}
