using System;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "MASTER - N [Buff]",
        menuName = "Combat/Effect/Buff MASTER")]
    public class SBuffMasterStat : SEffect
    {
        [SerializeField] private EnumStats.MasterStatType buffStat;
        public EnumStats.MasterStatType GetBuffType() => buffStat;

        protected override void DoEffectOn(
            SkillValuesHolders values, CombatingEntity effectTarget, float buffValue, bool isCritical)
        {
            var user = values.User;
            var targetStats = effectTarget.CombatStats.MasterStats;

            float finalBuffValue = UtilStats.CalculateBuffPower(user.CombatStats, buffValue, isCritical);

            switch (buffStat)
            {
                case EnumStats.MasterStatType.Offensive:
                    targetStats.Offensive += finalBuffValue;
                    break;
                case EnumStats.MasterStatType.Support:
                    targetStats.Support += finalBuffValue;
                    break;
                case EnumStats.MasterStatType.Vitality:
                    targetStats.Vitality += finalBuffValue;
                    break;
                case EnumStats.MasterStatType.Concentration:
                    targetStats.Concentration += finalBuffValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var resolution = new SkillComponentResolution(this,finalBuffValue);
            user.EventsHolder.OnPerformSupportAction(effectTarget,resolution);
            effectTarget.EventsHolder.OnReceiveSupportAction(user,resolution);
        }


        [Button]
        private void UpdateAssetName()
        {
            name = "MASTER - " + buffStat.ToString() + " [Buff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
