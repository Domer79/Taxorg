using System;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using SystemTools.Interfaces;

namespace SystemTools
{
    public static class ApplicationCustomizer
    {
        private static string _appVersion = "1.0.0.0";

        [DebuggerHidden]
        public static string ConnectionString
        {
            get { return ApplicationSettings.ConnectionString; }
            set { ApplicationSettings.ConnectionString = value; }
        }

        public static string ExcelFilePath
        {
            get { return ApplicationSettings.ExcelFilePath; }
        }

        public static bool LoggingDbContext
        {
            get { return ApplicationSettings.LoggingDbContext; }
            set { ApplicationSettings.LoggingDbContext = value; }
        }

        public static void RegisterErrorLog(Action<Exception> errorLog)
        {
            if (errorLog == null) 
                throw new ArgumentNullException("errorLog");

            SaveErrorLog = errorLog;
        }

        internal static Action<Exception> SaveErrorLog { get; set; }

        public static string AppVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }

        public static string SecurityConnectionString
        {
            get { return ApplicationSettings.SecurityConnectionString; }
            set { ApplicationSettings.SecurityConnectionString = value; }
        }

        public static IUser User { get; set; }

        public static void OnAuthenticated(IIdentity identity)
        {
            throw new NotImplementedException();
        }

        public static void OnAuthenticated(HttpCookie identity, HttpCookie passwordCookie)
        {
            throw new NotImplementedException();
        }
    }
}