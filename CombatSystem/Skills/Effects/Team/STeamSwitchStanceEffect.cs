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
        menuName = "Combat/Effect/Team/Stance")]
    public class STeamSwitchStanceEffect : SEffect, ITeamEffect
    {
        [SerializeField] private EnumTeam.Stance stance;
        private string _effectTag;
        private const string ControlEffectName = EffectTags.StanceEffectName;

        private void OnEnable()
        {
            _effectTag = ControlEffectName + "_" + stance + "_" + EffectPrefix;
        }

        private const string StanceSmallPrefix = EffectTags.StanceEffectPrefix;
        public override string EffectTag => _effectTag;
        public override string EffectSmallPrefix => StanceSmallPrefix;
        public override EnumStats.StatType EffectType => EnumStats.StatType.Control;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var targetTeam = target.Team;
            bool isAlly = UtilsTeam.IsAllyEntity(in performer, in targetTeam);
            if (!isAlly)
            {
                //todo make switch enemyStance
            }

            UtilsCombatTeam.SwitchStance(in targetTeam, in stance);
        }


        protected string GenerateAssetName(string stanceType)
        {
            string header = "Team STANCE - ";
            string suffix = " [Effect]";

            return header + stanceType + suffix;
        }

        [Button]
        protected void UpdateAssetName()
        {
            string generatedName = GenerateAssetName(stance.ToString());
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }
}
