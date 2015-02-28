using SqlClr;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxorgRepository.Models
{
    [Table("Tax")]
    public class Tax : ModelBase
    {
        [Key]
        [Column("idTax")]
        public int IdTax { get; set; }

        [Column("idOrganization")]
        public int? IdOrganization { get; set; }

        [Column("idTaxType")]
        public int IdTaxType { get; set; }

        [Column("tax", TypeName = "money")]
        public decimal TaxSum { get; set; }

        [Column("period")]
        public YearMonth Period { get; set; }

        [Column("periodName")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PeriodName { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual TaxType TaxType { get; set; }
    }
}
