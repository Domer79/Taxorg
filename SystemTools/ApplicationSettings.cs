using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
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
        private static string _applicationName;
        private static bool _enableSecurityAdminPanel;

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
            return GetAppSettings<string>("ExcelFilePath");
        }

        private static string GetConnectionStringName()
        {
            return GetAppSettings<string>("ConnectionStringName");
        }

        private static string GetSecurityConnectionStringName()
        {
            return GetAppSettings<string>("SecurityConnectionString");
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

        public static string ApplicationName
        {
            get
            {
                return _applicationName ?? (_applicationName = GetApplicationName() ?? "/");
            }
            set { _applicationName = value; }
        }

        public static string ApplicationVirtualPath
        {
            get
            {
                return ApplicationName == "/" ? "/" : string.Format("/{0}/", ApplicationName);
            }
        }

        public static string WebConfigVirtualFilePath
        {
            get
            {
                return string.Format("{0}web.config", ApplicationVirtualPath);
            }
        }

        private static string GetApplicationName()
        {
            return ConfigurationManager.AppSettings["AppName"] ?? GetApplicationNameFromHttpRuntime();
        }

        private static string GetApplicationNameFromHttpRuntime()
        {
            const string pattern = @"(?<appname>[\w]+)|/$";
            if (HttpRuntime.AppDomainAppVirtualPath == null)
                return "/";

            var input = HttpRuntime.AppDomainAppVirtualPath;

            var rx = new Regex(pattern);
            if (!rx.IsMatch(input))
                return "/";

            return rx.Match(input).ToString();
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

        public static bool EnableSecurityAdminPanel
        {
            get { return _enableSecurityAdminPanel = GetEnableSecurityAdminPanel(); }
            set { _enableSecurityAdminPanel = value; }
        }

        private static bool GetEnableSecurityAdminPanel()
        {
            return GetAppSettings<bool>("EnableSecurityAdminPanel");
        }
    }
}