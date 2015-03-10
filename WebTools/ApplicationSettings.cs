using System.Configuration;

namespace SystemTools
{
    public class ApplicationSettings
    {
        private static string _connectionString;
        private static string _excelFilePath;
        private static bool? _loggingDbContext;
        private static string _securityConnectionString;

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[GetConnectionStringName()].ConnectionString;
        }

        private static string GetExcelFilePath()
        {
            return ConfigurationManager.AppSettings["ExcelFilePath"];
        }

        private static string GetConnectionStringName()
        {
            return ConfigurationManager.AppSettings["GetConnectionStringName"]; 
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
                    return (bool) (_loggingDbContext ?? (_loggingDbContext = GetAppSettings("LoggingDbContext")));
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

        private static string GetSecurityConnectionString()
        {
            return ConfigurationManager.AppSettings["SecurityConnectionString"];
        }

        private static bool GetAppSettings(string parameterName)
        {
            bool logging;
            bool.TryParse(ConfigurationManager.AppSettings[parameterName], out logging);
            return logging;
        }
    }
}