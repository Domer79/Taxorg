using System;
using System.Diagnostics;
using System.Reflection;

namespace SystemTools
{
    public static class ApplicationCustomizer
    {
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

        public static Action<Exception> SaveErrorLog { get; set; }

        public static string AppVersion
        {
            get
            {
                var assembly = Assembly.GetCallingAssembly();

                var attr = (AssemblyVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyVersionAttribute));

                return attr == null ? "1.0.0.0" : attr.Version;
            }
        }
    }
}