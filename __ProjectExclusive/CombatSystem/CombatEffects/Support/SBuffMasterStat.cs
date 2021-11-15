using System;
using __ProjectExclusive.Localizations;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "MASTER - N [Buff]",
        menuName = "Combat/Effect/Buff MASTER", order = 101)]
    public class SBuffMasterStat : SEffect
    {
        [SerializeField] private EnumStats.MasterStatType buffStat;
        public EnumStats.MasterStatType GetBuffType() => buffStat;

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float buffValue,
            bool isCritical)
        {
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
            return new SkillComponentResolution(this, finalBuffValue);
        }
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            CombatSystemSingleton.EventsHolder.OnReceiveSupportEffect(entities,ref resolution);
        }
        
        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Buff;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Buff;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("F1") + "% " + TranslatorStats.GetText(buffStat);
        }




        [Button]
        protected virtual void UpdateAssetName()
        {
            name = "MASTER - " + buffStat.ToString() + " [Buff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
