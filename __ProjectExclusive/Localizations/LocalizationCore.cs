using UnityEngine;

namespace __ProjectExclusive.Localizations
{
    public static class LocalizationsEnum 
    {
        public enum Language
        {
            dev_en = DevEnglishIndex,
            en_en = EnglishDefaultIndex
        }

        public const int DevEnglishIndex = 0;
        public const int EnglishDefaultIndex = 10;


        public const string ID_DevEnglish = "dev_en";
        public const string ID_EnglishDefault = "en_en";
    }

    public interface ILocalizationStructure<T> : ILocalizationStructureRead<T>, ILocalizationStructureInject<T>
    {
        new T DevEnglish { set;get; }
        new T EnglishDefault { set; get; }
    }
    public interface ILocalizationStructureRead<out T>
    {
        T DevEnglish { get; }
        T EnglishDefault { get; }
    }
    public interface ILocalizationStructureInject<in T>
    {
        T DevEnglish { set; }
        T EnglishDefault { set; }
    }
}
