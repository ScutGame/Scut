using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scut.Client.Runtime
{
    class ConfigurationUtils
    {
        private static ConfigurationUtils _instance = new ConfigurationUtils();
        private Configuration _config;

        public static ConfigurationUtils GetInstance()
        {
            return _instance;
        }

        private ConfigurationUtils()
        {
            string fileName = System.IO.Path.GetFileName(Application.ExecutablePath);
            _config = ConfigurationManager.OpenExeConfiguration(fileName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool addSetting(string key, string value)
        {
            _config.AppSettings.Settings.Add(key, value);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getSetting(string key)
        {
          return  _config.AppSettings.Settings[key].Value;
        }

        public bool updateSeeting(string key, string newValue)
        {
            _config.AppSettings.Settings[key].Value = newValue;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void save()
        {
            _config.Save();
        }
    }
}
