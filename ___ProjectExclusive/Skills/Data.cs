using System.Collections.Generic;
using Characters;
using CombatEffects;
using Stats;
using UnityEngine;

namespace Skills
{
    public class SkillArguments
    {
        public CombatingEntity User;
        public IFullStatsData<float> UserStats;
        public CombatingEntity InitialTarget;
        public bool IsCritical;
    }

    public class SkillFeedback
    {
        public SkillFeedback()
        {
            EffectResolutions = new Queue<EffectResolution>();
        }


        public CombatSkill UsedSkill;
        public CombatingEntity User;
        public Queue<EffectResolution> EffectResolutions;

        public void Clear()
            => EffectResolutions.Clear();
    }

    public struct EffectResolution
    {
        public readonly CombatingEntity OnTarget;
        public readonly IEffectBase Effect;
        public readonly float Value;

        public EffectResolution(CombatingEntity onTarget, IEffectBase effect, float value)
        {
            OnTarget = onTarget;
            Effect = effect;
            Value = value;
        }
    }
}
