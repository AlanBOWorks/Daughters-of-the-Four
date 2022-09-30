using System;
using System.Collections.Generic;
using System.Globalization;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils_Project;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Vanguard Preset", order = -10)]
    public class SVanguardSkillPreset : SSkillPresetBase, IVanguardSkill
    {
        protected const string VanguardAssetPrefix =  " [Vanguard Skill]";

        [TitleGroup("Values")]
        [SerializeField]
        private bool ignoreSelf = true;

        [TitleGroup("Vanguard effects")]
        [SerializeField]
        private EnumsVanguardEffects.VanguardEffectType vanguardVisualType;
        [SerializeField]
        private PresetEffectValues[] counterEffects = new PresetEffectValues[0];
        [SerializeField] 
        private PresetEffectValues[] punishEffects = new PresetEffectValues[0];



        protected override string GetAssetPrefix() => VanguardAssetPrefix;


        public override IEffect GetMainEffectArchetype()
        {
            return null;
        }

        public override bool IgnoreSelf() => ignoreSelf;



        public override IEnumerable<PerformEffectValues> GetEffectsFeedBacks() 
            => GetEffects();

        public EnumsVanguardEffects.VanguardEffectType MainVanguardType => vanguardVisualType;

        public IEnumerable<PerformEffectValues> GetCounterEffects()
        {
            foreach (var effect in counterEffects)
                yield return effect.GenerateValues();
        }

        public bool HasCounterEffects() => counterEffects.Length > 0;

        public IEnumerable<PerformEffectValues> GetPunishEffects()
        {
            foreach (var effect in punishEffects)
                yield return effect.GenerateValues();
        }

        public bool HasPunishEffects() => punishEffects.Length > 0;


        public override EnumsSkill.TeamTargeting TeamTargeting => EnumsSkill.TeamTargeting.Self;
        


        protected override string GenerateAssetName()
        {
            return "#VANGUARD - " + base.GenerateAssetName();
        }



    }
}
