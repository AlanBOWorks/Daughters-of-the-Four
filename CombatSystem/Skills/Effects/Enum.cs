using System;

namespace CombatSystem.Skills.Effects
{
    public static class EnumsEffect
    {

        public static string TargetTypeTag = "T";
        public static string TargetTeamTypeTag = "TT";
        public static string PerformerTypeTag = "P";
        public static string PerformerTeamTypeTag = "PT";
        public static string TargetLineTypeTag = "TL";
        public static string PerformerLineTypeTag = "PL";
        public static string AllTypeTag ="A";

        public enum TargetType
        {
            Target = 0,
            TargetLine = 1,
            TargetTeam = 2,

            Performer = 10,
            PerformerLine = 11,
            PerformerTeam = 12,


            MostDesired = 100
        }

        public enum Archetype
        {
            Others,
            Offensive,
            Support,
            Team
        }

        public enum ConcreteType
        {
            Undefined = -200,
            DefaultOffensive = -100,
            DefaultSupport,
            DefaultTeam,

            DamageType = 0,
            DoT,
            DeBuff,
            DeBurst,

            Heal,
            Shielding,
            Buff,
            Burst,

            Guarding,
            Counter,
            Revenge,

            ControlGain,
            Stance,
            Initiative
        }

    }
}
