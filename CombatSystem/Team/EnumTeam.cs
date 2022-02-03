namespace CombatSystem.Team
{
    public static class EnumTeam
    {
        public const int VanguardIndex = 0;
        public const int AttackerIndex = VanguardIndex + 1;
        public const int SupportIndex = AttackerIndex + 1;
        public const int RolesAmount = SupportIndex + 1;
        public enum Role
        {
            Vanguard = VanguardIndex,
            Attacker = AttackerIndex,
            Support = SupportIndex
        }


        public const int FrontLineIndex = VanguardIndex;
        public const int MidLineIndex = AttackerIndex;
        public const int BackLineIndex = SupportIndex;
        public const int PositioningAmount = BackLineIndex + 1;

        public enum Positioning
        {
            FrontLine = FrontLineIndex,
            MidLine = MidLineIndex,
            BackLine = BackLineIndex
        }

        public enum Stance
        {
            Neutral,
            Attacking,
            Defending
        }
        public enum StanceFull
        {
            Neutral = Stance.Neutral,
            Attacking = Stance.Attacking,
            Defending = Stance.Defending,
            Disrupted
        }
    }
}
