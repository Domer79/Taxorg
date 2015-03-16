using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxorgRepository.Infrastructure
{
    public class Interceptor : IDbCommandInterceptor
    {
        public Interceptor()
        {
                Debug.WriteLine("Create new Interceptor");
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
//            throw new NotImplementedException();
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
//            throw new NotImplementedException();
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            try
            {
                if (interceptionContext.Result != null)
                {
                    var dataTable = interceptionContext.Result.GetSchemaTable();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            try
            {
                var dataTable = interceptionContext.Result.GetSchemaTable();
            }
            catch (Exception e)
            {
//                Debug.WriteLine(e);
            }
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
//            throw new NotImplementedException();
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
//            throw new NotImplementedException();
        }
    }
}
