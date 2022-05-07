using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.AI.Enemy;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.AI
{
    public class EnemyCombatEventsHolder : ControllerCombatEventsHolder, ITempoTickListener, IDiscriminationEventsHolder,
        IEnemyControllerListener
    {
        public EnemyCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);

            _controllerListeners = new HashSet<IEnemyControllerListener>();


#if UNITY_EDITOR
            Subscribe(CombatDebuggerSingleton.CombatEnemyLogs);
#endif
        }

        private readonly HashSet<IEnemyControllerListener> _controllerListeners;

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);

            if (listener is IEnemyControllerListener controllerListener)
                _controllerListeners.Add(controllerListener);
        }

        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is IEnemyControllerListener controllerListener)
                _controllerListeners.Remove(controllerListener);

        }

        public void OnControlEntitySelect(in CombatEntity selection)
        {
            foreach (var listener in _controllerListeners)
            {
                listener.OnControlEntitySelect(in selection);
            }
        }

        public void OnControlSkillSelect(in CombatSkill skill)
        {
            foreach (var listener in _controllerListeners)
            {
                listener.OnControlSkillSelect(in skill);
            }
        }

        public void OnTargetSelect(in CombatEntity target)
        {
            foreach (var listener in _controllerListeners)
            {
                listener.OnTargetSelect(in target);
            }
        }
    }

    public interface IEnemyControllerListener : ICombatEventListener
    {
        void OnControlEntitySelect(in CombatEntity selection);
        void OnControlSkillSelect(in CombatSkill skill);
        void OnTargetSelect(in CombatEntity target);
    }
}

