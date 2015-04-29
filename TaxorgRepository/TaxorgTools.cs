using SqlClr;
using TaxorgRepository.Repositories;
using TaxorgRepository.Tools;

namespace TaxorgRepository
{
    public class TaxorgTools
    {
        public static YearMonth GetCurrentPeriod()
        {
            return InnerTools.GetCurrentPeriod();
        }

        public static int GetPrevPeriodCount()
        {
            return InnerTools.GetPrevPeriodCount();
        }

        public static YearMonth GetPrevPeriod()
        {
            return InnerTools.GetPrevPeriod();
        }

        public static bool IsNotSameTaxLoad
        {
            get { return InnerTools.IsNotSameTaxLoad; }
            set { InnerTools.IsNotSameTaxLoad = value; }
        }

        public static string AppVersion
        {
            get { return InnerTools.AppVersion; }
        }

        public static void SetTaxPrevPeriodCount(int taxPrevPeriod)
        {
            InnerTools.SetTaxPrevPeriodCount(taxPrevPeriod);
        }

        public static bool IsMaintenance
        {
            get { return InnerTools.IsMaintenance; }
            set { InnerTools.IsMaintenance = value; }
        }

        public static string GetLastError()
        {
            return ErrorRepository.Errors.GetLastError();
        }
    }
}
