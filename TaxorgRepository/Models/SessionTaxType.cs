using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxorgRepository.Models
{
    [Table("SessionTaxType")]
    public class SessionTaxType : ModelBase
    {
        [Key]
        public int IdSessionTaxType { get; set; }
        public string SessionId { get; set; }
        public int IdTaxType { get; set; }

        public virtual Session Session { get; set; }
        public virtual TaxType TaxType { get; set; }
    }
}