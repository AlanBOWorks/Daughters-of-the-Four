using System;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Buff/MASTER", order = -10)]
    public class SBuffMasterStatEffect : SEffect
    {
        [SerializeField] private EnumStats.MasterStatType type;
        private string _effectTag;

        private const string MasterBuffEffectSmallPrefix = EffectTags.BuffEffectName;
        public override string EffectSmallPrefix => MasterBuffEffectSmallPrefix;
        public override EnumStats.StatType EffectType => EnumStats.StatType.Buff;

        private void OnEnable()
        {
            _effectTag = "BuffMASTER_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;

        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsEffects.CalculateBuffPower(in performerStats);
            float receivePower = UtilsStatsEffects.CalculateBuffReceivePower(in targetStats);

            float targetValue = effectValue * (bufferPower + receivePower);

            DoBuff(in targetStats, in targetValue);
        }

        private void DoBuff(in CombatStats stats, in float addingValue)
        {
            switch (type)
            {
                case EnumStats.MasterStatType.Offensive:
                    UtilsBuffStats.MasterBuffOffensive(stats.BuffStats, in addingValue);
                    break;
                case EnumStats.MasterStatType.Support:
                    UtilsBuffStats.MasterBuffSupport(stats.BuffStats, in addingValue);
                    break;
                case EnumStats.MasterStatType.Vitality:
                    UtilsBuffStats.MasterBuffVitality(stats.BuffStats, in addingValue);
                    break;
                case EnumStats.MasterStatType.Concentration:
                    UtilsBuffStats.MasterBuffConcentration(stats.BuffStats, in addingValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        protected string GenerateAssetName(string statTypeName)
        {
            const string buffType = "MASTER Stat - ";
            const string suffix = " [Effect]";

            return buffType + statTypeName + suffix;
        }

        [Button]
        protected void UpdateAssetName()
        {
            string generatedName = GenerateAssetName(type.ToString());
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }
}
