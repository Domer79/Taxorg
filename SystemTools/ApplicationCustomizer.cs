using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using SystemTools.ConfigSections;
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

        public static ISecurity Security { get; set; }
        public static bool EnableSecurity { get; set; }
        public static bool IsError { get; set; }

        public static bool EnableSecurityAdminPanel
        {
            get { return ApplicationSettings.EnableSecurityAdminPanel; }
            set
            {
                if (value && string.IsNullOrEmpty(ApplicationSettings.SecurityControllerName))
                    throw new InvalidOperationException("�������� SecurityControllerName �� �������� � ����� web.config.");

                ApplicationSettings.EnableSecurityAdminPanel = value;
            }
        }

        public static ApplicationType ApplicationType { get; set; }
        public static string CurrentSessionId { get; set; }
    }

    public enum ApplicationType
    {
        Windows,
        Web
    }
}