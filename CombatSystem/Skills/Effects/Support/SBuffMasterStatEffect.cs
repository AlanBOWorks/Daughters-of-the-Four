using System;
using CombatSystem._Core;
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
    public class SBuffMasterStatEffect : SEffect, IBuffEffect
    {
        [SerializeField] private EnumStats.MasterStatType type;
        private string _effectTag;

        private const string MasterBuffEffectSmallPrefix = EffectTags.BuffEffectName;
        public override string EffectSmallPrefix => MasterBuffEffectSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Buff;

        private const string BuffValuePrefix = "%";

        private void OnEnable()
        {
            _effectTag = "BuffMASTER_" + type + "_" + EffectPrefix;
        }
        public override string EffectTag => _effectTag;

        public override bool IsPercentSuffix() => true;
        public override float CalculateEffectByStatValue(CombatStats performerStats, float effectValue)
        {
            return effectValue * UtilsStatsFormula.CalculateBuffPower(performerStats);
        }


        public override void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
        {
            entities.Extract(out var performer, out var target);
            var performerStats = performer.Stats;
            var targetStats = target.Stats;
            float bufferPower = UtilsStatsFormula.CalculateBuffPower(performerStats);
            float receivePower = UtilsStatsFormula.CalculateReceiveBuffPower(targetStats);

            effectValue = UtilsStatsEffects.CalculateStatsBuffValue(effectValue, bufferPower, receivePower);
            effectValue *= luckModifier;

            DoBuff(targetStats, effectValue);
            CombatSystemSingleton.EventsHolder.OnBuffDone(entities,this, effectValue);
        }

        public bool IsBurstEffect() => false;

        private void DoBuff(CombatStats stats, float addingValue)
        {
            switch (type)
            {
                case EnumStats.MasterStatType.Offensive:
                    UtilsBuffStats.MasterBuffOffensive(stats.BuffStats, addingValue);
                    break;
                case EnumStats.MasterStatType.Support:
                    UtilsBuffStats.MasterBuffSupport(stats.BuffStats, addingValue);
                    break;
                case EnumStats.MasterStatType.Vitality:
                    UtilsBuffStats.MasterBuffVitality(stats.BuffStats, addingValue);
                    break;
                case EnumStats.MasterStatType.Concentration:
                    UtilsBuffStats.MasterBuffConcentration(stats.BuffStats, addingValue);
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

        public string GetStatVariationEffectText() => LocalizeStats.LocalizeStatPrefix(type);
    }
}
