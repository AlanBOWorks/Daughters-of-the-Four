using System;
using Characters;
using UnityEngine;

namespace Stats
{

    [Serializable]
    public class CharacterUpgradeStats : IStatsUpgradable
    {
        [SerializeField] private float offensivePower;
        [SerializeField] private float supportPower;
        [SerializeField] private float vitalityAmount;
        [SerializeField] private float enlightenment;

        public CharacterUpgradeStats()
        { }

        public CharacterUpgradeStats(IStatsUpgradable copyFrom)
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        public float OffensivePower
        {
            get => offensivePower;
            set => offensivePower = value;
        }

        public float SupportPower
        {
            get => supportPower;
            set => supportPower = value;
        }

        public float VitalityAmount
        {
            get => vitalityAmount;
            set => vitalityAmount = value;
        }

        public float Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }
    }
}
