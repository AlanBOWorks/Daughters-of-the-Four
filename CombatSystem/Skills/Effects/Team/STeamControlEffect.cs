using System;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{

    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Team/Control")]
    public class STeamControlEffect : SEffect, ITeamEffect
    {
        private string _effectTag;

        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.ControlGain;

        private const string GainName = EffectTags.GainControlEffectName;

        private string GetControlName() => GainName;
        public override string EffectTag => _effectTag;

        private const string GainSmallPrefix = EffectTags.GainControlEffectPrefix;
        public override string EffectSmallPrefix => GainSmallPrefix;

        private void OnEnable()
        {
            _effectTag = GetControlName().ToUpper() + "_" + EffectPrefix;
        }

        public override void DoEffect(EntityPairInteraction entities, float effectValue)
        {
            entities.Extract(out var performer, out var target);
            var targetTeam = target.Team;
            bool isAlly = UtilsTeam.IsAllyEntity(performer, targetTeam);
            if (!isAlly)
            {
                //todo make enemy Control variation
            }

            UtilsCombatTeam.GainControl(targetTeam, effectValue);
        }



        [Button]
        protected void UpdateAssetName()
        {
            const string header = "Team CONTROL - ";
            const string suffix = " [Effect]";

            var body = GetControlName();

            string generatedName = header + body + suffix;
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }
}
