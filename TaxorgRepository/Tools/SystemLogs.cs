using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using TaxorgRepository.Repositories;
using SystemTools;

namespace TaxorgRepository.Tools
{
    public class SystemLogs
    {
        public static async void SaveLogTaskAsync(string log)
        {
            await TaskToSaveLog(log);
        }

        [DebuggerStepThrough]
        public static void SaveLog(string log)
        {
            if (log == Environment.NewLine)
                return;

            using (var connection = new SqlConnection(ApplicationCustomizer.ConnectionString))
            {
                var command = new SqlCommand("Insert into SystemLog(log) values(@log)", connection);
                var p = command.Parameters.Add("log", SqlDbType.NVarChar);
                p.Value = log;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static Task TaskToSaveLog(string log)
        {
            return new Task(() => SaveLog(log));
        }
    }
}