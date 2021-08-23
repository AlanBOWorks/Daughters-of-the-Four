using UnityEngine;

namespace Stats
{

    [CreateAssetMenu(fileName = "SPECIAL - N [Stats]",
        menuName = "Combat/Stats/Concentration")]
    public class SSpecialStats : ScriptableObject, ISpecialStats
    {
        [SerializeField] private float enlightenment;
        [SerializeField] private float criticalChance;
        [SerializeField] private float speedAmount;

        public float GetEnlightenment() => enlightenment;

        public float GetCriticalChance() => criticalChance;

        public float GetSpeedAmount() => speedAmount;

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
    }
}
