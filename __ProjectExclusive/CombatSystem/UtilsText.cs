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

        /// <summary>
        /// Tries to get a single digit; If is not a single digit, it will be cast like a normal string
        /// </summary>
        public static string TryGetSingleDigit(int targetDigit)
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
                _ => targetDigit.ToString()
            };
        }

        private const string RoundZero = "00";
        private const string RoundOne = "01";
        private const string RoundTwo = "02";
        private const string RoundThree = "03";
        private const string RoundFour = "04";
        private const string RoundFive = "05";
        private const string RoundSix = "06";
        private const string RoundSeven = "07";
        private const string RoundEight = "08";
        private const string RoundNine = "09";

        private const string RoundTen = "10";
        private const string RoundEleven = "11";
        private const string RoundTwelve = "12";
        private const string RoundThirteen = "13";
        private const string RoundFourteen = "14";
        private const string RoundFifteen = "15";
        private const string RoundSixteen = "16";
        private const string RoundSeventeen = "17";
        private const string RoundEighteen = "18";
        private const string RoundNineteen = "19";

        private const string RoundTwentyOne = "21";
        private const string RoundTwentyTwo = "22";
        private const string RoundTwentyThree = "23";
        private const string RoundTwentyFour = "24";

        private const string RoundOfRange = "XX";

        public static string GetRoundDigit(int targetDigit)
        {
            return targetDigit switch
            {
                0 => RoundZero,
                1 => RoundOne,
                2 => RoundTwo,
                3 => RoundThree,
                4 => RoundFour,
                5 => RoundFive,
                6 => RoundSix,
                7 => RoundSeven,
                8 => RoundEight,
                9 => RoundNine,

                10 => RoundTen,
                11 => RoundEleven,
                12 => RoundTwelve,
                13 => RoundThirteen,
                14 => RoundFourteen,
                15 => RoundFifteen,
                16 => RoundSixteen,
                17 => RoundSeventeen,
                18 => RoundEighteen,
                19 => RoundNineteen,

                21 => RoundTwentyOne,
                22 => RoundTwentyTwo,
                23 => RoundTwentyThree,
                24 => RoundTwentyFour,

                _ => RoundOfRange
            };
        }

        /// <summary>
        /// Tries to get a 'round' digit; If is [out of range] digit, it will be cast like a normal string
        /// </summary>
        public static string TryGetRoundDigit(int targetDigit)
        {
            return targetDigit switch
            {
                0 => RoundZero,
                1 => RoundOne,
                2 => RoundTwo,
                3 => RoundThree,
                4 => RoundFour,
                5 => RoundFive,
                6 => RoundSix,
                7 => RoundSeven,
                8 => RoundEight,
                9 => RoundNine,

                10 => RoundTen,
                11 => RoundEleven,
                12 => RoundTwelve,
                13 => RoundThirteen,
                14 => RoundFourteen,
                15 => RoundFifteen,
                16 => RoundSixteen,
                17 => RoundSeventeen,
                18 => RoundEighteen,
                19 => RoundNineteen,

                21 => RoundTwentyOne,
                22 => RoundTwentyTwo,
                23 => RoundTwentyThree,
                24 => RoundTwentyFour,

                _ => targetDigit.ToString("00")
            };
        }
    }
}
