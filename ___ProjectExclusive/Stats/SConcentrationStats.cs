using UnityEngine;

namespace Stats
{

    [CreateAssetMenu(fileName = "CONCENTRATION - N [Stats]",
        menuName = "Combat/Stats/Concentration")]
    public class SConcentrationStats : SStatsBase, IConcentrationStats<float>
    {
        [SerializeField] private float enlightenment;
        [SerializeField] private float criticalChance;
        [SerializeField] private float speedAmount;


        public float Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }

        public float CriticalChance
        {
            get => criticalChance;
            set => criticalChance = value;
        }

        public float SpeedAmount
        {
            get => speedAmount;
            set => speedAmount = value;
        }

        public override void DoInjection(IBasicStats<float> stats)
        {
            UtilsStats.Add(stats,this);
        }
    }
}
