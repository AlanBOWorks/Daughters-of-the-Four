using UnityEngine;

namespace Localization.Characters
{
    public static class LocalizationPlayerCharacters 
    {
        public static string LocalizeCharactersName(ILocalizableCharacter localizable)
        {
            string localization = localizable.GetLocalizableCharactersName();
            //todo make localization logic
            return localization;
        }
    }

    public interface ILocalizableCharacter
    {
        string GetLocalizableCharactersName();
    }
}
