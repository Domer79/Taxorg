using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataRepository;
using SqlClr;

namespace TaxorgRepository.Models
{
    public class TaxSummary : ModelBase
    {
        [Key]
        [Column("idOrganization")]
        public int IdOrganization { get; set; }

        [Column("inn")]
        public string Inn { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("shortName")]
        public string ShortName { get; set; }

        [Column("addr")]
        public string Address { get; set; }

        [Column("tax")]
        public decimal Tax { get; set; }

        [Column("taxDebitKredit")]
        public string TaxDebitKredit { get; set; }

        [Column("prevTax")]
        public decimal PrevTax { get; set; }

        [NotMapped]
        public decimal Delta
        {
            get { return Tax - PrevTax; }
//            get { return Math.Abs(Tax - PrevTax); }
        }

        [Column("period")]
        public YearMonth Period { get; set; }

        [Column("periodName")]
        public string PeriodName { get; set; }
        public override string ToString()
        {
            return string.Format("{0} долг {1}", ShortName, Tax);
        }
    }
}
