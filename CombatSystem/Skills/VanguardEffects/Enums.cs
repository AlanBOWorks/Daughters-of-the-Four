namespace CombatSystem.Skills.VanguardEffects
{
    public static class EnumsVanguardEffects
    {
        public enum VanguardEffectType
        {
            /// <summary>
            /// Type for effects which needs the Vanguard being attacked to be trigger
            /// </summary>
            Revenge = 1,
            /// <summary>
            /// Type for effects which needs the Vanguard being ignored to be trigger
            /// </summary>
            Punish
        }
    }
}
