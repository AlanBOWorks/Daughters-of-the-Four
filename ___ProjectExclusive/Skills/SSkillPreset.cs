using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills
{
    /// <summary>
    /// Global presets to get generic values; <br></br>
    /// <seealso cref="Skill"/> are the specific skills for individual <seealso cref="SCharacterSkillsPreset"/>
    /// </summary>
    [CreateAssetMenu(fileName = "N (T) - SKILL L - [Preset]",
        menuName = "Combat/Skill Preset")]
    public class SSkillPreset : SSkillPresetBase
    {
        [TitleGroup("Effects")]
        [SerializeField]
        private List<EffectParams> effects = new List<EffectParams>(1);
        public override IEffect GetEffect(int index) => effects[index];
        public override int GetEffectAmount() => effects.Count;
    }

    public abstract class SSkillPresetBase : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField, TextArea] private string skillName = "NULL";
        public string SkillName => skillName;

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;
        public Sprite Icon => icon;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;

        [TitleGroup("Targeting")]
        [SerializeField] protected bool canTargetSelf = false;
        public bool CanTargetSelf() => canTargetSelf;
        [SerializeField] protected SkillType skillType = SkillType.Support;
        public SkillType GetSkillType() => skillType;
        public enum SkillType
        {
            SelfOnly,
            Offensive,
            Support,
            Other = SelfOnly
        }

        public IEffect GetMainEffect() => GetEffect(0);
        public abstract IEffect GetEffect(int index);
        public abstract int GetEffectAmount();

    }

    public abstract class SEffectBase : ScriptableObject
    {
        public abstract void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1);

        /// <summary>
        /// Checks if the effect has fail in the random check
        /// </summary>
        protected static bool FailRandom(float effectModifier)
        {
            return Random.value > effectModifier || effectModifier <= 0;
        }

        public enum EffectTarget
        {
            Target,
            /// <summary>
            /// The target team excluding the Target
            /// </summary>
            TargetTeamExcluded,
            TargetTeam,
            Self,
            SelfTeam,
            SelfTeamNotIncluded,
            All
        }

    }


    //its a class instead a struct because it's gone be used by a lot of entities as a reference
    [Serializable]
    public class EffectParams : EffectParamsBase
    {
        [Title("Preset")]
        public SEffectBase effectPreset;
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            effectPreset.DoEffect(user,target,power * randomModifier);
        }

        //There could be additional parameters such randomness, area, etc
    }

    [Serializable]
    public abstract class EffectParamsBase : IEffect
    {
        [TitleGroup("Stats"), Range(0, 1000), SuffixLabel("%00")]
        public float power = 1;
        //TODO Conditionals (Chance, condition to apply effect, etc)
        [SerializeField]
        private SEffectBase.EffectTarget effectTarget = SEffectBase.EffectTarget.Target;
        public SEffectBase.EffectTarget GetEffectTarget() => effectTarget;

        [Tooltip("If the effect will apply a small variation into the [Power] value")]
        public bool applyRandomness = true;
        public bool CanPerformRandom() => applyRandomness;

        public abstract void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier);
    }

    public interface IEffect
    {
        void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier);
        SEffectBase.EffectTarget GetEffectTarget();
        bool CanPerformRandom();
    }
}
