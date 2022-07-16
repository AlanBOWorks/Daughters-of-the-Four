using UnityEngine;

namespace CombatSystem.Localization
{
    public sealed class EffectTags
    {
        // Names
        public const string DamageEffectName = "Damage";
        public const string HealEffectName = StatsTags.HealStatName;

        public const string BuffEffectName = StatsTags.BuffStatName;
        public const string BurstEffectName = "Burst";
        public const string DeBuffEffectName = StatsTags.DeBuffStatName;
        public const string DeBurstEffectName = "DeBurst";

        public const string ShieldingEffectName = StatsTags.ShieldingStatName;
        public const string GuardingEffectName = "Guarding";

        public const string RemoveControlEffectName = "Remove ControlGain";
        public const string GainControlEffectName = StatsTags.ControlStatName;

        public const string StanceEffectName = "Stance";
        public const string InitiativeEffectName = "Initiative";


        // Tags
        public const string DamageEffectTag = "Damage_Effect";
        public const string HealEffectTag = StatsTags.HealStatTag;

        public const string BuffEffectTag = StatsTags.BuffStatTag;
        public const string BurstEffectTag = "Burst_Effect";
        public const string DeBuffEffectTag = StatsTags.DeBuffStatTag;
        public const string DeBurstEffectTag = "DeBurst_Effect";

        public const string ShieldingEffectTag = StatsTags.ShieldingStatTag;
        public const string GuardingEffectTag = "Guarding_Effect";

        public const string BurstControlEffectTag = "Burst Control_Effect";
        public const string GainControlEffectTag = StatsTags.ControlStatTag;

        public const string StanceEffectTag = "Stance_Effect";
        public const string InitiativeEffectTag = "Initiative_Tag";

        // Prefix

        public const string DamageEffectPrefix = "Dmg";
        public const string HealEffectPrefix = StatsTags.HealStatPrefix;

        public const string BuffEffectPrefix = StatsTags.BuffStatPrefix;
        public const string BurstEffectPrefix = "Brst";
        public const string DeBuffEffectPrefix = StatsTags.DeBuffStatPrefix;
        public const string DeBurstEffectPrefix = "DBrst";

        public const string ShieldingEffectPrefix = StatsTags.ShieldingStatPrefix;
        public const string GuardingEffectPrefix = "Guard";

        public const string BurstControlEffectPrefix = "BCtrl";
        public const string GainControlEffectPrefix = StatsTags.ControlStatPrefix;

        public const string StanceEffectPrefix = "Stnc";
        public const string InitiativeEffectPrefix = "Init";
    }
}
