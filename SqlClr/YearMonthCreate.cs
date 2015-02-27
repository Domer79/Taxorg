using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace SqlClr
{
    public class UserDefinedFunctions
    {
        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
        public static YearMonth YearMonthCreateByDateTime(SqlDateTime dateTime)
        {
            if (dateTime.IsNull)
                throw new NullReferenceException("dateTime");

            return new YearMonth(dateTime.Value);
        }

        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
        public static YearMonth YearMonthCreate(SqlInt32 year, SqlInt32 month)
        {
            if (year.IsNull || month.IsNull)
                throw new NullReferenceException("Входные данные не могут NULL");

            var yearMonth = new YearMonth{Year = year.Value, Month = month.Value};
            if (!yearMonth.ValidateYearMonth())
                throw new InvalidCastException("Не верный формат даты");

            return yearMonth;
        }
    }
}
