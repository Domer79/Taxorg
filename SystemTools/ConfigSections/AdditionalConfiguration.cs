using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SystemTools.ConfigSections
{
    public class AdditionalConfiguration
    {
        private readonly string _configFilePath;
        private static AdditionalConfiguration _instance;

        public AdditionalConfiguration()
        {
            
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Object"/>.
        /// </summary>
        private AdditionalConfiguration(string configFilePath)
        {
            _configFilePath = configFilePath;
        }

        #region ConfigurationProperty members

        public SignPage SignPage
        {
            get { return (SignPage) GetSection("signPage"); }
        }

        public ErrorPage ErrorPage
        {
            get { return (ErrorPage) GetSection("errorPage"); }
        }

        #endregion

        private object GetSection(string sectionName)
        {
            return GetSection(sectionName, _configFilePath);
        }

        private static object GetSection(string sectionName, string configFilePath)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(configFilePath);
//            var config = ConfigurationManager.OpenExeConfiguration(configFilePath);

//            return ConfigurationManager.GetSection(sectionName);
            return config.GetSection(sectionName);
        }

        public static AdditionalConfiguration Instance
        {
            get { return _instance ?? (_instance = new AdditionalConfiguration("/web.config")); }
        }

        public string SecurityConnectionString
        {
            get { return ConfigurationManager.AppSettings["SecurityConnectionString"]; }
        }

        internal static AdditionalConfiguration GetAdditionalConfiguration(string configFilePath)
        {
            return _instance = new AdditionalConfiguration(configFilePath);
        }
    }
}
