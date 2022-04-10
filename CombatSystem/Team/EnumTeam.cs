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
        public const int RoleTypesAmount = FlexIndex + 1;

        /// <summary>
        /// Total of [<see cref="RoleTypesAmount"/>]*2; this is for two teams Collection initializations 
        /// </summary>
        public const int OppositeTeamRolesAmount = RoleTypesAmount * 2;
        public enum Role
        {
            InvalidRole = InvalidIndex,
            Vanguard = VanguardIndex,
            Attacker = AttackerIndex,
            Support = SupportIndex,
            Flex = FlexIndex
        }

        /// <summary>
        /// Amount of OffRoleTypes = 2
        /// </summary>
        public const int OffRoleTypesLength = 2;
        /// <summary>
        /// The amount of OffRolesIndexes = 8
        /// </summary>
        public const int OffRoleIndexCount = RoleTypesAmount * 2;

        /// <summary>
        /// The index in an array for secondary Roles = 0;
        /// </summary>
        public const int SecondaryRoleInOffArrayIndex = 0;
        /// <summary>
        /// The index in an array for secondary Roles = 1
        /// </summary>
        public const int ThirdRoleInOffArrayIndex = SecondaryRoleInOffArrayIndex + 1;
        /// <summary>
        /// The index in an array for FullTeam Roles = 2
        /// </summary>
        public const int MainRoleInFullTeamArrayIndex = ThirdRoleInOffArrayIndex + 1; //In full team comp, mains are placed in last

        public const int SecondaryVanguardIndex = RoleTypesAmount;
        public const int SecondaryAttackerIndex = SecondaryVanguardIndex + 1;
        public const int SecondarySupportIndex = SecondaryAttackerIndex + 1;
        public const int SecondaryFlexIndex = SecondarySupportIndex + 1;

        public const int ThirdVanguardIndex = SecondaryFlexIndex + 1;
        public const int ThirdAttackerIndex = ThirdVanguardIndex + 1;
        public const int ThirdSupportIndex = ThirdAttackerIndex + 1;
        public const int ThirdFlexIndex = ThirdSupportIndex + 1;

        public enum ActiveRole
        {
            InvalidRole = InvalidIndex,
            MainVanguard = VanguardIndex,
            MainAttacker = AttackerIndex,
            MainSupport = SupportIndex,
            MainFlex = FlexIndex,

            SecondaryVanguard = SecondaryVanguardIndex,
            SecondaryAttacker = SecondaryAttackerIndex,
            SecondarySupport = SecondarySupportIndex,
            SecondaryFlex = SecondaryFlexIndex,

            ThirdVanguard = ThirdVanguardIndex,
            ThirdAttacker = ThirdAttackerIndex,
            ThirdSupport = ThirdSupportIndex,
            ThirdFlex = ThirdFlexIndex
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

        public const int MainRolesIndexInFullTeamArray = 0;
        /// <summary>
        /// Amount of Roles for Main + Off entities = 1+2
        /// </summary>
        public const int TeamAlimentRolesLength = 1+OffRoleTypesLength;
        /// <summary>
        /// The total amount of indexes in a FullTeam Structure (Mains + Offs)
        /// </summary>
        public const int FullTeamIndexCount = RoleTypesAmount+OffRoleIndexCount;
    }
}
