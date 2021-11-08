using UnityEngine;

namespace __ProjectExclusive
{
    public static class UtilsText
    {
        private const string Zero = "0";
        private const string One = "1";
        private const string Two = "2";
        private const string Three = "3";
        private const string Four = "4";
        private const string Five = "5";
        private const string Six = "6";
        private const string Seven = "7";
        private const string Eight = "8";
        private const string Nine = "9";
        private const string NinePlus = "X";
        public static string GetSingleDigit(int targetDigit)
        {
            return targetDigit switch
            {
                0 => Zero,
                1 => One,
                2 => Two,
                3 => Three,
                4 => Four,
                5 => Five,
                6 => Six,
                7 => Seven,
                8 => Eight,
                9 => Nine,
                _ => NinePlus
            };
        }
    }
}
