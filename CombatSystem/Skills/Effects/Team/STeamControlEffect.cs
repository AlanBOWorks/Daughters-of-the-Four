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
        [SerializeField] private bool isBurst;
        private string _effectTag;

        public override EnumsEffect.ConcreteType EffectType => (isBurst)
            ? EnumsEffect.ConcreteType.ControlBurst 
            : EnumsEffect.ConcreteType.ControlGain;

        private const string BurstName = EffectTags.BurstControlEffectName;
        private const string GainName = EffectTags.GainControlEffectName;

        private string GetControlName() => isBurst ? BurstName : GainName;
        public override string EffectTag => _effectTag;

        private const string BurstSmallPrefix = EffectTags.BurstControlEffectPrefix;
        private const string GainSmallPrefix = EffectTags.GainControlEffectPrefix;
        public override string EffectSmallPrefix => isBurst ? BurstSmallPrefix : GainSmallPrefix;

        private void OnEnable()
        {
            _effectTag = GetControlName().ToUpper() + "_" + EffectPrefix;
        }

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var targetTeam = target.Team;
            bool isAlly = UtilsTeam.IsAllyEntity(in performer, in targetTeam);
            if (!isAlly)
            {
                //todo make enemy Control variation
            }

            if (isBurst)
            {
                UtilsCombatTeam.BurstControl(in targetTeam,in effectValue);
            }
            else
            {
                UtilsCombatTeam.GainControl(in targetTeam, in effectValue);
            }
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
