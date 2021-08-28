using UnityEngine;

namespace Stats
{

    [CreateAssetMenu(fileName = "VITALITY - N [Stats]",
        menuName = "Combat/Stats/Vitality")]
    public class SVitalityStats : SStatsBase, IVitalityStats<float>
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float maxMortalityPoints;
        [SerializeField] private float damageReduction;
        [SerializeField] private float deBuffReduction;


        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float MaxMortalityPoints
        {
            get => maxMortalityPoints;
            set => maxMortalityPoints = value;
        }

        public float DamageReduction
        {
            get => damageReduction;
            set => damageReduction = value;
        }

        public float DeBuffReduction
        {
            get => deBuffReduction;
            set => deBuffReduction = value;
        }

        public override void DoInjection(IBasicStats<float> stats)
        {
            UtilsStats.Add(stats, this);
        }
    }
}
