using UnityEngine;

namespace Localization.Characters
{
    public static class LocalizationPlayerCharacters
    {
        public const string NullName = "NULL";
        public static string LocalizeCharactersName(string entityName)
        {
            if (entityName == null) return NullName;

            string localization = entityName;
            //todo make localization logic
            return localization;
        }
    }

}
