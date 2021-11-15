using System;
using __ProjectExclusive.Localizations;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "CONCENTRATION - N [Buff]",
        menuName = "Combat/Effect/Buff Concentration", order = 100)]
    public class SBuffConcentration : SBuff
    {
        [Title("Params")]
        [SerializeField] private EnumStats.ConcentrationStatType buffStat;
        public EnumStats.ConcentrationStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoBuffOn(
            CombatingEntity performer, IBaseStats<float> targetStats, float buffValue, bool isCritical)
        {
            float finalBuffValue = CalculateBuffStats(performer, buffValue, isCritical);

            switch (buffStat)
            {
                case EnumStats.ConcentrationStatType.InitiativeSpeed:
                    targetStats.InitiativeSpeed += finalBuffValue;
                    break;
                case EnumStats.ConcentrationStatType.InitialInitiative:
                    targetStats.InitialInitiative += finalBuffValue;
                    break;
                case EnumStats.ConcentrationStatType.ActionsPerSequence:
                    targetStats.ActionsPerSequence += finalBuffValue;
                    break;
                case EnumStats.ConcentrationStatType.Critical:
                    targetStats.Critical += finalBuffValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SkillComponentResolution(this, finalBuffValue);
        }
        protected override string GetBuffTooltip()
        {
            return TranslatorStats.GetText(buffStat);
        }


        [Button]
        private void UpdateAssetName()
        {
            name = "CONCENTRATION - " + buffStat.ToString() + " [Buff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}