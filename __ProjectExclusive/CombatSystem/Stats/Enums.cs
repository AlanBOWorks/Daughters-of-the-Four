using UnityEngine;

namespace Stats
{
    public static class EnumStats
    {
        public enum BehaviourType
        {
            Base,
            Buff,
            Burst
        }

        public enum OffensiveType
        {
            Direct,
            Persistent
        }
        public enum RecoveryType
        {
            Heal,
            Shield,
            Guard
        }

        public enum DamageResult
        {
            /// <summary>
            /// Non important damage was dealt
            /// </summary>
            None,
            /// <summary>
            /// Shields were lost
            /// </summary>
            ShieldBreak,
            /// <summary>
            /// Health reached 0
            /// </summary>
            HealthLost,
            /// <summary>
            /// Mortality reached 0
            /// </summary>
            Death
        }

        public enum RangeType
        {
            Melee,
            Ranged,
            /// <summary>
            /// Both Melee and Ranged; But with a preference towards Melee
            /// </summary>
            HybridMelee,
            /// <summary>
            /// Both Melee and Ranged, But with a preference towards Ranged
            /// </summary>
            HybridRanged //Means for common skills; selectable/unique skills can be both Melee and Ranged
        }
    }
}
