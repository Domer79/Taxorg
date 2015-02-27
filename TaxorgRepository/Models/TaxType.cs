namespace TaxorgRepository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaxType")]
    public class TaxType : ModelBase
    {
        public TaxType()
        {
            Tax = new HashSet<Tax>();
        }

        [Key]
        [Column("idTaxType")]
        public int IdTaxType { get; set; }

        [Required]
        [StringLength(100)]
        [Column("code")]
        public string Code { get; set; }

        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Tax> Tax { get; set; }
    }
}
