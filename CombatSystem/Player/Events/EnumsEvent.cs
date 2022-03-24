using UnityEngine;

namespace CombatSystem.Player.Events
{
    
    public static class EnumsEvent 
    {
        public enum SkillPointerStates
        {
            Idle,
            Hover,
            /// <summary>
            /// By exit the button; [<seealso cref="Idle"/> is forced by external events]
            /// </summary>
            Exit,
        }

        public enum SkillSelectionStates
        {
            Idle,
            FirstSelect,
            Switch,
            Deselect,
            Cancel,
            Submit
        }
    }
}

