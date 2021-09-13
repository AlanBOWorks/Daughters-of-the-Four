using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace _Player
{
    public class PlayerCombatEvents : TempoEvents, IPlayerSkillListener, ITempoTriggerHandler
    {
        public readonly List<IPlayerSkillListener> SkillListeners;

        public PlayerCombatEvents()
        {
            SkillListeners = new List<IPlayerSkillListener>();
        }

        public void Subscribe(IPlayerSkillListener listener)
        {
            SkillListeners.Add(listener);
        }

        public void UnSubscribe(IPlayerSkillListener listener)
        {
            SkillListeners.Remove(listener);
        }

        public void OnSkillSelect(CombatSkill selectedSkill)
        {
            foreach (IPlayerSkillListener listener in SkillListeners)
            {
                listener.OnSkillSelect(selectedSkill);
            }
        }

        public void OnSkillDeselect(CombatSkill deselectSkill)
        {
            foreach (IPlayerSkillListener listener in SkillListeners)
            {
                listener.OnSkillDeselect(deselectSkill);
            }
        }

        public void OnSubmitSkill(CombatSkill submitSkill)
        {
            foreach (IPlayerSkillListener listener in SkillListeners)
            {
                listener.OnSubmitSkill(submitSkill);
            }
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnInitiativeTrigger(entity);
            }
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnDoMoreActions(entity);
            }
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            foreach (ITempoListener listener in TempoListeners)
            {
                listener.OnFinisAllActions(entity);
            }
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            foreach (IRoundListener listener in RoundListeners)
            {
                listener.OnRoundCompleted(allEntities, lastEntity);
            }
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            foreach (ISkippedTempoListener listener in SkippedListeners)
            {
                listener.OnSkippedEntity(entity);
            }
        }
    }

    public interface IPlayerTempoListener : ITempoListener
    { }

    public interface IPlayerRoundListener : IRoundListener 
    { }

    public interface IPlayerSkillListener<in T>
    {
        void OnSkillSelect(T selectedSkill);
        void OnSkillDeselect(T deselectSkill);
        void OnSubmitSkill(T submitSkill);
    }

    public interface IPlayerSkillListener : IPlayerSkillListener<CombatSkill> 
    { }

    public interface IPlayerButtonListener : IPlayerSkillListener<USkillButton>
    { }
}
