using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Global presets to get generic values; <br></br>
    /// <seealso cref="Skill"/> are the specific skills for individual <seealso cref="SCharacterSkillsPreset"/>
    /// </summary>
    [CreateAssetMenu(fileName = "N (T) - SKILL L - [Preset]",
        menuName = "Combat/Skill Preset")]
    public class SSkillPreset : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField, TextArea] private string skillName = "NULL";
        public string SkillName => skillName;

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;
        public Sprite Icon => icon;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"),ShowIf("canCrit")]
        public float criticalAddition = 0f;
        

        [TitleGroup("Effects")]
        [SerializeField]
        private List<EffectParams> effects = new List<EffectParams>(1);
        public List<EffectParams> Effects => effects;
        public SEffectBase.EffectType MainEffectType => effects[0].GetEffectType();
        public SEffectBase.EffectTarget MainEffectTarget => effects[0].GetEffectTarget();


    }

    public abstract class SEffectBase : ScriptableObject
    {

        [SerializeField] protected EffectType effectType = EffectType.Support;

        public EffectType GetEffectType() => effectType;
        public abstract void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1);


        public enum EffectType
        {
            SelfOnly,
            Offensive,
            Support,
            Other = SelfOnly
        }
        public enum EffectTarget
        {
            Target,
            /// <summary>
            /// The target team excluding the Target
            /// </summary>
            TargetTeamExcluded,
            TargetTeam,
            All
        }

    }


    //its a class instead a struct because it's gone be used by a lot of entities as a reference
    [Serializable]
    public class EffectParams 
    {
        [Title("Preset")]
        public SEffectBase effectPreset;
        [TitleGroup("Stats"), Range(0, 1000), SuffixLabel("%00")]
        public float power = 1;
        //TODO Conditionals (Chance, condition to apply effect, etc)
        public SEffectBase.EffectType GetEffectType() => effectPreset.GetEffectType();
        [SerializeField] 
        private SEffectBase.EffectTarget effectTarget = SEffectBase.EffectTarget.Target;
        public SEffectBase.EffectTarget GetEffectTarget() => effectTarget;

        public void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            effectPreset.DoEffect(user,target,power * randomModifier);
        }

        //There could be additional parameters such randomness, area, etc
    }
}
