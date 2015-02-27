using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxorgRepository.Models;

namespace TaxorgRepository.Repositories
{
    internal class ErrorRepository : RepositoryBase<Error>
    {
        private static ErrorRepository _instance;

        public void SaveError(string errorMessage)
        {
            var error = Set.Create();
            error.TypeError = "DbContextLog";
            error.Message = errorMessage;

            Set.Add(error);
            SaveChanges();
        }

        public void SaveError(Exception e)
        {
            var error = Set.Create();
            error.TypeError = e.GetType().FullName;
            error.Message = e.Message;
            error.StackTrace = e.StackTrace;

            Set.Add(error);
            SaveChanges();
        }

        private ErrorRepository()
        {
        }

        public static ErrorRepository Errors
        {
            get { return _instance ?? (_instance = new ErrorRepository()); }
        }
    }

    public class ErrorLog
    {
        public static void SaveError(string errorMessage)
        {
            ErrorRepository.Errors.SaveError(errorMessage);
        }

        public static void SaveError(Exception e)
        {
            ErrorRepository.Errors.SaveError(e);
        }
    }
}
