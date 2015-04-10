namespace CP.TfsAssistant.Libraires
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// Loads/Saves configuration in local machine 
    /// </summary>
    public static class MySettingsManager
    {
        private static string _settingsFolder;
        private static ConcurrentDictionary<string, object> _settingsDic = new ConcurrentDictionary<string, object>();

        public static string SettingsFolder
        {
            get { return _settingsFolder; }
        }

        static MySettingsManager()
        {
            string value = System.Configuration.ConfigurationManager.AppSettings["MySettingsFolder"];
            if (string.IsNullOrEmpty(value))
            {
                _settingsFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            else if (Path.IsPathRooted(value))
            {
                _settingsFolder = value;
            }
            else if (value.StartsWith("$(") && value.IndexOf(")") > 2)
            {
                // value like "$(MyDocuments)Configuration"

                int index = value.IndexOf(")");
                string specialString = value.Substring(2, index - 2);
                Environment.SpecialFolder specialEnum;
                if (Enum.TryParse<Environment.SpecialFolder>(specialString, out specialEnum))
                {
                    string rest = value.Length > index ? value.Substring(index + 1, value.Length - index - 1) : string.Empty;
                    _settingsFolder = Path.Combine(Environment.GetFolderPath(specialEnum), rest.TrimStart(new char[] { '\\' }));
                }
                else
                {
                    throw new FormatException(string.Format("Can't parse the special folder $({0})!", specialString));
                }
            }
            else
            {
                _settingsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), value);
            }
        }

        public static T GetSettings<T>(string subFolder = null)
            where T : ISettings, new()
        {
            string partialPath = null;
            if (string.IsNullOrEmpty(subFolder))
            {
                partialPath = string.Format(@"{0}.xml", typeof(T).FullName);
            }
            else
            {
                partialPath = string.Format(@"{0}\{1}.xml", subFolder, typeof(T).FullName);
            }

            var fullPath = Path.Combine(SettingsFolder, partialPath);

            if (!_settingsDic.Keys.Contains(fullPath))
            {
                var settings = LoadSettings<T>(fullPath);
                _settingsDic.TryAdd(fullPath, settings);
            }
            return (T)_settingsDic[fullPath];
        }

        public static void SaveAll()
        {
            foreach (var item in _settingsDic)
            {
                Write(item.Key, item.Value as ISettings);
            }
        }

        public static void SaveSingle(ISettings settings)
        {
            if (settings == null)
            {
                return;
            }

            foreach (var item in _settingsDic)
            {
                if (settings.Equals(item.Value))
                {
                    Write(item.Key, item.Value as ISettings);
                    return;
                }
            }
        }

        private static T LoadSettings<T>(string fullPath)
            where T : ISettings, new()
        {
            T settings;
            if (!File.Exists(fullPath))
            {
                settings = new T();
                settings.Initialize();
            }
            else
            {
                settings = Read<T>(fullPath);
            }
            return settings;
        }

        /// <summary>
        /// Writes settings to file.
        /// </summary>
        private static void Write(string fullPath, ISettings settings)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            using (var writer = new StreamWriter(fullPath))
            {
                var serializer = new XmlSerializer(settings.GetType());
                serializer.Serialize(writer, settings);
            }
        }

        /// <summary>
        /// Reads settings from file
        /// </summary>
        /// <returns>Instance of type T</returns>
        private static T Read<T>(string fullPath)
            where T : ISettings, new()
        {
            T settings;
            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    settings = (T)serializer.Deserialize(fs);
                    return settings;
                }
            }
            catch
            {
                var t = new T();
                t.Initialize();
                Write(fullPath, t);
                return t;
            }
        }
    }
}
