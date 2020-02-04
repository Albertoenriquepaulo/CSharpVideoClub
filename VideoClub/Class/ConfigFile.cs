using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace VideoClub.Class
{
    class ConfigFile
    {
        //List<string> Data = new List<string>();
        public string[] AllKeys;

        public string[] GetAllSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            if (ThereAreElements())
            {
                return appSettings.AllKeys;
            }
            else
            {
                return new string[] { "ERROR" };
            }
        }

        //Verifica que hay elementos en el archivo de configuración
        public bool ThereAreElements()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        //Devuelve el valor del Key recibido
        public string GetKeyValue(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "Error";
            }
        }

        //Actualiza o Añade un elemento al archivo de configuración dado su key y su value
        public bool Update(string key, string value)
        {
            if (GetKeyValue(key) == "Not Found" || GetKeyValue(key) == "Error")
            {
                return false;
            }
            else
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                try
                {
                    settings[key].Value = value;
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                    return true;
                }
                catch (ConfigurationErrorsException)
                {
                    return false;
                }
            }
        }
        public bool Add(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            try
            {
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }
    }
}
