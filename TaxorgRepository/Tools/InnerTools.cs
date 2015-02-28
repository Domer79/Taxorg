﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlClr;
using TaxorgRepository.Models;

namespace TaxorgRepository.Tools
{
    internal class InnerTools
    {
        public static YearMonth GetCurrentPeriod(string connectionString)
        {
            var context = new TaxorgContext(connectionString);
            return GetCurrentPeriod(context);
        }

        public static YearMonth GetCurrentPeriod(TaxorgContext context)
        {
            var result = context.Database.SqlQuery<string>("select dbo.GetCurrentPeriod()").ToList();

            if (result.Count == 0)
                throw new Exception("Не настроен текущий период");

            return new YearMonth(result.First());

        }

        public static int GetPrevPeriodCount()
        {
            throw new NotImplementedException();
        }
    }
}