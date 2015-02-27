using TaxorgRepository.Interfaces;

namespace TaxorgRepository.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Organization")]
    public sealed class Organization : ModelBase
    {
        public Organization()
        {
            Tax = new HashSet<Tax>();
        }

        [Key]
        [Column("idOrganization")]
        public int IdOrganization { get; internal set; }

        [StringLength(900)]
        [Column("name")]
        public string Name { get; set; }

        [StringLength(200)]
        [Column("ShortName")]
        public string ShortName { get; set; }

        [StringLength(900)]
        [Column("addr")]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        [Column("inn")]
        public string Inn { get; set; }

        public ICollection<Tax> Tax { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", Name, ShortName);
        }
    }
}
