using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataRepository;

namespace TaxorgRepository.Models
{
    [Table("Bug")]
    public class Bug : ModelBase
    {
        [Key]
        [Column("idBug")]
        public int IdBug { get; set; }

        [Column("data")]
        public string ErrorData { get; set; }

        [Column("cause")]
        public string Cause { get; set; }

        [Column("timeLoad")]
        public DateTime TimeLabel { get; set; }

        [Column("accept")]
        public bool Accept { get; set; }

        public override int GetHashCode()
        {
            return IdBug.GetHashCode();
        }
    }
}