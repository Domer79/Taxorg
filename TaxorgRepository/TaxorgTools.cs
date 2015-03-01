using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlClr;
using TaxorgRepository.Models;
using TaxorgRepository.Tools;
using SystemTools;

namespace TaxorgRepository
{
    public class TaxorgTools
    {
        

        public static YearMonth GetCurrentPeriod()
        {
            return InnerTools.GetCurrentPeriod(ApplicationCustomizer.ConnectionString);
        }

        public static int GetPrevPeriodCount()
        {
            return InnerTools.GetPrevPeriodCount(ApplicationCustomizer.ConnectionString);
        }
    }
}
