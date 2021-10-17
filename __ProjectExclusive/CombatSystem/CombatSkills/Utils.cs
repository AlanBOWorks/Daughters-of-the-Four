using System;
using CombatEntity;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    public static class UtilSkills
    {
       
    }

    public static class EnumSkills
    {
        /// <summary>
        /// [Idle, InCooldown]
        /// </summary>
        public enum SKillState
        {
            Idle,
            InCooldown
            //Persistent??
        }

        /// <summary>
        /// [Self, Support, Offensive]
        /// </summary>
        public enum TargetType
        {
            Self,
            Support,
            Offensive
        }
    }
}
