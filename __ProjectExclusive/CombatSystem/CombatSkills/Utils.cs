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
        public enum SKillState
        {
            Idle,
            InCooldown
            //Persistent??
        }

        public enum TargetType
        {
            Self,
            Support,
            Offensive
        }
    }
}
