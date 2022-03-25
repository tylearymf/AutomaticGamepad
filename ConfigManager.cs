using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticGamepad
{
    internal class ConfigManager
    {
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        static StringBuilder s_StringBuilder = new StringBuilder();
        static string s_FilePath = "./config.ini";
        static string s_Group = "Config";
        static string s_Language = "Language";
        static string s_GamepadType = "GamepadType";


        public static void Init()
        {
            if (!File.Exists(s_FilePath))
            {
                File.WriteAllText(s_FilePath, string.Empty);

                Write(s_Language, Language.DefaultLanguage);
                Write(s_GamepadType, ((int)GamepadType.Xbox).ToString());
            }
        }

        public static void Write(string key, string value)
        {
            WriteINI(s_Group, key, value, s_FilePath);
        }

        public static string Read(string key)
        {
            return ReadINI(s_Group, key, string.Empty, s_FilePath);
        }

        public static GamepadType GetGamepadType()
        {
            var value = Read(s_GamepadType);
            int.TryParse(value, out var type);

            return (GamepadType)type;
        }

        public static string GetLanguage()
        {
            return Read(s_Language);
        }


        public static string ReadINI(string group, string key, string default_value, string filepath)
        {
            var sb = s_StringBuilder;

            sb.Clear();
            GetPrivateProfileString(group, key, default_value, sb, 255, filepath);
            return sb.ToString();
        }

        public static void WriteINI(string group, string key, string value, string filepath)
        {
            WritePrivateProfileString(group, key, value, filepath);
        }

    }
}
