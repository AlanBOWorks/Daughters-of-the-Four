using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "TEMPORAL(base) - N [Stats]",
        menuName = "Combat/Stats/Temporal(Base)")]
    public class STemporalStats : SStatsBase, ITemporalStats<float>
    {
        [SerializeField] private float initiativePercentage;
        [SerializeField] private float actionsPerInitiative;
        [SerializeField] private float harmonyAmount;
        public float InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public float ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }

        public override void DoInjection(IBasicStats<float> stats)
        {
            UtilsStats.Add(stats, this);
        }
    }
}
