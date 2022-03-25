using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace AutomaticGamepad
{
    public class Language
    {
        public const string English = "English";
        public const string Chinese = "Chinese";
        public const string DefaultLanguage = English;

        static ResourceManager s_ResourceManager;

        public static void Init()
        {
            var language = ConfigManager.GetLanguage();
            var languageFileName = string.Empty;
            var cultureName = string.Empty;

            if (string.Compare(language, English, true) == 0)
                languageFileName = Language.English;
            else if (string.Compare(language, Chinese, true) == 0)
                languageFileName = Language.Chinese;
            else
                languageFileName = DefaultLanguage;

            CultureInfo culture = null;
            switch (languageFileName)
            {
                case Chinese:
                    culture = new CultureInfo("zh-CN");
                    break;
                case English:
                    culture = new CultureInfo("en-US");
                    break;
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            s_ResourceManager = new ResourceManager($"AutomaticGamepad.Properties.Language_{languageFileName}", typeof(Language).Assembly);
        }

        public static string GetString(string key, params string[] args)
        {
            return string.Format(s_ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture), args);
        }
    }
}
