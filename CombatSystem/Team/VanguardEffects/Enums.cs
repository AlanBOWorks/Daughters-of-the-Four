namespace CombatSystem.Team.VanguardEffects
{
    public static class EnumsVanguardEffects
    {
        public enum VanguardEffectType
        {
            /// <summary>
            /// Type for waiting until can be performed (by sequence or condition)
            /// </summary>
            DelayImprove,
            /// <summary>
            /// Type for effects which needs the Vanguard being attacked to be trigger
            /// </summary>
            Revenge,
            /// <summary>
            /// Type for effects which needs the Vanguard being ignored to be trigger
            /// </summary>
            Punish
        }
    }
}
