namespace CombatSystem
{
    public static class EnumCombat
    {
        public const int FTierIndex = -1;
        public const int ETierIndex = 0;
        public const int ExTierIndex = 12;
        public const int DeuxTierIndex = 21;

        public enum RankingTier
        {
            F = FTierIndex,
            E = ETierIndex,
            D,
            C,
            B,
            A,
            S,
            SS,
            SSS,
            Ex = ExTierIndex,
            Deux = DeuxTierIndex
        }

        public enum QualityTier
        {
            Common,
            Rare,
            Epic,
            Legend,
            Transcended
        }
    }
}
