using System;

namespace CombatSystem.Team
{
    public static class EnumTeam
    {
        public enum StructureType
        {
            TeamRole,
            TeamPosition
        }

        public const int InvalidIndex = -1;
        public const int VanguardIndex = 0;
        public const int AttackerIndex = VanguardIndex + 1;
        public const int SupportIndex = AttackerIndex + 1;
        public const int FlexIndex = SupportIndex + 1;

        /// <summary>
        /// Amount of Role up to [<seealso cref="SupportIndex"/>]
        /// </summary>
        public const int BasicRolesAmount = SupportIndex + 1;
        /// <summary>
        /// Total of role, including up to [<seealso cref="FlexIndex"/>]
        /// </summary>
        public const int RolesAmount = FlexIndex + 1;

        /// <summary>
        /// Total of [<see cref="RolesAmount"/>]*2; this is for two teams Collection initializations 
        /// </summary>
        public const int OppositeTeamRolesAmount = RolesAmount * 2;
        public enum Role
        {
            InvalidRole = InvalidIndex,
            Vanguard = VanguardIndex,
            Attacker = AttackerIndex,
            Support = SupportIndex,
            Flex = FlexIndex
        }

        public enum ActiveRole
        {
            InvalidRole = InvalidIndex,
            MainVanguard = VanguardIndex,
            MainAttacker = AttackerIndex,
            MainSupport = SupportIndex,
            MainFlex = FlexIndex,
            SecondaryVanguard,
            SecondaryAttacker,
            SecondarySupport,
            SecondaryFlex
        }


        public const int FrontLineIndex = VanguardIndex;
        public const int MidLineIndex = AttackerIndex;
        public const int BackLineIndex = SupportIndex;
        public const int FlexLineIndex = BackLineIndex + 1;
        public const int BasicPositioningAmount = BackLineIndex + 1;
        public const int PositioningAmount = FlexLineIndex + 1;

        public enum Positioning
        {
            FrontLine = FrontLineIndex,
            MidLine = MidLineIndex,
            BackLine = BackLineIndex,
            FlexLine = FlexLineIndex
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



        public static int GetRoleIndex(in Role role)
        {
            switch (role)
            {
                case Role.Vanguard:
                    return VanguardIndex;
                case Role.Attacker:
                    return AttackerIndex;
                case Role.Support:
                    return SupportIndex;
                case Role.Flex:
                    return FlexLineIndex;
                default:
                    return InvalidIndex;
            }
        }

        public static int GetPositioningIndex(in Positioning positioning)
        {
            switch (positioning)
            {
                case Positioning.FrontLine:
                    return FrontLineIndex;
                case Positioning.MidLine:
                    return MidLineIndex;
                case Positioning.BackLine:
                    return BackLineIndex;
                case Positioning.FlexLine:
                    return FlexLineIndex;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }
        }
    }
}
