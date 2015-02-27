using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using SqlClr;

namespace TaxorgRepository.Models
{
    [Table("SliceTax")]
    public class SliceTax : ModelBase
    {
        [Key]
        public int IdTax { get; set; }
        public int IdOrganization { get; set; }
        public string OrganizationInn { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationShortName { get; set; }
        public int IdTaxType { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }

        [Column(TypeName = "money")]
        public decimal TaxSum { get; set; }

        [Column(TypeName = "money")]
        public decimal PrevTaxSum { get; set; }

        public string TaxDebitKredit { get; set; }
        
        public YearMonth Period { get; set; }

        public string PeriodName { get; set; }
    }
}
