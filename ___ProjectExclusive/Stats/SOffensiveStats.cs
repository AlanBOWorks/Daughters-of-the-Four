using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "OFFENSIVE - N [Stats]", 
        menuName = "Combat/Stats/Offensive")]
    public class SOffensiveStats : SStatsBase, IOffensiveStats<float>
    {
        [SerializeField] private float attackPower;
        [SerializeField] private float deBuffPower;
        [SerializeField] private float staticDamagePower;
        

        public float AttackPower
        {
            get => attackPower;
            set => attackPower = value;
        }

        public float DeBuffPower
        {
            get => deBuffPower;
            set => deBuffPower = value;
        }

        public float StaticDamagePower
        {
            get => staticDamagePower;
            set => staticDamagePower = value;
        }

        public override void DoInjection(IBasicStats<float> stats)
        {
            UtilsStats.Add(stats,this);
        }
    }
}
