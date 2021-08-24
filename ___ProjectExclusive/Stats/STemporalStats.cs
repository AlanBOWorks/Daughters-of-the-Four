using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "TEMPORAL(base) - N [Stats]",
        menuName = "Combat/Stats/Temporal(Base)")]
    public class STemporalStats : ScriptableObject, ICombatTemporalStatsBase
    {
        [SerializeField] private float initiativePercentage;
        [SerializeField] private int actionsPerInitiative;
        [SerializeField] private float harmonyAmount;
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

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }
    }
}
