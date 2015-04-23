using System;
using System.Configuration;
using System.Data.SqlClient;
using SystemTools.ConfigSections;

namespace SystemTools
{
    public class ApplicationSettings
    {
        private static string _connectionString;
        private static string _excelFilePath;
        private static bool? _loggingDbContext;
        private static string _securityConnectionString;
        private static string _securityControllerName;

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[GetConnectionStringName()].ConnectionString;
        }

        private static string GetSecurityConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[GetSecurityConnectionStringName()] == null
                ? null
                : ConfigurationManager.ConnectionStrings[GetSecurityConnectionStringName()].ConnectionString;
        }

        private static string GetExcelFilePath()
        {
            return ConfigurationManager.AppSettings["ExcelFilePath"];
        }

        private static string GetConnectionStringName()
        {
            return ConfigurationManager.AppSettings["ConnectionStringName"]; 
        }

        private static string GetSecurityConnectionStringName()
        {
            return ConfigurationManager.AppSettings["SecurityConnectionString"];
        }

        internal static string ExcelFilePath
        {
            get { return _excelFilePath ?? (_excelFilePath = GetExcelFilePath()); }
        }

        public static string ConnectionString
        {
            get
            {
                return _connectionString ?? (_connectionString = GetConnectionString());
            }
            set { _connectionString = value; }
        }

        public static bool LoggingDbContext
        {
            get
            {
                try
                {
                    return (bool) (_loggingDbContext ?? (_loggingDbContext = GetAppSettings<bool>("LoggingDbContext")));
                }
                catch
                {
                    return false;
                }
            }
            set { _loggingDbContext = value; }
        }

        public static string SecurityConnectionString
        {
            get { return _securityConnectionString ?? (_securityConnectionString = GetSecurityConnectionString());}
            set { _securityConnectionString = value; }
        }

        public static SignPage SignPage
        {
            get { return AdditionalConfiguration.Instance.SignPage; }
        }

        public static ErrorPage ErrorPage
        {
            get { return AdditionalConfiguration.Instance.ErrorPage; }
        }

        public static string SecurityControllerName
        {
            get { return _securityControllerName ?? (_securityControllerName = GetAppSettings<string>("SecurityControllerName")); }
        }

        private static T GetAppSettings<T>(string parameterName)
        {
            var appSetting = ConfigurationManager.AppSettings[parameterName];
            return appSetting != null ? (T)Convert.ChangeType(appSetting, typeof(T)) : default(T);
        }

        public static string DatabaseName
        {
            get
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
                return connectionStringBuilder.InitialCatalog;
            }
        }
    }
}