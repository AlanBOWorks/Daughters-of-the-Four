
namespace __ProjectExclusive.Localizations
{
    public sealed class LocalizationSingleton 
    {
        private static readonly LocalizationSingleton Instance = new LocalizationSingleton();

        static LocalizationSingleton()
        {

        }
        private LocalizationSingleton() 
        { }

        public static LocalizationsEnum.Language GameLanguage { get; private set; }

        public static void SwitchLanguage(LocalizationsEnum.Language language)
        {
            GameLanguage = language;
        }




    }
}
