using CombatSkills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Events
{
    public sealed class AnimationEventsHelper : IAnimationsListener<SkillValuesHolders>
    {
        private static SkillValuesHolders _values;
        private static bool _hasBeingInvoked;
        

        [Button]
        public static void Send_AnimationClimaxEvent()
        {
            CombatSystemSingleton.EventsHolder.OnAnimationClimax(_values);
            _hasBeingInvoked = true;
        }

        public void OnBeforeAnimation(SkillValuesHolders element)
        {
            _values = element;
            _hasBeingInvoked = false;
        }

        public void OnAnimationClimax(SkillValuesHolders element)
        {
        }

        public void OnAnimationHaltFinish(SkillValuesHolders element)
        {
            if(!_hasBeingInvoked)
                Send_AnimationClimaxEvent();
            _hasBeingInvoked = false;
        }
    }
}
