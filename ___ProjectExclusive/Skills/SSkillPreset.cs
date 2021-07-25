using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Global presets to get generic values; <br></br>
    /// <seealso cref="Skill"/> are the specific skills for individual <seealso cref="SCharacterSkillsPreset"/>
    /// </summary>
    [CreateAssetMenu(fileName = "N (T) - SKILL L - [Preset]",
        menuName = "Combat/Skill/Skill Preset")]
    public class SSkillPreset : SSkillPresetBase
    {
        
    }

    public abstract class SSkillPresetBase : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField,Delayed] 
        private string skillName = "NULL";
        public string SkillName => skillName;

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;
        public Sprite Icon => icon;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;
        [TitleGroup("Stats"), Range(0, 100), SuffixLabel("actions")]
        [SerializeField]
        private int cooldownCost = 1;
        public int CoolDownCost => cooldownCost;


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


        [TitleGroup("Effects")]
        [SerializeField]
        private EffectParams[] effects = new EffectParams[1];
        public IEffect GetMainEffect() => effects[0];
        public IEffect GetEffect(int index) => effects[index];
        public int GetEffectAmount() => effects.Length;

        [Button(ButtonSizes.Large)]
        protected virtual void UpdateAssetName()
        {
            ValidateEffects();
            var mainEffect = GetMainEffect();
            if (mainEffect != null)
            {
                name = ValidationName(mainEffect);
            }
            UtilsGame.UpdateAssetName(this);
        }

        protected void ValidateEffects()
        {
            foreach (EffectParams effectParams in effects)
            {
                effectParams.OnValidateEffects();
            }
        }

        protected string ValidationName(IEffect mainEffect)
        {
            string typeString = $" - [{cooldownCost}] - (" + mainEffect.GetEffectTarget().ToString().ToUpper() + ") ";
            return skillName + typeString + " - [SKILL Preset]";
        }
    }

}
