using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N (Simple) - Temporal Stats PASSIVE C - [Preset]",
        menuName = "Combat/Passive/Temporal Stats Boots")]
    public class SPassiveTemporalStatsBoost : SInjectionPassiveBase, ICombatTemporalStats
    {

        [TitleGroup("Params")]
        [InfoBox("If false this is a Base Buff Type", InfoMessageType.Warning)]
        [SerializeField] private bool isBurstType = false;

        [TitleGroup("Stats")]
        [HideIf("isBurstType")]
        [SerializeField] private float healthPoints;
        [HideIf("isBurstType")]
        [SerializeField] private float shieldAmount;
        [HideIf("isBurstType")]
        [SerializeField] private float mortalityPoints;
        [HideIf("isBurstType")]
        [SerializeField] private float harmonyAmount;
        [SerializeField] private float initiativePercentage;
        [SerializeField] private int actionsPerInitiative;

        public override void InjectPassive(CombatingEntity target)
        {
            ICharacterFullStats stats;
            if (isBurstType)
                stats = target.CombatStats.BurstStats;
            else
            {
                stats = target.CombatStats.BaseStats;
                stats.HealthPoints += healthPoints;
                stats.ShieldAmount += shieldAmount;
            }

            
            stats.MortalityPoints += mortalityPoints;
            stats.InitiativePercentage += initiativePercentage;
            stats.ActionsPerInitiative += actionsPerInitiative;

        }

        public float HealthPoints
        {
            get => healthPoints;
            set => healthPoints = value;
        }

        public float ShieldAmount
        {
            get => shieldAmount;
            set => shieldAmount = value;
        }

        public float MortalityPoints
        {
            get => mortalityPoints;
            set => mortalityPoints = value;
        }

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }
    }
}
