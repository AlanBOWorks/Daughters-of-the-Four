using System;
using CombatSystem.Entity;
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
        public override string EffectTag => _effectTag;

        private void OnEnable()
        {
            _effectTag = "CONTROL_" + GetBurstSuffix() + "_" + EffectPrefix;
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

        private const string BurstPrefix = "BURST";
        private const string GainPrefix = "GAIN";

        private string GetBurstSuffix() => isBurst ? BurstPrefix : GainPrefix;

        [Button]
        protected void UpdateAssetName()
        {
            const string header = "Team CONTROL - ";
            const string suffix = " [Effect]";

            var body = GetBurstSuffix();

            string generatedName = header + body + suffix;
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }
}
