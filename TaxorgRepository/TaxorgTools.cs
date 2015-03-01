using SqlClr;
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
    }
}
