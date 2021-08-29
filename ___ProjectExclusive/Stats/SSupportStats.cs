using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "SUPPORT - N [Stats]",
        menuName = "Combat/Stats/Support")]
    public class SSupportStats : SStatsBase, ISupportStats<float>
    {
        [SerializeField] private float healPower;
        [SerializeField] private float buffPower;
        [SerializeField] private float buffReceivePower;


        public float HealPower
        {
            get => healPower;
            set => healPower = value;
        }

        public float BuffPower
        {
            get => buffPower;
            set => buffPower = value;
        }

        public float BuffReceivePower
        {
            get => buffReceivePower;
            set => buffReceivePower = value;
        }

        public override void DoInjection(IBasicStats<float> stats)
        {
            UtilsStats.Add(stats, this);
        }

        protected override string AssetPrefix()
        {
            return "Stat SUPPORT - ";
        }
    }
}
